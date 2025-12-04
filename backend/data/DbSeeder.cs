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

            // --------------------------------------------------------
            // BOOK LIST (Original + New + Extra + Added 3 more)
            // --------------------------------------------------------
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
                },

                // Your Added Books
                new BookModel
                {
                    Title = "Principles of Programming Languages",
                    Author = "Michael L. Scott",
                    ISBN = "978-0131486814",
                    Genre = "Computer Science",
                    TotalCopies = 4,
                    AvailableCopies = 4,
                    Description = "A comprehensive introduction to programming language theory."
                },
                new BookModel
                {
                    Title = "Probability and Statistics",
                    Author = "Morris H. DeGroot",
                    ISBN = "978-0321500465",
                    Genre = "Mathematics",
                    TotalCopies = 4,
                    AvailableCopies = 4,
                    Description = "A foundational text on probability theory and statistics."
                },
                new BookModel
                {
                    Title = "Calculus",
                    Author = "James Stewart",
                    ISBN = "978-1285740621",
                    Genre = "Mathematics",
                    TotalCopies = 5,
                    AvailableCopies = 5,
                    Description = "A standard university-level calculus textbook."
                },
                new BookModel
                {
                    Title = "Software Engineering",
                    Author = "Ian Sommerville",
                    ISBN = "978-0137035153",
                    Genre = "Computer Science",
                    TotalCopies = 4,
                    AvailableCopies = 4,
                    Description = "A widely used textbook on software engineering principles."
                },
                new BookModel
                {
                    Title = "Naruto Vol.1",
                    Author = "Masashi Kishimoto",
                    ISBN = "978-1569319000",
                    Genre = "Manga",
                    TotalCopies = 6,
                    AvailableCopies = 6,
                    Description = "The beginning of Naruto Uzumaki's ninja journey."
                },
                new BookModel
                {
                    Title = "Chainsaw Man Vol.3",
                    Author = "Tatsuki Fujimoto",
                    ISBN = "978-1974717276",
                    Genre = "Manga",
                    TotalCopies = 6,
                    AvailableCopies = 6,
                    Description = "The third installment of the Chainsaw Man manga series."
                },

                // Extra Random Books
                new BookModel
                {
                    Title = "Introduction to Machine Learning",
                    Author = "Ethem Alpaydin",
                    ISBN = "978-0262043793",
                    Genre = "Machine Learning",
                    TotalCopies = 4,
                    AvailableCopies = 4,
                    Description = "A structured introduction to machine learning concepts."
                },
                new BookModel
                {
                    Title = "Database System Concepts",
                    Author = "Avi Silberschatz",
                    ISBN = "978-9332901387",
                    Genre = "Computer Science",
                    TotalCopies = 4,
                    AvailableCopies = 4,
                    Description = "A core textbook on database systems and SQL."
                },
                new BookModel
                {
                    Title = "One Piece Vol.1",
                    Author = "Eiichiro Oda",
                    ISBN = "978-1569319017",
                    Genre = "Manga",
                    TotalCopies = 7,
                    AvailableCopies = 7,
                    Description = "The beginning of Monkey D. Luffy's pirate adventure."
                },
                new BookModel
                {
                    Title = "The Art of War",
                    Author = "Sun Tzu",
                    ISBN = "978-1599869773",
                    Genre = "Strategy",
                    TotalCopies = 4,
                    AvailableCopies = 4,
                    Description = "An ancient Chinese military treatise."
                },
                new BookModel
                {
                    Title = "Clean Code",
                    Author = "Robert C. Martin",
                    ISBN = "978-0132350884",
                    Genre = "Software Engineering",
                    TotalCopies = 5,
                    AvailableCopies = 5,
                    Description = "A handbook of agile software craftsmanship."
                },

                // 3 More Books You Requested
                new BookModel
                {
                    Title = "Design Patterns",
                    Author = "Erich Gamma, Richard Helm, Ralph Johnson, John Vlissides",
                    ISBN = "978-0201633610",
                    Genre = "Software Engineering",
                    TotalCopies = 4,
                    AvailableCopies = 4,
                    Description = "Classic reusable solutions in software design."
                },
                new BookModel
                {
                    Title = "Deep Learning",
                    Author = "Ian Goodfellow, Yoshua Bengio, Aaron Courville",
                    ISBN = "978-0262035613",
                    Genre = "Artificial Intelligence",
                    TotalCopies = 5,
                    AvailableCopies = 5,
                    Description = "Foundational deep learning textbook."
                },
                new BookModel
                {
                    Title = "Tokyo Ghoul Vol.1",
                    Author = "Sui Ishida",
                    ISBN = "978-1421580364",
                    Genre = "Manga",
                    TotalCopies = 6,
                    AvailableCopies = 6,
                    Description = "The dark beginning of Kaneki Ken's transformation."
                }
            };

            await context.Books.AddRangeAsync(books);
            await context.SaveChangesAsync();
            Console.WriteLine($"✅ Seeded {books.Count} books");

            // --------------------------------------------------------
            // 10 Vietnamese Students
            // --------------------------------------------------------
            Console.WriteLine("👥 Seeding Vietnamese students...");

            string[] vnStudentNames =
            {
                "Nguyen Van An", "Tran Thi Bich", "Le Hoang Nam", "Pham Minh Khang",
                "Hoang Gia Bao", "Vo Thi Kim", "Dang Quoc Huy", "Bui Thanh Phuong",
                "Do Ngoc Lan", "Phan Bao Chau"
            };

            var students = vnStudentNames.Select(name =>
            {
                var clean = RemoveVietnameseTones(name).ToLower().Replace(" ", "");
                return new Student
                {
                    Username = clean,
                    Email = clean + "@student.hcmut.edu.vn",
                     //= RemoveVietnameseTones(name),
                    StudentId = "225" + _random.Next(1000, 9999),
                    MembershipPoints = _random.Next(30, 1401)
                };
            }).ToList();

            await context.Students.AddRangeAsync(students);
            await context.SaveChangesAsync();
            Console.WriteLine($"✅ Seeded {students.Count} Vietnamese students");

            // --------------------------------------------------------
            // Faculty
            // --------------------------------------------------------
            Console.WriteLine("👨‍🏫 Seeding Vietnamese faculty members...");

            var facultyData = new[]
            {
                new { Name = "TS Nguyen Van Minh", Dept = "Khoa Khoa Hoc May Tinh", FacultyId = "F225001" },
                new { Name = "PGS Tran Thi Huong", Dept = "Khoa Ky Thuat Dien Tu", FacultyId = "F225002" },
                new { Name = "GS Le Quoc Tuan", Dept = "Khoa Toan - Tin Hoc", FacultyId = "F225003" }
            };

            var faculty = facultyData.Select(f =>
            {
                var clean = RemoveVietnameseTones(f.Name).ToLower().Replace(" ", "");
                return new FacultyMember
                {
                    Username = clean,
                    Email = clean + "@hcmut.edu.vn",
                    FacultyId = f.FacultyId,
                    Department = f.Dept,
                    MembershipPoints = _random.Next(500, 1501)
                };
            }).ToList();

            await context.FacultyMembers.AddRangeAsync(faculty);
            await context.SaveChangesAsync();
            Console.WriteLine($"✅ Seeded {faculty.Count} Vietnamese faculty");

            // --------------------------------------------------------
            // Librarians
            // --------------------------------------------------------
            Console.WriteLine("📚 Seeding Vietnamese librarians...");

            string[] librarianNames =
            {
                "Nguyen Thanh Ha", "Tran Cong Minh",
                "Pham Thi Dung", "Le Quoc Trung"
            };

            var librarians = librarianNames.Select(name =>
            {
                var clean = RemoveVietnameseTones(name).ToLower().Replace(" ", "");
                return new Librarian
                {
                    Username = clean,
                    Email = clean + "@lib.hcmut.edu.vn",
                    //FullName = RemoveVietnameseTones(name),
                    EmployeeId = "LIB225" + _random.Next(1000, 9999)
                };
            }).ToList();

            await context.Librarians.AddRangeAsync(librarians);
            await context.SaveChangesAsync();
            Console.WriteLine($"✅ Seeded {librarians.Count} Vietnamese librarians");

            // --------------------------------------------------------
            // Admins
            // --------------------------------------------------------
            Console.WriteLine("🛡️ Seeding Vietnamese admins...");

            var adminData = new[]
            {
                new { Name = "Quan Tri Vien Nguyen", Username = "admin_nguyen", AdminId = "ADM225001" },
                new { Name = "Sieu Quan Tri Tran", Username = "superadmin_tran", AdminId = "ADM225002" }
            };

            var admins = adminData.Select(a => new Admin
            {
                Username = a.Username,
                Email = a.Username + "@admin.hcmut.edu.vn",
                //FullName = RemoveVietnameseTones(a.Name),
                AdminId = a.AdminId
            }).ToList();

            await context.Admins.AddRangeAsync(admins);
            await context.SaveChangesAsync();
            Console.WriteLine($"✅ Seeded {admins.Count} Vietnamese admins");

            Console.WriteLine("🎉 Database seeded successfully!");
        }

        // --------------------------------------------------------
        // Helper - Remove Vietnamese tones from names/emails
        // --------------------------------------------------------
        private static string RemoveVietnameseTones(string text)
        {
            string[] vietChars =
            {
                "àáạảãâầấậẩẫăằắặẳẵ",
                "èéẹẻẽêềếệểễ",
                "ìíịỉĩ",
                "òóọỏõôồốộổỗơờớợởỡ",
                "ùúụủũưừứựửữ",
                "ỳýỵỷỹ",
                "đ",
                "ÀÁẠẢÃÂẦẤẬẨẪĂẰẮẶẲẴ",
                "ÈÉẸẺẼÊỀẾỆỂỄ",
                "ÌÍỊỈĨ",
                "ÒÓỌỎÕÔỒỐỘỔỖƠỜỚỢỞỠ",
                "ÙÚỤỦŨƯỪỨỰỬỮ",
                "ỲÝỴỶỸ",
                "Đ"
            };

            string[] replaceChars =
            {
                "a","e","i","o","u","y","d","A","E","I","O","U","Y","D"
            };

            for (int i = 0; i < vietChars.Length; i++)
            {
                foreach (char c in vietChars[i])
                {
                    text = text.Replace(c, replaceChars[i][0]);
                }
            }

            return text;
        }
    }
}
