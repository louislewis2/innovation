namespace Innovation.Sample.Data.Handlers.Customers.Reactors
{
    using System;
    using System.Threading.Tasks;

    using Innovation.Api.Reactions;
    using Innovation.Sample.Api.Customers.Commands;

    public class CreateCustomerReactor : ICommandReactor<CreateCustomer>
    {
        public async Task React(CreateCustomer command)
        {
            await Task.Run(() =>
            {
                Console.WriteLine($"Create Customer Event About To Fire: { command.FirstName }");
            });
        }
    }
}
