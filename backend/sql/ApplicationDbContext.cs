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

        // --- USER & ROLES ---
        public DbSet<UserModel> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<LibrarianModel> Librarians { get; set; }
        public DbSet<AdminModel> Admins { get; set; } // Make sure you have this model class
        //public DbSet<Membership> Memberships { get; set; }

        public DbSet<Membership> Memberships { get; set; }

        
        public DbSet<BookQueue> BookQueues { get; set; }
        // --- LIBRARY SYSTEM (NEW) ---
        public DbSet<BookModel> Books { get; set; }

        //public DbSet<BorrowRequest> BorrowRequests { get; set; } // The Queue
        //        public DbSet<BorrowRecord> BorrowRecords { get; set; }   // The History/Active Loans

        // (This looked like a typo in your snippet, generic object? 
        //  I commented it out unless you have a specific model for it)
        // public object MembershipRequests { get; internal set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ==============================
            // 1. USER HIERARCHY & CONFIG
            // ==============================
            modelBuilder.Entity<UserModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            });

            // TPH (Table Per Hierarchy) Inheritance
            modelBuilder.Entity<Student>().HasBaseType<UserModel>();
            modelBuilder.Entity<LibrarianModel>().HasBaseType<UserModel>();
            modelBuilder.Entity<AdminModel>().HasBaseType<UserModel>();
            modelBuilder.Entity<FacultyMember>().HasBaseType<UserModel>();

            modelBuilder.Entity<Membership>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.Level)
                      .HasConversion<string>();
            });


            // Membership Config
            
            // ==============================
            // 2. LIBRARY SYSTEM CONFIG
            // ==============================

            // --- A. BOOK MODEL ---
            modelBuilder.Entity<BookModel>(entity =>
            {
                entity.HasKey(e => e.BookId);
                entity.Property(e => e.Title).IsRequired();
                
                // Store the Enum as a String (e.g., "Available", "Lost") 
                // instead of an Integer (0, 1) for easier DB debugging.
                entity.Property(e => e.Status)
                      .HasConversion<string>();
            });

            modelBuilder.Entity<BookQueue>(entity =>
            {
                entity.HasKey(e => e.QueueId);

                // One-to-One relationship between UserModel and BookQueue
                entity.HasOne<UserModel>()
                      .WithOne()
                      .HasForeignKey<BookQueue>(bq => bq.QueueId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Membership>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.Level)
                      .HasConversion<string>();
            });
        }
    }
}