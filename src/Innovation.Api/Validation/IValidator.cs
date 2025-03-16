namespace Innovation.Api.Validation
{
    using System.Threading.Tasks;
    using System.Diagnostics.CodeAnalysis;

    using Innovation.Api.Commanding;

    public interface IValidator<in TCommand> where TCommand : ICommand
    {
        Task<IValidationResult> Validate([DisallowNull] TCommand command);
    }
}
