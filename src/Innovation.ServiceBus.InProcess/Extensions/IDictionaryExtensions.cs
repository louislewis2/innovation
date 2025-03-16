namespace System.Collections.Generic
{
    using System.ComponentModel.DataAnnotations;

    public static class IDictionaryExtensions
    {
        public static ValidationResult[] ConvertToValidationResult(this IDictionary<string, string[]> errorResults)
        {
            if (errorResults == null)
            {
                return Array.Empty<ValidationResult>();
            }

            var validationResults = new List<ValidationResult>();

            foreach (var errorResult in errorResults)
            {
                foreach(var errorEntry in errorResult.Value)
                {
                    validationResults.Add(new ValidationResult(errorMessage: errorEntry, memberNames: [errorResult.Key]));
                }
            }

            return validationResults.ToArray();
        }
    }
}
