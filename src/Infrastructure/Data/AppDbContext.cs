using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Authentication> Authentications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.UserName).IsRequired();
                entity.Property(u => u.Email).IsRequired();
            });

            modelBuilder.Entity<Authentication>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.HashedPassword).IsRequired();
                entity.Property(a => a.IsEmailConfirmed).IsRequired();
                entity.Property(a => a.IsActivated).IsRequired();
                entity.HasOne(a => a.User)
                      .WithOne(u => u.Authentication)
                      .HasForeignKey<Authentication>(a => a.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}