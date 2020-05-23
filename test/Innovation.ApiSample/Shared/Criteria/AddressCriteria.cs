namespace Innovation.ApiSample.Shared.Criteria
{
    public class AddressCriteria
    {
        #region Constructor

        public AddressCriteria(string line1, string code)
        {
            this.Line1 = line1;
            this.Code = code;
        }

        #endregion Constructor

        #region Properties

        public string Line1 { get; }
        public string Code { get; }

        #endregion Properties
    }
}
