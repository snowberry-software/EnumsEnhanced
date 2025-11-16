using System.Collections.Immutable;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EnumsEnhanced;

[Generator]
internal class EnumsEnhanced : IIncrementalGenerator
{
    public static readonly DiagnosticDescriptor UnderylingEnumerationTypeNotFound
      = new("EE001",
            "Underlying Enumeration Type not found",
            "The underlying type of the enumeration '{0}' could not be resolved",
            nameof(EnumsEnhanced),
            DiagnosticSeverity.Error,
            true);

    /// <inheritdoc/>
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var enumDeclarationsProvider = context.SyntaxProvider.CreateSyntaxProvider(
            static (n, _) => n is EnumDeclarationSyntax,
            static (n, _) => ((INamedTypeSymbol)n.SemanticModel.GetDeclaredSymbol(n.Node)!, (EnumDeclarationSyntax)n.Node)
        );

        context.RegisterSourceOutput(enumDeclarationsProvider.Collect(), static (sourceProductionContext, enumDeclarations) =>
        {
            foreach (var pair in enumDeclarations)
            {
                var symbol = pair.Item1;
                var eds = pair.Item2;

                if (symbol.ContainingType != null)
                    continue;

                var membersSymbols = symbol
                    .GetMembers()
                    .Where(x => x is IFieldSymbol)
                    .Cast<IFieldSymbol>()
                    .ToImmutableArray();

                var sb = new StringBuilder();

                GenerateEnumMethods(sourceProductionContext, eds, symbol, membersSymbols, sb);

                string classCode = GetClassTemplate(eds, symbol, out string? className)
                    .Replace("{CLASS_BODY}", sb.ToString());

                sourceProductionContext.AddSource($"{className}.g.cs", classCode.Trim());
            }
        });
    }

    private static void GenerateEnumMethods(SourceProductionContext sourceProductionContext, EnumDeclarationSyntax enumDeclarationSyntax, INamedTypeSymbol enumSymbol, IList<IFieldSymbol> memberSymbols, StringBuilder methodSb)
    {
        var methodImplAttributeText = new StringBuilder();
        methodImplAttributeText.AppendLine("#if NETCOREAPP3_0_OR_GREATER");
        //methodImplAttributeText.AppendLine($"[{nameof(MethodImplAttribute)}({nameof(MethodImplOptions)}.{nameof(MethodImplOptions.AggressiveInlining)} | {nameof(MethodImplOptions)}.AggressiveOptimization)]");
        methodImplAttributeText.AppendLine("#else");
        methodImplAttributeText.AppendLine($"[{nameof(MethodImplAttribute)}({nameof(MethodImplOptions)}.{nameof(MethodImplOptions.AggressiveInlining)})]");
        methodImplAttributeText.AppendLine("#endif");

        const string hasFlagMethodName = "HasFlagFast";

        var enumUnderlyingType = enumSymbol.EnumUnderlyingType;

        if (enumUnderlyingType == null)
        {
            sourceProductionContext.ReportDiagnostic(
                Diagnostic.Create(UnderylingEnumerationTypeNotFound,
                enumDeclarationSyntax.Identifier.GetLocation(),
                enumDeclarationSyntax.Identifier.ToString()));

            return;
        }

        var enumUnderlyingTypeSpecial = enumUnderlyingType.SpecialType;

        bool isByteCheck = enumUnderlyingTypeSpecial is
            SpecialType.System_Byte or SpecialType.System_SByte or
            SpecialType.System_Boolean or SpecialType.System_Char;

        // HasFlagFast
        {
            methodSb.AppendLine($$"""

                /// <summary>
                /// Determines whether one or more bit fields are set in the current instance.
                /// </summary>
                /// <param name="e">The value of the enum.</param>
                /// <param name="flag">The flag to check.</param>
                /// <returns><see langword="true"/> if the bit field or bit fields that are set in flag are also set in the current instance; otherwise, false.</returns>
                {{methodImplAttributeText}}
                public static bool {{hasFlagMethodName}}(this {{enumSymbol.Name}} e, {{enumSymbol.Name}} flag)
                {
            #if NETCOREAPP3_0_OR_GREATER
                    {{enumUnderlyingType.Name}} flagsValue = Unsafe.As<{{enumSymbol.Name}}, {{enumUnderlyingType.Name}}>(ref flag);
                    return (Unsafe.As<{{enumSymbol.Name}}, {{enumUnderlyingType.Name}}>(ref e) & flagsValue) == flagsValue;
            #else
                    return (({{enumUnderlyingType.Name}})e & ({{enumUnderlyingType.Name}})flag) == ({{enumUnderlyingType.Name}})flag;
            #endif
                }
            """);
        }

        // GetNamesFast
        const string getNamesMethodName = "GetNamesFast";
        methodSb.AppendLine($$"""

            /// <summary>
            /// Retrieves an array of the names of the constants.
            /// </summary>
            /// <returns>A string array of the names of the constants.</returns>
            public static string[] {{getNamesMethodName}}()
            {
                return new string[] {
                    {{string.Join(", ", memberSymbols.Select(x => $"\"{x.Name}\""))}}
                };
            }

        """);

        // GetValuesFast
        const string getValuesMethodName = "GetValuesFast";
        methodSb.AppendLine($$"""

            /// <summary>
            /// Retrieves an array of the values of the constants.
            /// </summary>
            /// <returns>An array that contains the values of the constants.</returns>
            public static {{enumSymbol.Name}}[] {{getValuesMethodName}}()
            {
                return new {{enumSymbol.Name}}[] {
                    {{string.Join(", ", memberSymbols.Select(x => $"{enumSymbol.Name}.{x.Name}"))}}
                };
            }

        """);

        // IsDefinedFast
        var constantValuesChecked = new List<object>();
        var switchCases = new StringBuilder();
        {
            const string isDefinedMethodName = "IsDefinedFast";

            var switchCasesValue = new StringBuilder();

            foreach (var member in memberSymbols.OrderBy(x => x.Name.Length))
            {
                string memberRef = $"{enumSymbol.Name}.{member.Name}";

                switchCases.AppendLine($"case \"{member.Name}\":");
                switchCases.AppendLine("\treturn true;");

                if (!member.HasConstantValue)
                    continue;

                if (constantValuesChecked.Contains(member.ConstantValue))
                {
                    string skipText = $"// Skipping duplicated constant value: {memberRef} -> {member.ConstantValue}";
                    switchCasesValue.AppendLine(skipText);
                    switchCasesValue.AppendLine();
                    continue;
                }

                switchCasesValue.AppendLine($"case {memberRef}:");
                switchCasesValue.AppendLine("\treturn true;");

                constantValuesChecked.Add(member.ConstantValue);
            }

            constantValuesChecked.Clear();

            methodSb.AppendLine($$"""

                /// <inheritdoc cref="{{isDefinedMethodName}}({{enumSymbol.Name}})"/>
                public static bool {{isDefinedMethodName}}(string value)
                {
                    _ = value ?? throw new ArgumentNullException(nameof(value));

                    switch(value)
                    {
                        {{switchCases}}
                    }

                    return false;
                }

                /// <inheritdoc cref="{{isDefinedMethodName}}({{enumSymbol.Name}})"/>
                {{methodImplAttributeText}}
                public static bool {{isDefinedMethodName}}({{enumUnderlyingType.Name}} value)
                {
                    return {{isDefinedMethodName}}(({{enumSymbol.Name}})value);
                }

                /// <summary>
                /// Returns a <see cref="bool"/> telling whether its given value exists in the enumeration.
                /// </summary>
                /// <param name="value">The value of the enumeration constant.</param>
                /// <returns><see langword="true"/> if a constant is defined with the given value from the <paramref name="value"/>.</returns>
                public static bool {{isDefinedMethodName}}({{enumSymbol.Name}} value)
                {
                    switch(value)
                    {
                        {{switchCasesValue}}
                    }

                    return false;
                }
            """);
        }

        // GetNameFast
        switchCases.Clear();
        {
            const string getNameMethodName = "GetNameFast";
            const string toStringMethodName = "ToStringFast";
            var flagCases = new StringBuilder();

            const string separator = $", ";

            for (int i = 0; i < memberSymbols.Count; i++)
            {
                var member = memberSymbols[i];
                string memberRef = $"{enumSymbol.Name}.{member.Name}";

                if (member.HasConstantValue)
                {
                    if (constantValuesChecked.Contains(member.ConstantValue))
                    {
                        string skipText = $"// Skipping duplicated constant value: {memberRef} -> {member.ConstantValue}";
                        switchCases.AppendLine(skipText);
                        switchCases.AppendLine();

                        flagCases.AppendLine(skipText);
                        flagCases.AppendLine();
                        continue;
                    }

                    constantValuesChecked.Add(member.ConstantValue);
                }

                switchCases.AppendLine($"case {memberRef}:");
                switchCases.AppendLine($"\treturn nameof({memberRef});");
                switchCases.AppendLine();

                flagCases.AppendLine(@$"if(e.{hasFlagMethodName}({memberRef}))");
                flagCases.AppendLine("\t" + @$"flagBuilder.{nameof(StringBuilder.Append)}({memberRef}.{getNameMethodName}(false) + ""{separator}"");");
                flagCases.AppendLine();
            }

            methodSb.AppendLine($$"""

                /// <summary>
                /// Resolves the name of the given enum value.
                /// </summary>
                /// <param name="e">The value of a particular enumerated constant in terms of its underlying type.</param>
                /// <param name="includeFlagNames">Determines whether the value has flags, so it will return `EnumValue, EnumValue2`.</param>
                /// <returns> A string containing the name of the enumerated constant or <see langword="null"/> if the enum has multiple flags set but <paramref name="includeFlagNames"/> is not enabled.</returns>
                public static string? {{getNameMethodName}}(this {{enumSymbol.Name}} e, bool includeFlagNames = false)
                {
                    switch(e)
                    {
                        {{switchCases}}
                    }

                    // Returning null is the default behavior.
                    if(!includeFlagNames)
                        return null;
                        //throw new Exception("Enum name could not be found!");

                    var flagBuilder = new StringBuilder();
                    {{flagCases}}

                    return flagBuilder.ToString().Trim(new char[] { ',', ' ' });
                }

                /// <summary>
                /// Converts the value of this instance to its equivalent string representation.
                /// </summary>
                /// <param name="e">The value of a particular enumerated constant in terms of its underlying type.</param>
                /// <returns>The string representation of the value of this instance.</returns>
                {{methodImplAttributeText}}
                public static string? {{toStringMethodName}}(this {{enumSymbol.Name}} e)
                {
                    return {{getNameMethodName}}(e, true);
                }
            """);
        }

        // ParseFast
        {
            const string parseMethodName = "ParseFast";
            const string tryParseMethodName = "TryParseFast";
            switchCases.Clear();

            var ifCases = new StringBuilder();
            var valueSwitchCases = new StringBuilder();
            constantValuesChecked.Clear();

            foreach (var member in memberSymbols.OrderBy(x => x.Name.Length).Cast<IFieldSymbol>())
            {
                string memberRef = $"{enumSymbol.Name}.{member.Name}";

                switchCases.AppendLine($"case nameof({memberRef}):");
                switchCases.AppendLine($"\tparsed = true;");
                switchCases.AppendLine($"\tlocalResult |= ({enumUnderlyingType.Name}){memberRef};");
                switchCases.AppendLine($"\tbreak;");
                switchCases.AppendLine();

                ifCases.AppendLine($"if(subValue.Equals(\"{member.Name}\", {nameof(StringComparison)}.{nameof(StringComparison.OrdinalIgnoreCase)})) {{");
                ifCases.AppendLine($"\tparsed = true;");
                ifCases.AppendLine($"\tlocalResult |= ({enumUnderlyingType.Name}){memberRef}; }}");

                if (!member.HasConstantValue)
                    continue;

                if (constantValuesChecked.Contains(member.ConstantValue))
                {
                    string skipText = $"// Skipping duplicated constant value: {memberRef} -> {member.ConstantValue}";
                    valueSwitchCases.AppendLine(skipText);
                    valueSwitchCases.AppendLine();
                    continue;
                }

                valueSwitchCases.AppendLine($"case ({enumUnderlyingType.Name}){memberRef}:");
                valueSwitchCases.AppendLine($"\tsuccessful = true;");
                valueSwitchCases.AppendLine($"\treturn {memberRef};");
                valueSwitchCases.AppendLine();

                constantValuesChecked.Add(member.ConstantValue);
            }

            methodSb.AppendLine($$"""

                /// <summary>
                /// Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object.
                /// </summary>
                /// <param name="value">A string containing the name or value to convert.</param>
                /// <param name="ignoreCase"><see langword="true"/> to ignore case; false to regard case.</param>
                /// <param name="result">The result of the enumeration constant.</param>
                /// <returns><see langword="true"/> if the conversion succeeded; <see langword="false"/> otherwise.</returns>
                public static bool {{tryParseMethodName}}(string value, bool ignoreCase, out {{enumSymbol.Name}} result)
                {
                    result = {{parseMethodName}}(out var successful, value: value, ignoreCase: ignoreCase, throwOnFailure: false);
                    return successful;
                }

                /// <summary>
                /// Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object.
                /// </summary>
                /// <param name="value">A string containing the name or value to convert.</param>
                /// <param name="ignoreCase"><see langword="true"/> to ignore case; false to regard case.</param>
                /// <returns>The enumeration value whose value is represented by the given value.</returns>
                public static {{enumSymbol.Name}} {{parseMethodName}}(string value, bool ignoreCase = false)
                {
                    return {{parseMethodName}}(out _, value: value, ignoreCase: ignoreCase, throwOnFailure: true);
                }

                /// <summary>
                /// Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object.
                /// </summary>
                /// <param name="successful"><see langword="true"/> if the conversion succeeded; <see langword="false"/> otherwise.</param>
                /// <param name="value">A string containing the name or value to convert.</param>
                /// <param name="ignoreCase"><see langword="true"/> to ignore case; false to regard case.</param>
                /// <param name="throwOnFailure">Determines whether to throw an <see cref="Exception"/> on errors or not.</param>
                /// <returns>The enumeration value whose value is represented by the given value.</returns>
                public static {{enumSymbol.Name}} {{parseMethodName}}(out bool successful, string value, bool ignoreCase = false, bool throwOnFailure = true)
                {
                    successful = false;

                    if (throwOnFailure && (string.{{nameof(string.IsNullOrEmpty)}}(value) || string.{{nameof(string.IsNullOrWhiteSpace)}}(value)))
                    {
                        throw new {{nameof(ArgumentException)}}("Value can't be null or whitespace!", nameof(value));
                    }

                    {{enumUnderlyingType.Name}} localResult = 0;
                    bool parsed = false;
                    string subValue;
                    string originalValue = value;
                    char firstChar = value[0];

                    if (char.{{nameof(char.IsDigit)}}(firstChar) || firstChar == '-' || firstChar == '+')
                    {
                        if({{enumUnderlyingType.Name}}.TryParse(value, NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingWhite, null, out var valueNumber))
                            switch(valueNumber)
                            {
                                {{valueSwitchCases}}
                            }
                    }
                    else
                    while(value != null && value.Length > 0)
                    {
                        parsed = false;

                        int endIndex = value.IndexOf(',');

                        if(endIndex < 0)
                        {
                            // No next separator; use the remainder as the next value.
                            subValue = value.Trim();
                            value = null!;
                        }
                        else if(endIndex != value!.Length - 1)
                        {
                            // Found a separator before the last char.
                            subValue = value.Substring(0, endIndex).Trim();
                            value = value.Substring(endIndex + 1);
                        }
                        else
                        {
                            // Last char was a separator, which is invalid.
                            break;
                        }

                        if(!ignoreCase)
                        {
                            switch(subValue)
                            {
                                {{switchCases}}
                            }
                        }
                        else
                        {
                            {{ifCases}}
                        }

                        if(!parsed)
                            break;
                    }

                    successful = true;

                    if (!parsed)
                    {
                        successful = false;

                        if (throwOnFailure)
                            throw new {{nameof(ArgumentException)}}($"Could not convert the given value `{originalValue}`.", nameof(value));
                    }

                    return ({{enumSymbol.Name}})localResult;
                }

            """);
        }
    }

    private static string GetClassTemplate(EnumDeclarationSyntax eds, ISymbol enumSymbol, out string className)
    {
        className = $"{enumSymbol.Name}Enhanced";

        return @$"

            #nullable enable

            using {typeof(StringBuilder).Namespace};
            using {typeof(Unsafe).Namespace};
            using {typeof(IEnumerable<>).Namespace};
            using {typeof(ArgumentNullException).Namespace};
            using {typeof(StringComparison).Namespace};
            using {typeof(NumberStyles).Namespace};

            namespace {enumSymbol.ContainingNamespace.ToDisplayString()}
            {{
                /// <summary>
                /// Reflection free extension methods for the <see cref=""{enumSymbol.Name}""/> type.
                /// </summary>
                {AccessibilityToAccessModifier(enumSymbol.DeclaredAccessibility)} static partial class {className}
                {{
                    {{CLASS_BODY}}
                }}
            }}

            #nullable restore
        ";

        static string AccessibilityToAccessModifier(Accessibility accessibility)
        {
            return accessibility switch
            {
                Accessibility.Internal or Accessibility.Private => "internal",
                _ => "public"
            };
        }
    }
}

internal class ServiceNotifications : ISyntaxReceiver
{
    /// <inheritdoc/>
    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is EnumDeclarationSyntax eds)
            DeclaredEnums.Add(eds);
    }

    public List<EnumDeclarationSyntax> DeclaredEnums { get; } = [];
}