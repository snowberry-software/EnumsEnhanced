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

                //foreach (var memberSymbol in membersSymbols)
                //    sb.AppendLine($"// {memberSymbol.Name}");

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

        bool isFlags = enumSymbol
            .GetAttributes()
            .Any(a => a.AttributeClass?.ToDisplayString() == "System.FlagsAttribute");

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
            var flagCasesToString = new StringBuilder();
            var switchCasesToString = new StringBuilder();

            const string separator = $", ";

            var groupedByValue = memberSymbols
                .Where(x => x.HasConstantValue)
                .GroupBy(x => x.ConstantValue);

            foreach (var group in groupedByValue)
            {
                object? value = group.Key;
                var memberFirst = group.First();
                var memberLast = isFlags ? group.Last() : group.First();

                // GetNameFast uses FIRST name (matches Enum.GetName behavior)
                string memberRefFirst = $"{enumSymbol.Name}.{memberFirst.Name}";
                switchCases.AppendLine($"case {memberRefFirst}:");
                switchCases.AppendLine($"\treturn nameof({memberRefFirst});");
                switchCases.AppendLine();

                // ToStringFast uses LAST name (matches Enum.ToString behavior)
                string memberRefLast = $"{enumSymbol.Name}.{memberLast.Name}";
                switchCasesToString.AppendLine($"case {memberRefLast}:");
                switchCasesToString.AppendLine($"\treturn nameof({memberRefLast});");
                switchCasesToString.AppendLine();
            }

            var sortedByConstantValueDescGrouped = memberSymbols
                .Where(x => x.HasConstantValue)
                .OrderByDescending(x => x.ConstantValue)
                .GroupBy(x => x.ConstantValue);

            const string c_ToStringFastInternal = "ToStringFastInternal";
            GenerateFlagCases(flagCases, isToString: false, enumSymbol, enumUnderlyingType, sortedByConstantValueDescGrouped, constantValuesChecked, getNameMethodName);
            constantValuesChecked.Clear();
            GenerateFlagCases(flagCasesToString, isToString: true, enumSymbol, enumUnderlyingType, sortedByConstantValueDescGrouped, constantValuesChecked, c_ToStringFastInternal);

            const string c_CheckedMaskName = "checkedMask";
            const string c_CheckedMaskNameCurrent = "checkedMaskCurrent";

            AppendGetNameMethod(methodSb, "public", enumSymbol, enumUnderlyingType, getNameMethodName, switchCases, flagCases, isFlags);
            AppendGetNameMethod(methodSb, "private", enumSymbol, enumUnderlyingType, c_ToStringFastInternal, switchCasesToString, flagCasesToString, isFlags);

            methodSb.AppendLine($$"""

                /// <summary>
                /// Converts the value of this instance to its equivalent string representation.
                /// </summary>
                /// <param name="e">The value of a particular enumerated constant in terms of its underlying type.</param>
                /// <returns>The string representation of the value of this instance.</returns>
                {{methodImplAttributeText}}
                public static string? {{toStringMethodName}}(this {{enumSymbol.Name}} e)
                {
                    return {{c_ToStringFastInternal}}(e, true);
                }
            """);

            static void AppendGetNameMethod(
                StringBuilder sb,
                string accessModifier,
                ISymbol enumSymbol,
                ISymbol enumUnderlyingType,
                string methodName,
                StringBuilder switchCases,
                StringBuilder flagCases,
                bool writeFlags)
            {
                sb.AppendLine($$"""
                    /// <summary>
                    /// Resolves the name of the given enum value.
                    /// </summary>
                    /// <param name="e">The value of a particular enumerated constant in terms of its underlying type.</param>
                    /// <param name="includeFlagNames">Determines whether the value has flags, so it will return `EnumValue, EnumValue2`.</param>
                    /// <returns> A string containing the name of the enumerated constant or <see langword="null"/> if the enum has multiple flags set but <paramref name="includeFlagNames"/> is not enabled.</returns>
                    {{accessModifier}} static string? {{methodName}}(this {{enumSymbol.Name}} e, bool includeFlagNames = false)
                    {
                        switch(e)
                        {
                            {{switchCases}}
                        }
                
                        {{(writeFlags ? "" : $"return (({enumUnderlyingType.Name})e).ToString();")}}

                        // Returning null is the default behavior.
                        if(!includeFlagNames)
                            return null;
                            //throw new Exception("Enum name could not be found!");
                
                        {{(!writeFlags ? "/*" : "")}}

                        var flagBuilder = new StringBuilder();
                        {{flagCases}}
                        if({{c_CheckedMaskNameCurrent}} != default)
                            return (({{enumUnderlyingType.Name}})e).ToString();

                        return flagBuilder.ToString().Trim(new char[] { ',', ' ' });

                        {{(!writeFlags ? "*/" : "")}}
                    }
                    """);
            }

            static void GenerateFlagCases(StringBuilder flagCasesBuilder,
                bool isToString,
                ISymbol enumSymbol,
                ISymbol enumUnderlyingType,
                IEnumerable<IGrouping<object?, IFieldSymbol>> fields,
                List<object> constantValuesChecked,
                string toStringMethodName)
            {
                bool foundZero = false;
                flagCasesBuilder.AppendLine($"{enumUnderlyingType.Name} {c_CheckedMaskNameCurrent} = ({enumUnderlyingType.Name})e;");
                foreach (var group in fields)
                {
                    var member = group.Last();

                    string memberRef = $"{enumSymbol.Name}.{member.Name}";

                    if (member.HasConstantValue)
                    {
                        if (!foundZero && member.ConstantValue.ToString() == "0")
                        {
                            foundZero = true;
                            continue;
                        }

                        if (constantValuesChecked.Contains(member.ConstantValue))
                        {
                            string skipText = $"// Skipping duplicated constant value: {memberRef} -> {member.ConstantValue}";

                            flagCasesBuilder.AppendLine(skipText);
                            flagCasesBuilder.AppendLine();
                            continue;
                        }

                        constantValuesChecked.Add(member.ConstantValue);
                    }

                    flagCasesBuilder.AppendLine(@$"if(({c_CheckedMaskNameCurrent} & {member.ConstantValue}) == {member.ConstantValue}) {{");
                    flagCasesBuilder.AppendLine("\t" + @$"flagBuilder.{nameof(StringBuilder.Insert)}(0, ""{separator}"" + {memberRef}.{toStringMethodName}(false));");
                    flagCasesBuilder.AppendLine("\t" + @$"{c_CheckedMaskNameCurrent} -= {member.ConstantValue}; }}");
                    flagCasesBuilder.AppendLine();
                }
            }
        }

        // ParseFast
        {
            const string parseMethodName = "ParseFast";
            const string tryParseMethodName = "TryParseFast";
            switchCases.Clear();

            var ifCases = new StringBuilder();

            constantValuesChecked.Clear();

            foreach (var member in memberSymbols.OrderBy(x => x.Name.Length).Cast<IFieldSymbol>())
            {
                if (!member.HasConstantValue)
                    continue;

                if (member.ConstantValue == null)
                    continue;

                string memberRef = $"{enumSymbol.Name}.{member.Name}";

                string constantValueString = member.ConstantValue is IConvertible convertible ? convertible.ToString(CultureInfo.InvariantCulture) : member.ConstantValue.ToString();

                switchCases.AppendLine($"case nameof({memberRef}):");
                switchCases.AppendLine($"\tparsed = true;");
                switchCases.AppendLine($"\tlocalResult |= {constantValueString};");
                switchCases.AppendLine($"\tbreak;");
                switchCases.AppendLine();

                ifCases.AppendLine($"if(subValue.Equals(nameof({memberRef}), {nameof(StringComparison)}.{nameof(StringComparison.OrdinalIgnoreCase)})) {{");
                ifCases.AppendLine($"\tparsed = true;");
                ifCases.AppendLine($"\tlocalResult |= {constantValueString}; }}");
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

                    if (string.{{nameof(string.IsNullOrWhiteSpace)}}(value))
                    {
                        if (throwOnFailure)
                            throw new {{nameof(ArgumentException)}}("Value can't be null or whitespace!", nameof(value));
                        
                        return default;
                    }

                    {{enumUnderlyingType.Name}} localResult = 0;
                    bool parsed = false;
                    string subValue;
                    string originalValue = value;
                    char firstChar = value[0];

                    if (char.{{nameof(char.IsWhiteSpace)}}(firstChar))
                        firstChar = value.TrimStart()[0];

                    if (char.{{nameof(char.IsDigit)}}(firstChar) || firstChar == '-' || firstChar == '+')
                    {
                        if({{enumUnderlyingType.Name}}.TryParse(value, NumberStyles.AllowLeadingSign | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, null, out var valueNumber))
                        {
                            parsed = true;
                            localResult = valueNumber;
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