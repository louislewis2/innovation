namespace Innovation.Benchmarks.ValidatorTests
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;

    using MiniValidation;
    using Innovation.Api.Commanding;
    using Innovation.Api.Dispatching;
    using BenchmarkDotNet.Attributes;


    using Innovation.ApiSample;

    [MemoryDiagnoser]
    public class DispatcherCommandTests : DependencyBuilderBase
    {
        #region Fields

        private IDispatcher dispatcher;
        private IServiceScopeFactory serviceScopeFactory;
        private IServiceScope serviceScope;
        private IServiceProvider serviceProvider;
        private static BlankCommand blankCommand = new BlankCommand();

        #endregion Fields

        #region Methods

        [GlobalSetup]
        public void GlobalSetup()
        {
            this.dispatcher = this.GetRequiredService<IDispatcher>();
            this.serviceScopeFactory = this.GetRequiredService<IServiceScopeFactory>();
            this.serviceProvider = this.GetRequiredService<IServiceProvider>();
            this.serviceScope = this.serviceScopeFactory.CreateScope();
            MiniValidator.TryValidate(blankCommand, this.serviceProvider, out var tt);
        }

        [Benchmark]
        public async ValueTask<ICommandResult> Command()
        {
            return await dispatcher.Command(command: blankCommand);
        }

        #endregion Methods
    }
}
