namespace Innovation.Sample.Data.Contexts
{
    using Microsoft.EntityFrameworkCore;

    using Innovation.Sample.Data.Anemics.Auditing;

    public class AuditDbContextBase<T> : DbContext where T : DbContext
    {
        #region Constructor

        public AuditDbContextBase(DbContextOptions<T> options) : base(options)
        {
        }

        #endregion Constructor

        #region Properties

        public DbSet<CommandAuditEntryAnemic> CommandAuditEntries { get; set; }
        public DbSet<QueryAuditEntryAnemic> QueryAuditEntries { get; set; }

        #endregion Properties
    }
}
