using KubernetesTestApp.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace KubernetesTestApp.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : base(dbContextOptions)
    {
    }

    public DbSet<Profile> Profiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Profile>()
            .Property(x => x.Id)
            .IsRequired();

        modelBuilder.Entity<Profile>()
            .Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(35);

        modelBuilder.Entity<Profile>()
            .Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(35);

        modelBuilder.Entity<Profile>()
            .Property(x => x.Profession)
            .IsRequired()
            .HasMaxLength(50);

        base.OnModelCreating(modelBuilder);
    }
}