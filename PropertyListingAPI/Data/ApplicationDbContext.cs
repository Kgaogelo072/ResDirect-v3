using Microsoft.EntityFrameworkCore;
using PropertyListingAPI.Models;

namespace PropertyListingAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyImage> PropertyImages { get; set; }
        public DbSet<ViewingRequest> ViewingRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

            modelBuilder.Entity<ViewingRequest>()
                .HasOne(v => v.Property)
                .WithMany()
                .HasForeignKey(v => v.PropertyId)
                .OnDelete(DeleteBehavior.Restrict); // <-- prevent cascade

            modelBuilder.Entity<ViewingRequest>()
                .HasOne(v => v.Tenant)
                .WithMany()
                .HasForeignKey(v => v.TenantId)
                .OnDelete(DeleteBehavior.Restrict); // <-- prevent cascade

            modelBuilder.Entity<Property>()
                .HasOne(p => p.Agent)
                .WithMany()
                .HasForeignKey(p => p.AgentId)
                .OnDelete(DeleteBehavior.Restrict); // <-- prevent cascade

            // Configure PropertyImage relationship
            modelBuilder.Entity<PropertyImage>()
                .HasOne(pi => pi.Property)
                .WithMany(p => p.Images)
                .HasForeignKey(pi => pi.PropertyId)
                .OnDelete(DeleteBehavior.Cascade); // Delete images when property is deleted

            // Ensure only one primary image per property
            modelBuilder.Entity<PropertyImage>()
                .HasIndex(pi => new { pi.PropertyId, pi.IsPrimary })
                .HasFilter("IsPrimary = 1")
                .IsUnique();
        }
    }
}
