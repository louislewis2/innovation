namespace Innovation.Sample.Data.Anemics.Customers
{
    using Innovation.Sample.Api.Customers.ViewModels;

    public static class CustomerAnemicExtensions
    {
        public static CustomerLite ToCustomerLite(this CustomerAnemic customerAnemic)
        {
            if (customerAnemic == null)
            {
                return null;
            }

            return new CustomerLite(
                id: customerAnemic.Id,
                fullName: customerAnemic.GetFullName());
        }

        public static CustomerDetail ToCustomerDetail(this CustomerAnemic customerAnemic)
        {
            if (customerAnemic == null)
            {
                return null;
            }

            return new CustomerDetail(
                id: customerAnemic.Id,
                firstName: customerAnemic.FirstName,
                lastName: customerAnemic.LastName,
                phoneNumber: customerAnemic.PhoneNumber,
                email: customerAnemic.Email);
        }

        public static CustomerLite[] ToCustomerLite(this CustomerAnemic[] customerAnemics)
        {
            if (customerAnemics == null || customerAnemics.Length < 1)
            {
                return null;
            }

            var itemsList = new CustomerLite[customerAnemics.Length];

            for(var i = 0; i <  customerAnemics.Length; i++)
            {
                itemsList[i] = customerAnemics[i].ToCustomerLite();
            }

            return itemsList;
        }

        public static CustomerDetail[] ToCustomerDetail(this CustomerAnemic[] customerAnemics)
        {
            if (customerAnemics == null || customerAnemics.Length < 1)
            {
                return null;
            }

            var itemsList = new CustomerDetail[customerAnemics.Length];

            for (var i = 0; i < customerAnemics.Length; i++)
            {
                itemsList[i] = customerAnemics[i].ToCustomerDetail();
            }

            return itemsList;
        }
    }
}
