namespace Innovation.ApiSample
{
    using Innovation.Api.Commanding;

    public class BlankCommand : ICommand
    {
        public string EventName => nameof(BlankCommand);
    }
}
