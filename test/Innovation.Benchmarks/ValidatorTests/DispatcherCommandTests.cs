namespace Innovation.Benchmarks.ValidatorTests
{
    using System.Threading.Tasks;

    using Innovation.Api.Commanding;
    using BenchmarkDotNet.Attributes;
    using Innovation.Api.Dispatching;

    using Innovation.ApiSample;

    /// <summary>
    /// A benchmark class to test the performance of the Dispatcher with a specific command object (BlankCommand).
    /// </summary>
    [MemoryDiagnoser]
    public class DispatcherCommandTests : DependencyBuilderBase
    {
        #region Fields

        private IDispatcher dispatcher;
        private static BlankCommand blankCommand = new BlankCommand();

        #endregion Fields

        #region Methods

        [GlobalSetup]
        public void GlobalSetup()
        {
            this.dispatcher = this.GetRequiredService<IDispatcher>();
        }

        [Benchmark]
        public async ValueTask<ICommandResult> DispatchBlankCommand()
        {
            return await dispatcher.Command(command: blankCommand);
        }

        #endregion Methods
    }
}
