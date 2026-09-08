using CharmCityBank.Api.Domain.Accounts;
using CharmCityBank.Api.Domain.Audits;
using CharmCityBank.Api.Domain.Customers;
using CharmCityBank.Api.Domain.Transactions;
using CharmCityBank.Api.Domain.Transfers;
using CharmCityBank.Api.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CharmCityBank.Api.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<BankAccount> BankAccounts { get; set; }
    public DbSet<BankTransaction> BankTransactions { get; set; }
    public DbSet<Transfer> Transfers { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasSequence<long>("CustomerNumberSequence")
            .StartsAt(1)
            .IncrementsBy(1);

        // Customer
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(c => c.CustomerNumber)
                .HasDefaultValueSql("NEXT VALUE FOR CustomerNumberSequence");

            entity.HasIndex(c => c.CustomerNumber).IsUnique();

            entity.Property(c => c.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(c => c.LastName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(c => c.IsDisabled)
                .IsRequired()
                .HasDefaultValue(false);

            entity.HasIndex(c => new { c.FirstName, c.LastName });

            entity.HasOne<ApplicationUser>()
                .WithOne()
                .HasForeignKey<Customer>(c => c.IdentityUserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(c => c.IdentityUserId)
                .IsRequired()
                .HasMaxLength(450);

            entity.HasIndex(c => c.IdentityUserId)
                .IsUnique();
        });

        // Address
        modelBuilder.Entity<Address>(entity =>
        {
            entity.Property(a => a.StreetAddress)
                .IsRequired()
                .HasMaxLength(500);

            entity.HasIndex(a => a.StreetAddress);

            entity.Property(a => a.City)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(a => a.State)
                .IsRequired()
                .HasMaxLength(2);

            entity.Property(a => a.ZipCode)
                .IsRequired()
                .HasMaxLength(15);

            entity.HasIndex(a => a.ZipCode);

            entity.HasOne(a => a.Customer)
                .WithMany(c => c.Addresses)
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Bank Account
        modelBuilder.Entity<BankAccount>(entity =>
        {
            entity.Property(b => b.AccountNumber)
                .IsRequired()
                .HasMaxLength(10);
            entity.HasIndex(b => b.AccountNumber)
                .IsUnique();
            entity.HasOne(b => b.Customer)
                .WithMany(c => c.BankAccounts)
                .HasForeignKey(b => b.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(b => b.AccountType)
                .IsRequired();

            entity.Property(b => b.Balance)
                .HasPrecision(18, 2);

            entity.Property(b => b.Status)
                .IsRequired();

            entity.HasIndex(b => b.CreatedAtUtc);
            entity.HasIndex(b => b.UpdatedAtUtc);
            entity.HasIndex(b => b.Status);
        });

        // Bank Transaction
        modelBuilder.Entity<BankTransaction>(entity =>
        {
            entity.HasOne(t => t.Account)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(t => t.Transfer)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.TransferId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(t => t.TransactionType)
                .IsRequired();

            entity.Property(t => t.Amount)
                .HasPrecision(18, 2);

            entity.Property(t => t.BalanceBefore)
                .HasPrecision(18, 2);

            entity.Property(t => t.BalanceAfter)
                .HasPrecision(18, 2);

            entity.Property(t => t.Description)
                .HasMaxLength(200);

            entity.HasIndex(t => t.CreatedAtUtc);
        });

        // Transfer
        modelBuilder.Entity<Transfer>(entity =>
        {
            entity.HasOne(t => t.FromAccount)
                .WithMany(b => b.OutgoingTransfers)
                .HasForeignKey(t => t.FromAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(t => t.ToAccount)
                .WithMany(b => b.IncomingTransfers)
                .HasForeignKey(t => t.ToAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(t => t.Amount)
                .HasPrecision(18, 2);

            entity.Property(t => t.Status)
                .IsRequired();

            entity.Property(t => t.Memo)
                .HasMaxLength(200);

            entity.HasIndex(t => t.CreatedAtUtc);
            entity.HasIndex(t => t.CompletedAtUtc);
        });

        // Audit  Log
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(a => a.IdentityUserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(a => a.ActionType)
                .IsRequired();

            entity.Property(a => a.EntityType)
                .IsRequired();

            entity.Property(a => a.EntityId)
                .HasMaxLength(50);

            entity.HasIndex(a => a.EntityId);

            entity.HasIndex(a => a.CreatedAtUtc);
        });
    }
}