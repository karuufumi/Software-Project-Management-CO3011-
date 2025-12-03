using backend.Data;
using backend.repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext with SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories
builder.Services.AddScoped(typeof(IUserRepository<>), typeof(UserRepository<>));
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBorrowRepository, BorrowRepository>();
builder.Services.AddScoped<IQueueRepository, QueueRepository>();
builder.Services.AddScoped<IMembershipRepository, MembershipRepository>();

var app = builder.Build();

// Automatically apply migrations and seed the database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    Console.WriteLine("🔄 Applying migrations...");
    await context.Database.MigrateAsync();
    Console.WriteLine("✅ Migrations applied!");
    
    Console.WriteLine("🌱 Seeding database...");
    await DbSeeder.SeedDatabase(context);
}

// Enable Swagger in all environments
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

// Add root route with success message
// ...existing code...

// Add root route with HTML response
app.MapGet("/", () => Results.Content(@"
<!DOCTYPE html>
<html>
<head>
    <title>Library Management API</title>
    <style>
        body {
            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, sans-serif;
            display: flex;
            justify-content: center;
            align-items: center;
            min-height: 100vh;
            margin: 0;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
        }
        .container {
            text-align: center;
            padding: 3rem;
            background: rgba(255, 255, 255, 0.1);
            backdrop-filter: blur(10px);
            border-radius: 20px;
            box-shadow: 0 8px 32px 0 rgba(31, 38, 135, 0.37);
        }
        h1 { font-size: 3rem; margin: 0 0 1rem 0; }
        p { font-size: 1.2rem; margin: 0.5rem 0; opacity: 0.9; }
        .links {
            margin-top: 2rem;
            display: flex;
            gap: 1rem;
            justify-content: center;
            flex-wrap: wrap;
        }
        a {
            display: inline-block;
            padding: 0.8rem 1.5rem;
            background: white;
            color: #667eea;
            text-decoration: none;
            border-radius: 8px;
            font-weight: 600;
            transition: transform 0.2s;
        }
        a:hover {
            transform: translateY(-2px);
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
        }
        .status {
            display: inline-block;
            padding: 0.5rem 1rem;
            background: #10b981;
            border-radius: 20px;
            font-size: 0.9rem;
            margin-bottom: 1rem;
        }
    </style>
</head>
<body>
    <div class='container'>
        <div class='status'>✅ Status: Running</div>
        <h1>🎉 Library Management System</h1>
        <p>API Version 1.0.0</p>
        <p>Backend is successfully deployed and running!</p>
        <div class='links'>
            <a href='/swagger'>📖 API Documentation</a>
            <a href='/api/book'>📚 View Books</a>
            <a href='/api/dashboard/stats'>📊 Dashboard</a>
        </div>
    </div>
</body>
</html>
", "text/html"));

// ...existing code...

// Add health check endpoint


Console.WriteLine("\n🚀 Application is running!");
Console.WriteLine($"📍 Environment: {app.Environment.EnvironmentName}");
Console.WriteLine("📍 Swagger UI: http://localhost:5000/swagger");
Console.WriteLine("📍 API Base URL: http://localhost:5000/api");
Console.WriteLine("📍 Test endpoint: http://localhost:5000/api/book\n");

app.Run();