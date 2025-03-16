namespace Innovation.Benchmarks.ValidatorTests
{
    using Innovation.Api.Dispatching;
    using BenchmarkDotNet.Attributes;

    [MemoryDiagnoser]
    public class ValueStopwatchTests : DependencyBuilderBase
    {
        #region Methods

        [Benchmark]
        public long StopWatchUsage()
        {
            var stopWatch = ValueStopwatch.StartNew();

            return (long)stopWatch.GetElapsedTime().TotalMilliseconds;

        }

        #endregion Methods
    }
}
