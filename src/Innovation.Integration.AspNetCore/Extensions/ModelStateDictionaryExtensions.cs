namespace Microsoft.AspNetCore.Mvc.ModelBinding
{
    using Innovation.Api.CommandHelpers;

    public static class ModelStateDictionaryExtensions
    {
        public static CommandResult Handle(this ModelStateDictionary modelState)
        {
            var commandResult = new CommandResult();

            if (modelState == null)
            {
                commandResult.Fail(memberName: "ModelState", errorMessage: "Cannot be null");

                return commandResult;
            }

            if (modelState.IsValid)
            {
                commandResult.Fail(memberName: "ModelState", errorMessage: "No model state errors found");

                return commandResult;
            }

            foreach (var propertyErrorPair in modelState)
            {
                foreach (var error in propertyErrorPair.Value.Errors)
                {
                    commandResult.Fail(memberName: propertyErrorPair.Key, errorMessage: error.ErrorMessage);
                }
            }

            return commandResult;
        }
    }
}
