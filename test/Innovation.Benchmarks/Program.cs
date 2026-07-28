namespace Innovation.Benchmarks
{
    using System.Threading.Tasks;

    using BenchmarkDotNet.Configs;
    using BenchmarkDotNet.Reports;
    using BenchmarkDotNet.Running;

    using Innovation.Benchmarks.ValidatorTests;

    public class Program
    {
        public static async Task Main(string[] args)
        {
#if DEBUG

            var dispatcherCommandTests = new DispatcherCommandTests();
            dispatcherCommandTests.GlobalSetup();
            await dispatcherCommandTests.Command();

            //var dataAnnotationsValidatorTests = new DataAnnotationsValidatorTests();
            //dataAnnotationsValidatorTests.GlobalSetup();
            //dataAnnotationsValidatorTests.TestNew();

            var summary = BenchmarkRunner.Run<DataAnnotationsValidatorTests>(config:
                DefaultConfig.Instance
                .WithOptions(ConfigOptions.DisableOptimizationsValidator));
#else
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, DefaultConfig.Instance
            .WithOptions(ConfigOptions.DisableOptimizationsValidator)
            .WithSummaryStyle(summaryStyle: SummaryStyle.Default.WithRatioStyle(ratioStyle: BenchmarkDotNet.Columns.RatioStyle.Trend)));

#endif
        }
    }
}
