﻿namespace Innovation.Sample.Data.Contexts
{
    using Microsoft.EntityFrameworkCore;

    using Innovation.Sample.Data.Anemics;

    public class ExampleContext : DbContext
    {
        #region Properties

        public DbSet<AnemicCustomer> Customers { get; set; }
        public DbSet<AnemicLogEntry> LogEntries { get; set; }

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
#if (NETSTANDARD1_6 || NETFULL)
            optionsBuilder.UseInMemoryDatabase();
#else
            optionsBuilder.UseInMemoryDatabase("Innovation_Sample_Database");
#endif
        }

        #endregion Overrides
    }
}
