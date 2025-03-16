namespace Innovation.Sample.Data.Anemics
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class AnemicBase
    {
        #region Constructor

        public AnemicBase(Guid id)
        {
            this.Id = id;
        }

        #endregion Constructor

        #region Properties


        // When using sql server with Guid's as primary key
        // Consider using a non-clustered index
        // https://learn.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.sqlserverkeybuilderextensions?view=efcore-8.0
        [Key]
        public Guid Id { get; set; }

        #endregion Properties
    }
}
