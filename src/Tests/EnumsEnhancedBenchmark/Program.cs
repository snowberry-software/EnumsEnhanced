using BenchmarkDotNet.Running;

BenchmarkRunner.Run(typeof(Program).Assembly);
//BenchmarkRunner.Run<HasFlagBenchmark>();
//BenchmarkRunner.Run<GetNamesBenchmark>();
//BenchmarkRunner.Run<GetNameBenchmark>();
//BenchmarkRunner.Run<GetValuesBenchmark>();
//BenchmarkRunner.Run<IsDefinedBenchmark>();
//BenchmarkRunner.Run<IsDefinedValueBenchmark>();
//BenchmarkRunner.Run<ParseNameBenchmark>();
//BenchmarkRunner.Run<ParseValueBenchmark>();