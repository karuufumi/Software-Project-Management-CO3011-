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

        // DbSets
        public DbSet<BookModel> Books { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<FacultyMember> FacultyMembers { get; set; }
        public DbSet<Librarian> Librarians { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<BorrowRecord> BorrowRecords { get; set; }
        public DbSet<BookQueue> BookQueues { get; set; }
        public DbSet<Membership> Memberships { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure UserModel inheritance (TPH - Table Per Hierarchy)
            modelBuilder.Entity<UserModel>()
                .HasDiscriminator<string>("UserType")
                .HasValue<Student>("Student")
                .HasValue<FacultyMember>("Faculty")
                .HasValue<Librarian>("Librarian")
                .HasValue<Admin>("Admin");

            // Configure BorrowRecord relationships
            modelBuilder.Entity<BorrowRecord>()
                .HasOne(br => br.User)
                .WithMany(u => u.BorrowedBooks)
                .HasForeignKey(br => br.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BorrowRecord>()
                .HasOne(br => br.Book)
                .WithMany(b => b.BorrowRecords)
                .HasForeignKey(br => br.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure BookQueue relationships
            modelBuilder.Entity<BookQueue>()
                .HasOne(q => q.User)
                .WithOne()
                .HasForeignKey<BookQueue>(q => q.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BookQueue>()
                .HasMany(q => q.QueuedBooks)
                .WithMany()
                .UsingEntity(j => j.ToTable("QueuedBookItems"));

            // Configure Membership relationships
            modelBuilder.Entity<Membership>()
                .HasOne(m => m.User)
                .WithOne()
                .HasForeignKey<Membership>(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure indexes for performance
            modelBuilder.Entity<BookModel>()
                .HasIndex(b => b.ISBN)
                .IsUnique();

            modelBuilder.Entity<UserModel>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<UserModel>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<BorrowRecord>()
                .HasIndex(br => new { br.UserId, br.BookId, br.IsReturned });

            modelBuilder.Entity<Membership>()
                .HasIndex(m => m.UserId)
                .IsUnique();
        }
    }
}