namespace Innovation.Sample.Data.Anemics
{
    using System;

    public class AnemicCustomer
    {
        #region Properties

        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        #endregion Properties
    }
}
