using Microsoft.EntityFrameworkCore;
using OrbitPass.Core.Entities;

namespace OrbitPass.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // DbSets will be added here as we create entities
    public DbSet<User> Users { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<OrbitCoinTransaction> OrbitCoinTransactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Entity configurations will be added here
        //User -> Events (One-to-Many)
        // modelBuilder.Entity<Event>()
        //     .HasOne<User>()
        //     .WithMany()
        //     .HasForeignKey(e => e.OrganizerId)
        //     .OnDelete(DeleteBehavior.Restrict);

        // //User -> Order (One-to-Many)
        // modelBuilder.Entity<Order>()
        //     .HasOne<User>()
        //     .WithMany()
        //     .HasForeignKey(o => o.UserId)
        //     .OnDelete(DeleteBehavior.Cascade);

        // //Events -> Orders
        // modelBuilder.Entity<Order>()
        //     .HasOne<Event>()
        //     .WithMany()
        //     .HasForeignKey(o => o.EventId)
        //     .OnDelete(DeleteBehavior.Cascade);

        // //Order -> Tickets
        // modelBuilder.Entity<Ticket>()
        //     .HasOne<Order>()
        //     .WithMany()
        //     .HasForeignKey(t => t.OrderId)
        //     .OnDelete(DeleteBehavior.Cascade);

        // //User -> Transactions
        // modelBuilder.Entity<OrbitCoinTransaction>()
        //     .HasOne<User>()
        //     .WithMany()
        //     .HasForeignKey(t => t.UserId);
    }
}