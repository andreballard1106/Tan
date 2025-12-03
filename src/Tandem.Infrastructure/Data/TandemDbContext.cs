using Microsoft.EntityFrameworkCore;
using Tandem.Domain.Entities;

namespace Tandem.Infrastructure.Data;

public class TandemDbContext : DbContext
{
    public TandemDbContext(DbContextOptions<TandemDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.MiddleName).HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(20);
            entity.Property(e => e.EmailAddress).IsRequired().HasMaxLength(255);
            entity.HasIndex(e => e.EmailAddress).IsUnique();
        });
    }
}

