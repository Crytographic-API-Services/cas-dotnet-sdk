using BenchmarkDotNet.Running;
using cas_dotnet_benchmarks;
using cas_dotnet_benchmarks.PasswordHashers;

BenchmarkRunner.Run(new[]{
            BenchmarkConverter.TypeToBenchmarks( typeof(PasswordHasherBenchmarks)),
            BenchmarkConverter.TypeToBenchmarks( typeof(BCryptBenchmark))
            });