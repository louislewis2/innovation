namespace Innovation.ApiSample.Validation
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Innovation.Api.Validation;

    public class SampleValidationResult : IValidationResult
    {
        #region Constructor

        public SampleValidationResult()
        {
            this.Success = true;
        }

        #endregion Constructor

        #region Properties

        public bool Success { get; private set; }
        public List<ValidationResult> Errors { get; private set; }

        #endregion Properties

        #region Methods

        public void Add(ValidationResult validationResult)
        {
            if(this.Errors == null)
            {
                this.Success = false;
                this.Errors = new List<ValidationResult>();
            }

            this.Errors.Add(item: validationResult);
        }

        #endregion Methods
    }
}
