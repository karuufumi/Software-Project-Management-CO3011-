using backend.Data;
using backend.repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// PostgreSQL configuration
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Get connection string from environment or config
var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
    ?? builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Database connection string is required");
}

// Configure DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Register repositories
builder.Services.AddScoped(typeof(IUserRepository<>), typeof(UserRepository<>));
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBorrowRepository, BorrowRepository>();
builder.Services.AddScoped<IQueueRepository, QueueRepository>();
builder.Services.AddScoped<IMembershipRepository, MembershipRepository>();

var app = builder.Build();

// Apply migrations and seed database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    try
    {
        Console.WriteLine("🔄 Applying migrations...");
        await context.Database.MigrateAsync();
        Console.WriteLine("✅ Migrations applied");
        
        Console.WriteLine("🌱 Seeding database...");
        await DbSeeder.SeedDatabase(context);
        Console.WriteLine("✅ Database ready");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"⚠️ Database setup: {ex.Message}");
        if (ex.Message.Contains("already exists"))
        {
            try { await DbSeeder.SeedDatabase(context); } catch { }
        }
    }
}

// Configure middleware
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();

// Root endpoint - HTML Homepage
app.MapGet("/", () => Results.Content(@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Library Management System API</title>
    <style>
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }
        body {
            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial, sans-serif;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            min-height: 100vh;
            display: flex;
            align-items: center;
            justify-content: center;
            padding: 20px;
        }
        .container {
            background: white;
            border-radius: 20px;
            box-shadow: 0 20px 60px rgba(0,0,0,0.3);
            max-width: 900px;
            width: 100%;
            padding: 40px;
            animation: fadeIn 0.5s ease-in;
        }
        @keyframes fadeIn {
            from { opacity: 0; transform: translateY(-20px); }
            to { opacity: 1; transform: translateY(0); }
        }
        h1 {
            color: #667eea;
            font-size: 2.5em;
            margin-bottom: 10px;
            display: flex;
            align-items: center;
            gap: 15px;
        }
        .status {
            display: inline-block;
            background: #10b981;
            color: white;
            padding: 5px 15px;
            border-radius: 20px;
            font-size: 0.4em;
            font-weight: 600;
            text-transform: uppercase;
            animation: pulse 2s infinite;
        }
        @keyframes pulse {
            0%, 100% { opacity: 1; }
            50% { opacity: 0.7; }
        }
        .subtitle {
            color: #6b7280;
            font-size: 1.1em;
            margin-bottom: 30px;
        }
        .info-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
            gap: 15px;
            margin-bottom: 30px;
        }
        .info-card {
            background: #f9fafb;
            padding: 15px;
            border-radius: 10px;
            border-left: 4px solid #667eea;
        }
        .info-label {
            color: #6b7280;
            font-size: 0.85em;
            font-weight: 600;
            text-transform: uppercase;
            letter-spacing: 0.5px;
        }
        .info-value {
            color: #1f2937;
            font-size: 1.1em;
            font-weight: 700;
            margin-top: 5px;
        }
        .section {
            margin-bottom: 30px;
        }
        .section-title {
            color: #1f2937;
            font-size: 1.3em;
            font-weight: 700;
            margin-bottom: 15px;
            padding-bottom: 10px;
            border-bottom: 2px solid #e5e7eb;
        }
        .endpoints {
            display: grid;
            gap: 10px;
        }
        .endpoint {
            background: #f9fafb;
            padding: 15px;
            border-radius: 8px;
            display: flex;
            justify-content: space-between;
            align-items: center;
            transition: all 0.3s ease;
        }
        .endpoint:hover {
            background: #f3f4f6;
            transform: translateX(5px);
        }
        .endpoint-path {
            font-family: 'Courier New', monospace;
            color: #667eea;
            font-weight: 600;
        }
        .endpoint-desc {
            color: #6b7280;
            font-size: 0.9em;
        }
        .btn {
            display: inline-block;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 12px 30px;
            border-radius: 8px;
            text-decoration: none;
            font-weight: 600;
            transition: all 0.3s ease;
            box-shadow: 0 4px 15px rgba(102, 126, 234, 0.4);
        }
        .btn:hover {
            transform: translateY(-2px);
            box-shadow: 0 6px 20px rgba(102, 126, 234, 0.6);
        }
        .footer {
            margin-top: 30px;
            padding-top: 20px;
            border-top: 2px solid #e5e7eb;
            text-align: center;
            color: #6b7280;
            font-size: 0.9em;
        }
    </style>
</head>
<body>
    <div class='container'>
        <h1>
            📚 Library Management System
            <span class='status'>● Live</span>
        </h1>
        <p class='subtitle'>RESTful API for managing library operations, books, users, and borrowing system</p>
        
        <div class='info-grid'>
            <div class='info-card'>
                <div class='info-label'>Version</div>
                <div class='info-value'>1.0.0</div>
            </div>
            <div class='info-card'>
                <div class='info-label'>Database</div>
                <div class='info-value'>PostgreSQL</div>
            </div>
            <div class='info-card'>
                <div class='info-label'>Environment</div>
                <div class='info-value'>Production</div>
            </div>
            <div class='info-card'>
                <div class='info-label'>Framework</div>
                <div class='info-value'>.NET 8</div>
            </div>
        </div>

        <div class='section'>
            <div class='section-title'>📖 Documentation</div>
            <a href='/swagger' class='btn'>🚀 Open Swagger UI</a>
        </div>

        <div class='section'>
            <div class='section-title'>🔗 API Endpoints</div>
            <div class='endpoints'>
                <div class='endpoint'>
                    <span class='endpoint-path'>/api/book</span>
                    <span class='endpoint-desc'>Books management</span>
                </div>
                <div class='endpoint'>
                    <span class='endpoint-path'>/api/user</span>
                    <span class='endpoint-desc'>User management</span>
                </div>
                <div class='endpoint'>
                    <span class='endpoint-path'>/api/user/students</span>
                    <span class='endpoint-desc'>Student users</span>
                </div>
                <div class='endpoint'>
                    <span class='endpoint-path'>/api/user/faculty</span>
                    <span class='endpoint-desc'>Faculty members</span>
                </div>
                <div class='endpoint'>
                    <span class='endpoint-path'>/api/user/librarians</span>
                    <span class='endpoint-desc'>Librarian accounts</span>
                </div>
                <div class='endpoint'>
                    <span class='endpoint-path'>/api/user/admins</span>
                    <span class='endpoint-desc'>Administrator accounts</span>
                </div>
                <div class='endpoint'>
                    <span class='endpoint-path'>/api/borrow</span>
                    <span class='endpoint-desc'>Borrowing operations</span>
                </div>
                <div class='endpoint'>
                    <span class='endpoint-path'>/api/queue</span>
                    <span class='endpoint-desc'>Book reservation queue</span>
                </div>
                <div class='endpoint'>
                    <span class='endpoint-path'>/api/membership</span>
                    <span class='endpoint-desc'>Membership management</span>
                </div>
                <div class='endpoint'>
                    <span class='endpoint-path'>/api/dashboard</span>
                    <span class='endpoint-desc'>Dashboard statistics</span>
                </div>
                <div class='endpoint'>
                    <span class='endpoint-path'>/health</span>
                    <span class='endpoint-desc'>Health check</span>
                </div>
            </div>
        </div>

        <div class='footer'>
            <p>🎓 Software Project Management CO3011</p>
            <p>Built with ASP.NET Core & PostgreSQL</p>
        </div>
    </div>
</body>
</html>
", "text/html"));

// Health check endpoint
app.MapGet("/health", async (ApplicationDbContext context) =>
{
    try
    {
        var canConnect = await context.Database.CanConnectAsync();
        
        return Results.Json(new
        {
            status = canConnect ? "healthy" : "unhealthy",
            database = new
            {
                connected = canConnect,
                type = "PostgreSQL",
                provider = "Npgsql"
            },
            application = new
            {
                version = "1.0.0",
                environment = app.Environment.EnvironmentName,
                timestamp = DateTime.UtcNow
            }
        });
    }
    catch (Exception ex)
    {
        return Results.Json(new
        {
            status = "unhealthy",
            error = ex.Message,
            timestamp = DateTime.UtcNow
        });
    }
});

Console.WriteLine("✅ Application started successfully!");
Console.WriteLine($"📍 Environment: {app.Environment.EnvironmentName}");
Console.WriteLine($"📍 Database: PostgreSQL");
Console.WriteLine($"📍 Swagger: /swagger");

app.Run();