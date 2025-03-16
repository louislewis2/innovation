namespace Innovation.Sample.Data.Contexts
{
    using Microsoft.EntityFrameworkCore;

    using Innovation.Sample.Data.Anemics.Logging;
    using Innovation.Sample.Data.Anemics.Customers;

    public class PrimaryContext : AuditDbContextBase<PrimaryContext>
    {
        #region Constructor

        public PrimaryContext(DbContextOptions<PrimaryContext> options) : base(options: options)
        {
        }

        #endregion Constructor

        #region Properties

        public DbSet<CustomerAnemic> Customers { get; set; }
        public DbSet<LogEntryAnemic> LogEntries { get; set; }

        #endregion Properties

        #region Overrides

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerAnemic>()
                .HasIndex(b => b.UserName)
                .IsUnique();
        }

        #endregion Overrides
    }
}
