using backend.models;
using backend.repository;
using Microsoft.AspNetCore.Mvc;

namespace backend.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository<UserModel> _userRepository;
        private readonly IBorrowRepository _borrowRepository;
        private readonly IMembershipRepository _membershipRepository;

        public DashboardController(
            IBookRepository bookRepository,
            IUserRepository<UserModel> userRepository,
            IBorrowRepository borrowRepository,
            IMembershipRepository membershipRepository)
        {
            _bookRepository = bookRepository;
            _userRepository = userRepository;
            _borrowRepository = borrowRepository;
            _membershipRepository = membershipRepository;
        }

        // GET: api/dashboard/stats
        [HttpGet("stats")]
        public async Task<ActionResult<ApiResponse<DashboardStats>>> GetOverallStats()
        {
            try
            {
                var allBooks = await _bookRepository.GetAllBooks();
                var allUsers = await _userRepository.GetAllUsers();
                var pendingRequests = await _borrowRepository.GetPendingRequests();

                var totalBorrowedBooks = allBooks.Sum(b => b.TotalCopies - b.AvailableCopies);
                
                var allBorrows = allUsers
                    .SelectMany(u => u.BorrowedBooks ?? Enumerable.Empty<BorrowRecord>())
                    .ToList();

                var activeBorrows = allBorrows
                    .Where(b => !b.IsReturned && b.DueDate != null)
                    .Count();

                var overdueBorrows = allBorrows
                    .Where(b => !b.IsReturned && b.DueDate != null && b.DueDate < DateTime.UtcNow)
                    .Count();

                // Count users by role
                var students = allUsers.OfType<Student>().Count();
                var faculty = allUsers.OfType<FacultyMember>().Count();
                var librarians = allUsers.OfType<Librarian>().Count();
                var admins = allUsers.OfType<Admin>().Count();

                var stats = new DashboardStats
                {
                    TotalBooks = allBooks.Count(),
                    AvailableBooks = allBooks.Sum(b => b.AvailableCopies),
                    BorrowedBooks = totalBorrowedBooks,
                    TotalUsers = allUsers.Count(),
                    StudentCount = students,
                    FacultyCount = faculty,
                    LibrarianCount = librarians,
                    AdminCount = admins,
                    ActiveBorrows = activeBorrows,
                    OverdueBorrows = overdueBorrows,
                    PendingRequests = pendingRequests.Count(),
                    TotalBorrowsAllTime = allBorrows.Count
                };

                return Ok(new ApiResponse<DashboardStats>
                {
                    Success = true,
                    Data = stats,
                    Message = "Dashboard statistics retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to retrieve dashboard statistics"
                });
            }
        }

        // GET: api/dashboard/popular-books?limit=10
        [HttpGet("popular-books")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PopularBookInfo>>>> GetPopularBooks([FromQuery] int limit = 10)
        {
            try
            {
                var allUsers = await _userRepository.GetAllUsers();
                var allBorrows = allUsers
                    .SelectMany(u => u.BorrowedBooks ?? Enumerable.Empty<BorrowRecord>())
                    .ToList();

                // Group by book and count borrows
                var popularBooks = allBorrows
                    .GroupBy(b => b.BookId)
                    .Select(g => new
                    {
                        BookId = g.Key,
                        BorrowCount = g.Count()
                    })
                    .OrderByDescending(x => x.BorrowCount)
                    .Take(limit)
                    .ToList();

                var result = new List<PopularBookInfo>();

                foreach (var item in popularBooks)
                {
                    var book = await _bookRepository.GetBookById(item.BookId);
                    if (book != null)
                    {
                        result.Add(new PopularBookInfo
                        {
                            BookId = book.BookId,
                            Title = book.Title,
                            Author = book.Author,
                            Genre = book.Genre,
                            BorrowCount = item.BorrowCount,
                            AvailableCopies = book.AvailableCopies,
                            TotalCopies = book.TotalCopies
                        });
                    }
                }

                return Ok(new ApiResponse<IEnumerable<PopularBookInfo>>
                {
                    Success = true,
                    Data = result,
                    Message = $"Retrieved top {result.Count} popular books"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to retrieve popular books"
                });
            }
        }

        // GET: api/dashboard/active-users?limit=10
        [HttpGet("active-users")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ActiveUserInfo>>>> GetActiveUsers([FromQuery] int limit = 10)
        {
            try
            {
                var allUsers = await _userRepository.GetAllUsers();

                var activeUsers = allUsers
                    .Where(u => u is Student || u is FacultyMember)
                    .Select(u => new ActiveUserInfo
                    {
                        UserId = u.Id,
                        Username = u.Username,
                        Email = u.Email,
                        Role = u.Role,
                        TotalBorrows = u.BorrowedBooks?.Count ?? 0,
                        ActiveBorrows = u.BorrowedBooks?.Count(b => !b.IsReturned && b.DueDate != null) ?? 0,
                        MembershipPoints = u is Student s ? s.MembershipPoints : 
                                         u is FacultyMember f ? f.MembershipPoints : 0
                    })
                    .OrderByDescending(u => u.TotalBorrows)
                    .Take(limit)
                    .ToList();

                return Ok(new ApiResponse<IEnumerable<ActiveUserInfo>>
                {
                    Success = true,
                    Data = activeUsers,
                    Message = $"Retrieved top {activeUsers.Count} active users"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to retrieve active users"
                });
            }
        }

        // GET: api/dashboard/overdue-summary
        [HttpGet("overdue-summary")]
        public async Task<ActionResult<ApiResponse<OverdueSummary>>> GetOverdueSummary()
        {
            try
            {
                var allUsers = await _userRepository.GetAllUsers();
                var allBorrows = allUsers
                    .SelectMany(u => u.BorrowedBooks ?? Enumerable.Empty<BorrowRecord>())
                    .ToList();

                var overdueBorrows = allBorrows
                    .Where(b => !b.IsReturned && b.DueDate != null && b.DueDate < DateTime.UtcNow)
                    .ToList();

                var overdueDetails = new List<OverdueDetail>();
                int totalPenalty = 0;

                foreach (var borrow in overdueBorrows)
                {
                    var user = allUsers.FirstOrDefault(u => u.Id == borrow.UserId);
                    var book = await _bookRepository.GetBookById(borrow.BookId);
                    
                    if (user != null && book != null && borrow.DueDate != null)
                    {
                        var daysOverdue = (DateTime.UtcNow - borrow.DueDate.Value).Days;
                        var penalty = daysOverdue * 10; // 10 points per day
                        totalPenalty += penalty;

                        overdueDetails.Add(new OverdueDetail
                        {
                            BorrowId = borrow.BorrowId,
                            UserId = user.Id,
                            Username = user.Username,
                            BookId = book.BookId,
                            BookTitle = book.Title,
                            DueDate = borrow.DueDate.Value,
                            DaysOverdue = daysOverdue,
                            PenaltyPoints = penalty
                        });
                    }
                }

                var summary = new OverdueSummary
                {
                    TotalOverdue = overdueBorrows.Count,
                    TotalPenaltyPoints = totalPenalty,
                    OverdueDetails = overdueDetails.OrderByDescending(d => d.DaysOverdue).ToList()
                };

                return Ok(new ApiResponse<OverdueSummary>
                {
                    Success = true,
                    Data = summary,
                    Message = $"Found {summary.TotalOverdue} overdue borrow(s)"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to retrieve overdue summary"
                });
            }
        }

        // GET: api/dashboard/recent-activity?limit=20
        [HttpGet("recent-activity")]
        public async Task<ActionResult<ApiResponse<IEnumerable<RecentActivity>>>> GetRecentActivity([FromQuery] int limit = 20)
        {
            try
            {
                var allUsers = await _userRepository.GetAllUsers();
                var allBorrows = allUsers
                    .SelectMany(u => u.BorrowedBooks ?? Enumerable.Empty<BorrowRecord>())
                    .OrderByDescending(b => b.BorrowDate)
                    .Take(limit)
                    .ToList();

                var activities = new List<RecentActivity>();

                foreach (var borrow in allBorrows)
                {
                    var user = allUsers.FirstOrDefault(u => u.Id == borrow.UserId);
                    var book = await _bookRepository.GetBookById(borrow.BookId);

                    if (user != null && book != null)
                    {
                        string activityType;
                        DateTime activityDate;

                        if (borrow.IsReturned)
                        {
                            activityType = "Returned";
                            activityDate = borrow.ReturnDate ?? borrow.BorrowDate;
                        }
                        else if (borrow.DueDate != null)
                        {
                            activityType = "Borrowed";
                            activityDate = borrow.BorrowDate;
                        }
                        else
                        {
                            activityType = "Requested";
                            activityDate = borrow.BorrowDate;
                        }

                        activities.Add(new RecentActivity
                        {
                            ActivityType = activityType,
                            ActivityDate = activityDate,
                            Username = user.Username,
                            BookTitle = book.Title,
                            BorrowId = borrow.BorrowId
                        });
                    }
                }

                return Ok(new ApiResponse<IEnumerable<RecentActivity>>
                {
                    Success = true,
                    Data = activities.OrderByDescending(a => a.ActivityDate),
                    Message = $"Retrieved {activities.Count} recent activities"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to retrieve recent activity"
                });
            }
        }

        // GET: api/dashboard/genre-distribution
        [HttpGet("genre-distribution")]
        public async Task<ActionResult<ApiResponse<IEnumerable<GenreDistribution>>>> GetGenreDistribution()
        {
            try
            {
                var allBooks = await _bookRepository.GetAllBooks();

                var distribution = allBooks
                    .GroupBy(b => b.Genre)
                    .Select(g => new GenreDistribution
                    {
                        Genre = g.Key,
                        BookCount = g.Count(),
                        TotalCopies = g.Sum(b => b.TotalCopies),
                        AvailableCopies = g.Sum(b => b.AvailableCopies)
                    })
                    .OrderByDescending(d => d.BookCount)
                    .ToList();

                return Ok(new ApiResponse<IEnumerable<GenreDistribution>>
                {
                    Success = true,
                    Data = distribution,
                    Message = $"Retrieved distribution for {distribution.Count} genres"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to retrieve genre distribution"
                });
            }
        }

        // GET: api/dashboard/membership-distribution
        [HttpGet("membership-distribution")]
        public async Task<ActionResult<ApiResponse<IEnumerable<TierDistribution>>>> GetMembershipDistribution()
        {
            try
            {
                var allUsers = await _userRepository.GetAllUsers();
                var eligibleUsers = allUsers.Where(u => u is Student || u is FacultyMember).ToList();

                var distribution = new List<TierDistribution>
                {
                    new TierDistribution 
                    { 
                        TierName = "Bronze (0-99)", 
                        UserCount = eligibleUsers.Count(u => GetUserPoints(u) < 100) 
                    },
                    new TierDistribution 
                    { 
                        TierName = "Silver (100-499)", 
                        UserCount = eligibleUsers.Count(u => GetUserPoints(u) >= 100 && GetUserPoints(u) < 500) 
                    },
                    new TierDistribution 
                    { 
                        TierName = "Gold (500-999)", 
                        UserCount = eligibleUsers.Count(u => GetUserPoints(u) >= 500 && GetUserPoints(u) < 1000) 
                    },
                    new TierDistribution 
                    { 
                        TierName = "Platinum (1000+)", 
                        UserCount = eligibleUsers.Count(u => GetUserPoints(u) >= 1000) 
                    }
                };

                return Ok(new ApiResponse<IEnumerable<TierDistribution>>
                {
                    Success = true,
                    Data = distribution,
                    Message = "Membership tier distribution retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError
                {
                    Error = ex.Message,
                    Message = "Failed to retrieve membership distribution"
                });
            }
        }

        // Helper method
        private int GetUserPoints(UserModel user)
        {
            if (user is Student student)
                return student.MembershipPoints;
            if (user is FacultyMember faculty)
                return faculty.MembershipPoints;
            return 0;
        }
    }

    // Response DTOs
    public class DashboardStats
    {
        public int TotalBooks { get; set; }
        public int AvailableBooks { get; set; }
        public int BorrowedBooks { get; set; }
        public int TotalUsers { get; set; }
        public int StudentCount { get; set; }
        public int FacultyCount { get; set; }
        public int LibrarianCount { get; set; }
        public int AdminCount { get; set; }
        public int ActiveBorrows { get; set; }
        public int OverdueBorrows { get; set; }
        public int PendingRequests { get; set; }
        public int TotalBorrowsAllTime { get; set; }
    }

    public class PopularBookInfo
    {
        public string BookId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public int BorrowCount { get; set; }
        public int AvailableCopies { get; set; }
        public int TotalCopies { get; set; }
    }

    public class ActiveUserInfo
    {
        public string UserId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int TotalBorrows { get; set; }
        public int ActiveBorrows { get; set; }
        public int MembershipPoints { get; set; }
    }

    public class OverdueSummary
    {
        public int TotalOverdue { get; set; }
        public int TotalPenaltyPoints { get; set; }
        public List<OverdueDetail> OverdueDetails { get; set; } = new();
    }

    public class OverdueDetail
    {
        public string BorrowId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string BookId { get; set; } = string.Empty;
        public string BookTitle { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public int DaysOverdue { get; set; }
        public int PenaltyPoints { get; set; }
    }

    public class RecentActivity
    {
        public string ActivityType { get; set; } = string.Empty; // "Requested", "Borrowed", "Returned"
        public DateTime ActivityDate { get; set; }
        public string Username { get; set; } = string.Empty;
        public string BookTitle { get; set; } = string.Empty;
        public string BorrowId { get; set; } = string.Empty;
    }

    public class GenreDistribution
    {
        public string Genre { get; set; } = string.Empty;
        public int BookCount { get; set; }
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
    }

    public class TierDistribution
    {
        public string TierName { get; set; } = string.Empty;
        public int UserCount { get; set; }
    }
}