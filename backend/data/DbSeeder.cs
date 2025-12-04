using backend.models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public static class DbSeeder
    {
        private static readonly Random _random = new();

        public static async Task SeedDatabase(ApplicationDbContext context)
        {
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

            // -----------------------------------------
            // 🧑‍🎓 SEED 10 VIETNAMESE STUDENTS
            // -----------------------------------------
            Console.WriteLine("👥 Seeding Vietnamese students...");

            string[] vnNames = new[]
            {
                "Nguyen Van An", "Tran Thi Bich", "Le Hoang Nam", "Pham Minh Khang",
                "Hoang Gia Bao", "Vo Thi Kim", "Dang Quoc Huy", "Bui Thanh Phuong",
                "Do Ngoc Lan", "Phan Bao Chau"
            };

            var students = vnNames.Select((name, index) => new Student
            {
                Username = name.ToLower().Replace(" ", "_"),
                Email = name.ToLower().Replace(" ", "") + "@gmail.com",
                StudentId = "225" + _random.Next(1000, 9999),
                Role = "Student", // ✅ Added Role
                MembershipPoints = _random.Next(30, 1401)
            }).ToList();

            await context.Students.AddRangeAsync(students);
            await context.SaveChangesAsync();
            Console.WriteLine($"✅ Seeded {students.Count} Vietnamese students");

            // -----------------------------------------
            // 📚 SEED 4 VIETNAMESE LIBRARIANS
            // -----------------------------------------
            Console.WriteLine("📚 Seeding Vietnamese librarians...");

            string[] librarianNames = new[]
            {
                "Nguyen Thanh Ha",
                "Tran Cong Minh",
                "Pham Thi Dung",
                "Le Quoc Trung"
            };

            var librarians = librarianNames.Select(name => new Librarian
            {
                Username = name.ToLower().Replace(" ", "_"),
                Email = name.ToLower().Replace(" ", "") + "@gmail.com",
                EmployeeId = "LIB225" + _random.Next(1000, 9999),
                Role = "Librarian" // ✅ Added Role
            }).ToList();

            await context.Librarians.AddRangeAsync(librarians);
            await context.SaveChangesAsync();
            Console.WriteLine($"✅ Seeded {librarians.Count} Vietnamese librarians");

            // -----------------------------------------
            // 🛡️ SEED 2 VIETNAMESE ADMINS
            // -----------------------------------------
            Console.WriteLine("🛡️ Seeding Vietnamese admins...");

            string[] adminNames = new[]
            {
                "Admin Nguyen",
                "Super Admin Tran"
            };

            var admins = adminNames.Select(name => new Admin
            {
                Username = name.ToLower().Replace(" ", "_"),
                Email = name.ToLower().Replace(" ", "") + "@gmail.com",
                AdminId = "ADM225" + _random.Next(1000, 9999),
                Role = "Admin" // ✅ Added Role
            }).ToList();

            await context.Admins.AddRangeAsync(admins);
            await context.SaveChangesAsync();
            Console.WriteLine($"✅ Seeded {admins.Count} Vietnamese admins");

            Console.WriteLine("🎉 Database seeded successfully!");
        }
    }
}