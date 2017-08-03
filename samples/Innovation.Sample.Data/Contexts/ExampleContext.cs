namespace Innovation.Sample.Data.Contexts
{
    using Microsoft.EntityFrameworkCore;

    using Innovation.Sample.Data.Anemics;

    public class ExampleContext : DbContext
    {
        #region Properties

        public DbSet<AnemicCustomer> Customers { get; set; }

        #endregion Properties

        #region Overrides

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnemicCustomer>()
                .HasIndex(b => b.UserName)
                .IsUnique();

            modelBuilder.Entity<AnemicCustomer>()
                .HasKey(x => x.Id);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase();
        }

        #endregion Overrides
    }
}
