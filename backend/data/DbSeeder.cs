using backend.models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public static class DbSeeder
    {
        public static async Task SeedDatabase(ApplicationDbContext context)
        {
            // Check if data already exists
            if (await context.Books.AnyAsync())
            {
                Console.WriteLine("✅ Database already seeded!");
                return;
            }

            Console.WriteLine("📚 Seeding books...");

            // Seed Books
            var books = new List<BookModel>
            {
                new BookModel
                {
                    Title = "Harry Potter and the Sorcerer's Stone",
                    Author = "J.K. Rowling",
                    ISBN = "978-0439708180",
                    Genre = "Fantasy",
                    TotalCopies = 5,
                    AvailableCopies = 5,
                    Description = "A young wizard begins his magical journey at Hogwarts."
                },
                new BookModel
                {
                    Title = "1984",
                    Author = "George Orwell",
                    ISBN = "978-0451524935",
                    Genre = "Dystopian",
                    TotalCopies = 3,
                    AvailableCopies = 3,
                    Description = "A dystopian social science fiction novel."
                },
                new BookModel
                {
                    Title = "To Kill a Mockingbird",
                    Author = "Harper Lee",
                    ISBN = "978-0061120084",
                    Genre = "Fiction",
                    TotalCopies = 4,
                    AvailableCopies = 4,
                    Description = "A story of racial injustice and childhood innocence."
                },
                new BookModel
                {
                    Title = "The Great Gatsby",
                    Author = "F. Scott Fitzgerald",
                    ISBN = "978-0743273565",
                    Genre = "Fiction",
                    TotalCopies = 3,
                    AvailableCopies = 3,
                    Description = "A tale of the American Dream in the Jazz Age."
                },
                new BookModel
                {
                    Title = "Pride and Prejudice",
                    Author = "Jane Austen",
                    ISBN = "978-0141439518",
                    Genre = "Romance",
                    TotalCopies = 4,
                    AvailableCopies = 4,
                    Description = "A romantic novel of manners."
                },
                new BookModel
                {
                    Title = "The Hobbit",
                    Author = "J.R.R. Tolkien",
                    ISBN = "978-0547928227",
                    Genre = "Fantasy",
                    TotalCopies = 5,
                    AvailableCopies = 5,
                    Description = "A fantasy adventure in Middle-earth."
                },
                new BookModel
                {
                    Title = "The Catcher in the Rye",
                    Author = "J.D. Salinger",
                    ISBN = "978-0316769174",
                    Genre = "Fiction",
                    TotalCopies = 3,
                    AvailableCopies = 3,
                    Description = "A story of teenage rebellion and alienation."
                },
                new BookModel
                {
                    Title = "Brave New World",
                    Author = "Aldous Huxley",
                    ISBN = "978-0060850524",
                    Genre = "Dystopian",
                    TotalCopies = 3,
                    AvailableCopies = 3,
                    Description = "A dystopian vision of a futuristic society."
                },
                new BookModel
                {
                    Title = "The Lord of the Rings",
                    Author = "J.R.R. Tolkien",
                    ISBN = "978-0544003415",
                    Genre = "Fantasy",
                    TotalCopies = 6,
                    AvailableCopies = 6,
                    Description = "An epic fantasy trilogy."
                },
                new BookModel
                {
                    Title = "Animal Farm",
                    Author = "George Orwell",
                    ISBN = "978-0451526342",
                    Genre = "Political Fiction",
                    TotalCopies = 4,
                    AvailableCopies = 4,
                    Description = "A satirical allegory of Soviet totalitarianism."
                }
            };

            await context.Books.AddRangeAsync(books);
            await context.SaveChangesAsync();
            Console.WriteLine($"✅ Seeded {books.Count} books");

            Console.WriteLine("👥 Seeding users...");

            // Seed Students
            var students = new List<Student>
            {
                new Student
                {
                    Username = "john_doe",
                    Email = "john.doe@university.edu",
                    StudentId = "S2024001",
                    MembershipPoints = 150
                },
                new Student
                {
                    Username = "jane_smith",
                    Email = "jane.smith@university.edu",
                    StudentId = "S2024002",
                    MembershipPoints = 300
                },
                new Student
                {
                    Username = "mike_johnson",
                    Email = "mike.johnson@university.edu",
                    StudentId = "S2024003",
                    MembershipPoints = 50
                },
                new Student
                {
                    Username = "emily_brown",
                    Email = "emily.brown@university.edu",
                    StudentId = "S2024004",
                    MembershipPoints = 600
                },
                new Student
                {
                    Username = "alex_wilson",
                    Email = "alex.wilson@university.edu",
                    StudentId = "S2024005",
                    MembershipPoints = 200
                }
            };

            await context.Students.AddRangeAsync(students);
            await context.SaveChangesAsync();
            Console.WriteLine($"✅ Seeded {students.Count} students");

            // Seed Faculty Members
            var faculty = new List<FacultyMember>
            {
                new FacultyMember
                {
                    Username = "prof_smith",
                    Email = "smith@university.edu",
                    FacultyId = "F001",
                    Department = "Computer Science",
                    MembershipPoints = 800
                },
                new FacultyMember
                {
                    Username = "prof_jones",
                    Email = "jones@university.edu",
                    FacultyId = "F002",
                    Department = "Mathematics",
                    MembershipPoints = 1200
                },
                new FacultyMember
                {
                    Username = "prof_davis",
                    Email = "davis@university.edu",
                    FacultyId = "F003",
                    Department = "Physics",
                    MembershipPoints = 500
                }
            };

            await context.FacultyMembers.AddRangeAsync(faculty);
            await context.SaveChangesAsync();
            Console.WriteLine($"✅ Seeded {faculty.Count} faculty members");

            // Seed Librarians
            var librarians = new List<Librarian>
            {
                new Librarian
                {
                    Username = "lib_alice",
                    Email = "alice@library.edu",
                    
                    EmployeeId = "LIB001",
                },
                new Librarian
                {
                    Username = "lib_bob",
                    Email = "bob@library.edu",
                    EmployeeId = "LIB002",
                }
            };

            await context.Librarians.AddRangeAsync(librarians);
            await context.SaveChangesAsync();
            Console.WriteLine($"✅ Seeded {librarians.Count} librarians");

            // Seed Admins
            var admins = new List<Admin>
            {
                new Admin
                {
                    Username = "admin",
                    Email = "admin@library.edu",
                    AdminId = "ADM001",
                },
                new Admin
                {
                    Username = "superadmin",
                    Email = "superadmin@library.edu",
                    AdminId = "ADM002",
                }
            };

            await context.Admins.AddRangeAsync(admins);
            await context.SaveChangesAsync();
            Console.WriteLine($"✅ Seeded {admins.Count} admins");

            Console.WriteLine("✅ Database seeded successfully!");
        }
    }
}