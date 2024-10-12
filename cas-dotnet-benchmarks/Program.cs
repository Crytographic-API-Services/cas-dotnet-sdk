using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using cas_dotnet_benchmarks;
using cas_dotnet_benchmarks.PasswordHashers;

var config = ManualConfig.Create(DefaultConfig.Instance)
         .With(ConfigOptions.JoinSummary)
         .With(ConfigOptions.DisableLogFile);

BenchmarkRunner.Run(new[]{
            BenchmarkConverter.TypeToBenchmarks( typeof(PasswordHasherBenchmarks), config),
            BenchmarkConverter.TypeToBenchmarks( typeof(BCryptBenchmark), config)
            });