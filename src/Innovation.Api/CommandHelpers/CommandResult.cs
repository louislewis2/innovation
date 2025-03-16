namespace Innovation.Api.CommandHelpers
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.ComponentModel.DataAnnotations;

    using Commanding;

    public class CommandResult : ICommandResult
    {
        #region Fields

        private bool success;
        private IDictionary<string, List<string>> errors;

        #endregion Fields

        #region Constructor

        public CommandResult(bool success = true, string recordId = "", IDictionary<string, string[]> errors = null)
        {
            this.success = errors != null ? false : success;
            this.RecordId = recordId;

            if (errors != null)
            {
                this.Fail(errors);
            }
        }

        public CommandResult([DisallowNull] IDictionary<string, string[]> errors)
        {
            if (errors != null)
            {
                this.InitializeErrors();
                this.AddErrorPairsToDictionary(errorDictionary: errors);
            }
        }

        public CommandResult([DisallowNull] string memberName, [DisallowNull] string errorMessage)
        {
            if (errors != null)
            {
                this.InitializeErrors();
                this.Fail(memberName: memberName, errorMessage: errorMessage);
            }
        }

        public CommandResult([DisallowNull] string errorMessage)
        {
            if (errors != null)
            {
                this.InitializeErrors();
                this.Fail(errorMessage: errorMessage);
            }
        }

        public CommandResult([DisallowNull] Exception ex)
        {
            if (errors != null)
            {
                this.InitializeErrors();
                this.Fail(ex: ex);
            }
        }

        #endregion Constructor

        #region Properties

        public bool Success => this.success;
        public IDictionary<string, string[]> Errors
        {
            get
            {
                if (this.errors == null)
                {
                    return null;
                }

                var errorDictionary = new Dictionary<string, string[]>();
                foreach (var error in this.errors)
                {
                    errorDictionary.Add(key: error.Key, value: error.Value.ToArray());
                }

                return errorDictionary;
            }
        }

        public string RecordId { get; internal set; }

        #endregion Properties

        #region Methods

        // TODO: Properly document this method, that it access the errors by index
        // if an invalid element is specificed, it will return null
        public (string Name, string[] Reasons) this[int errorKey]
        {
            get => this.GetErrorAt(errorKey);
        }

        public string this[int errorKey, int errorReasonKey]
        {
            get
            {
                var errorEntry = this.GetErrorAt(errorKey);

                if (errorEntry == default || errorEntry.Reasons?.Length == 0)
                {
                    return null;
                }

                return errorEntry.Reasons[errorReasonKey];
            }
        }

        public void Fail([DisallowNull] IDictionary<string, string[]> errors)
        {
            this.InitializeErrors();
            this.AddErrorPairsToDictionary(errors);
        }

        public void Fail([DisallowNull] IDictionary<string, List<string>> errors)
        {
            this.InitializeErrors();

            foreach (var errorEntry in errors)
            {
                foreach (var errorMessage in errorEntry.Value)
                {
                    this.Fail(memberName: string.IsNullOrWhiteSpace(value: errorEntry.Key) ? "MemberName Not Provided" : errorEntry.Key, errorMessage: errorMessage);
                }
            }
        }

        public void Fail([DisallowNull] string errorMessage)
        {
            this.InitializeErrors();
            this.Fail(memberName: "Member Not Provided", errorMessage: errorMessage);
        }

        public void Fail([DisallowNull] Exception ex)
        {
            this.InitializeErrors();
            this.Fail(memberName: "Error", errorMessage: ex.GetInnerMostMessage());
        }

        public void Fail([DisallowNull] string memberName, [DisallowNull] string errorMessage)
        {
            this.InitializeErrors();

            if (this.errors.ContainsKey(key: memberName))
            {
                if (!this.errors[memberName].Exists(errorEntryMatch => errorEntryMatch == errorMessage))
                {
                    this.errors[memberName].Add(errorMessage);
                }
            }
            else
            {
                this.errors.Add(key: memberName, value: new List<string> { errorMessage });
            }
        }

        public void Fail([DisallowNull] IEnumerable<ValidationResult> validationResults)
        {
            this.InitializeErrors();

            foreach (var validationResult in validationResults)
            {
                foreach (var memberName in validationResult.MemberNames)
                {
                    this.Fail(memberName: string.IsNullOrWhiteSpace(value: memberName) ? "MemberName Not Provided" : memberName, errorMessage: validationResult.ErrorMessage);
                }
            }
        }

        public void SetRecord([DisallowNull] string recordId)
        {
            this.RecordId = recordId;
        }

        public void SetRecord([DisallowNull] Guid recordId)
        {
            this.RecordId = recordId.ToString();
        }

        public void SetRecord(int recordId)
        {
            this.RecordId = recordId.ToString();
        }

        public void SetRecord(long recordId)
        {
            this.RecordId = recordId.ToString();
        }

        #endregion Methods

        #region Private Methods

        private (string Name, string[] Reasons) GetErrorAt(int index)
        {
            var keyValuePair = this.errors.ElementAtOrDefault(index);

            if (string.IsNullOrEmpty(value: keyValuePair.Key) && keyValuePair.Value == null)
            {
                return default;
            }

            return (keyValuePair.Key, keyValuePair.Value.ToArray());
        }

        private void InitializeErrors()
        {
            if (this.errors == null)
            {
                this.errors = new Dictionary<string, List<string>>();
            }

            this.success = false;
        }

        private void AddErrorPairsToDictionary(IDictionary<string, string[]> errorDictionary)
        {
            if (errorDictionary == null)
            {
                return;
            }

            this.InitializeErrors();

            foreach (var errorEntry in errorDictionary)
            {
                foreach (var errorDetail in errorEntry.Value)
                {
                    this.Fail(memberName: errorEntry.Key, errorMessage: errorDetail);
                }
            }
        }

        #endregion Private Methods

        #region Overrides

        public override string ToString()
        {
            if (this.Success)
            {
                return Success.ToString();
            }

            return this.Errors == null ? "" : $"{this.Success} Errors: {string.Join(",", this.Errors)}";
        }

        #endregion Overrides
    }
}