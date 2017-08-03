namespace Innovation.Sample.Data.Handlers.Customers.Reactors
{
    using System;
    using System.Threading.Tasks;

    using Innovation.Api.Reactions;
    using Innovation.Api.Commanding;
    using Innovation.Sample.Api.Customers.Commands;

    public class CreateCustomerResultReactor : ICommandResultReactor<CreateCustomer>
    {
        public async Task React(ICommandResult commandResult, CreateCustomer command)
        {
            await Task.Run(() =>
            {
                Console.WriteLine($"Create Customer Has Just Fired: { commandResult.Success }");
            });
        }
    }
}
