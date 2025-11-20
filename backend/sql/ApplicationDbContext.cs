using backend.models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<LibrarianModel> Librarians { get; set; }
        public DbSet<Membership> Memberships { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // USER BASE ENTITY
            modelBuilder.Entity<UserModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            });

            // INHERITANCE
            modelBuilder.Entity<Student>()
                .HasBaseType<UserModel>();

            modelBuilder.Entity<LibrarianModel>()
                .HasBaseType<UserModel>();
            
            modelBuilder.Entity<AdminModel>()
                .HasBaseType<UserModel>();

            // MEMBERSHIP
            modelBuilder.Entity<Membership>(entity =>
            {
                entity.HasKey(e => e.MembershipId);

                // Membership belongs to ONE User
                entity.HasOne(m => m.User)
                      .WithMany()             // If User has many memberships, use .WithMany(u => u.Memberships)
                      .HasForeignKey(m => m.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
