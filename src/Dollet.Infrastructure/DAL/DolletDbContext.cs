using Dollet.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dollet.Infrastructure.DAL
{
    public class DolletDbContext : DbContext
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<AppData> AppData { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<CurrencyValue> CurrencyValues { get; set; }
        public DbSet<AccountCategory> AccountCategories { get; set; }

        public DolletDbContext(DbContextOptions options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relație one-to-many între Users și Accounts
            modelBuilder.Entity<Account>()
                .HasOne(a => a.User)
                .WithMany(u => u.Accounts) // Colecția Accounts din Users
                .HasForeignKey(a => a.UserId) // Cheia străină
                .OnDelete(DeleteBehavior.Cascade); // Ștergerea utilizatorului șterge

            modelBuilder.Entity<Account>()
                .HasMany(x => x.Expenses)
                .WithOne(x => x.Account)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Account>()
                .HasMany(x => x.Incomes)
                .WithOne(x => x.Account)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Expense>()
                .HasOne(x => x.Account)
                .WithMany(x => x.Expenses)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Income>()
                .HasOne(x => x.Account)
                .WithMany(x => x.Incomes)
                .OnDelete(DeleteBehavior.SetNull);

            // Configurare many-to-many între Account și Category
            modelBuilder.Entity<AccountCategory>()
                .HasKey(ac => new { ac.AccountId, ac.CategoryId }); // Cheie compusă

            modelBuilder.Entity<AccountCategory>()
                .HasOne(ac => ac.Account)
                .WithMany(a => a.AccountCategories) // Navigare din Account
                .HasForeignKey(ac => ac.AccountId) // Cheia străină AccountId
                .OnDelete(DeleteBehavior.Cascade); // Ștergerea unui cont șterge legăturile

            modelBuilder.Entity<AccountCategory>()
                .HasOne(ac => ac.Category)
                .WithMany(c => c.AccountCategories) // Navigare din Category
                .HasForeignKey(ac => ac.CategoryId) // Cheia străină CategoryId
                .OnDelete(DeleteBehavior.Cascade); // Ștergerea unei categorii șterge legăturile

            base.OnModelCreating(modelBuilder);
        }
    }
}
