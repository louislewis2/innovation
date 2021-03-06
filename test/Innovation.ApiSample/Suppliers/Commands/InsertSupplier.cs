﻿namespace Innovation.ApiSample.Suppliers.Commands
{
    using Innovation.Api.Commanding;

    using Criteria;

    public class InsertSupplier : ICommand
    {
        #region Constructor

        public InsertSupplier(SupplierCriteria supplierCriteria)
        {
            this.Criteria = supplierCriteria;
        }

        #endregion Constructor

        #region Properties

        public SupplierCriteria Criteria { get; }

        #endregion Properties

        #region ICommand

        public string EventName => "Insert Supplier";

        #endregion ICommand
    }
}
