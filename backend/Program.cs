using backend.Data;
using backend.repository;
using Microsoft.EntityFrameworkCore;
using Npgsql;

Console.WriteLine("========================================");
Console.WriteLine("🚀 APPLICATION STARTING");
Console.WriteLine("========================================");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Get connection string
var connFromEnv = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
var connFromConfig = builder.Configuration.GetConnectionString("DefaultConnection");

string? connectionString = connFromEnv ?? connFromConfig;

if (string.IsNullOrWhiteSpace(connectionString))
{
    Console.WriteLine("❌ No connection string found!");
    throw new InvalidOperationException("Connection string required");
}

Console.WriteLine($"✅ Connection string found (length: {connectionString.Length})");
Console.WriteLine($"   First 30 chars: {connectionString.Substring(0, Math.Min(30, connectionString.Length))}...");

// Build proper connection strings to test
var testConnections = new Dictionary<string, string>();

// Try to detect format and create variations
if (connectionString.StartsWith("postgresql://") || connectionString.StartsWith("postgres://"))
{
    Console.WriteLine("📝 Detected URI format, converting to key-value...");
    
    try
    {
        // Parse URI manually
        var uri = new Uri(connectionString);
        var userInfo = uri.UserInfo.Split(':');
        var username = userInfo[0];
        var password = userInfo.Length > 1 ? userInfo[1] : "";
        
        // Create key-value connection string
        var kvConnStr = $"Host={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.TrimStart('/')};Username={username};Password={password}";
        
        testConnections["Key-Value SSL Require"] = kvConnStr + ";SSL Mode=Require;Trust Server Certificate=true";
        testConnections["Key-Value SSL Prefer"] = kvConnStr + ";SSL Mode=Prefer";
        testConnections["Key-Value No SSL"] = kvConnStr + ";SSL Mode=Disable";
        
        Console.WriteLine($"✅ Converted to: {kvConnStr.Replace(password, "***")}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"⚠️ URI parsing failed: {ex.Message}");
    }
}
else if (connectionString.Contains("Host="))
{
    Console.WriteLine("📝 Detected key-value format");
    testConnections["Original Key-Value"] = connectionString;
}
else
{
    Console.WriteLine("⚠️ Unknown connection string format!");
}

// Add hardcoded fallback
testConnections["Hardcoded Fallback"] = "Host=dpg-d4nsli9r0fns73dirvf0-a.oregon-postgres.render.com;Port=5432;Database=lms_kuuo;Username=lms_kuuo_user;Password=9SHBC4OHLC2jVz0HRbFzqoqfhjU30TJ4;SSL Mode=Require;Trust Server Certificate=true";

Console.WriteLine("\n🔍 TESTING CONNECTION VARIATIONS:");

string? workingConnection = null;
string? workingMethod = null;

foreach (var (name, testConn) in testConnections)
{
    Console.WriteLine($"\n📡 Testing: {name}");
    Console.WriteLine($"   Connection: {MaskPassword(testConn)}");
    
    try
    {
        using var conn = new NpgsqlConnection(testConn);
        
        Console.WriteLine("   Opening connection...");
        await conn.OpenAsync();
        
        Console.WriteLine("   ✅ Connected! Checking server...");
        
        using var cmd = new NpgsqlCommand("SELECT version();", conn);
        var version = await cmd.ExecuteScalarAsync();
        
        Console.WriteLine($"   ✅ PostgreSQL: ");
        
        workingConnection = testConn;
        workingMethod = name;
        Console.WriteLine($"\n✅✅✅ SUCCESS WITH: {name} ✅✅✅");
        break;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"   ❌ {ex.GetType().Name}: {ex.Message}");
        if (ex.InnerException != null)
        {
            Console.WriteLine($"      Inner: {ex.InnerException.Message}");
        }
    }
}

if (workingConnection == null)
{
    Console.WriteLine("\n❌❌❌ ALL CONNECTION ATTEMPTS FAILED! ❌❌❌");
    Console.WriteLine("\n🔧 RECOMMENDED FIX:");
    Console.WriteLine("In Render Dashboard → Your Web Service → Environment:");
    Console.WriteLine("Set ConnectionStrings__DefaultConnection to:");
    Console.WriteLine("Host=dpg-d4nsli9r0fns73dirvf0-a.oregon-postgres.render.com;Port=5432;Database=lms_kuuo;Username=lms_kuuo_user;Password=9SHBC4OHLC2jVz0HRbFzqoqfhjU30TJ4;SSL Mode=Require;Trust Server Certificate=true");
    
    // Use hardcoded fallback
    workingConnection = testConnections["Hardcoded Fallback"];
    Console.WriteLine("\n⚠️ Using hardcoded fallback connection string");
}
else
{
    Console.WriteLine($"\n✅ Will use: {workingMethod}");
}

connectionString = workingConnection;

// Configure services
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped(typeof(IUserRepository<>), typeof(UserRepository<>));
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBorrowRepository, BorrowRepository>();
builder.Services.AddScoped<IQueueRepository, QueueRepository>();
builder.Services.AddScoped<IMembershipRepository, MembershipRepository>();

var app = builder.Build();

Console.WriteLine("\n✅ App built, testing database with EF Core...");

// Database setup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    try
    {
        Console.WriteLine("🔄 EF Core: Testing connection...");
        var canConnect = await context.Database.CanConnectAsync();
        
        if (canConnect)
        {
            Console.WriteLine("✅ EF Core: Connected successfully!");
            
            var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
            var pendingList = pendingMigrations.ToList();
            
            if (pendingList.Any())
            {
                Console.WriteLine($"📋 Applying {pendingList.Count} migration(s)...");
                await context.Database.MigrateAsync();
                Console.WriteLine("✅ Migrations applied!");
            }
            else
            {
                Console.WriteLine("✅ No pending migrations");
            }
            
            Console.WriteLine("🌱 Seeding database...");
            await DbSeeder.SeedDatabase(context);
            Console.WriteLine("✅ Database ready!");
        }
        else
        {
            Console.WriteLine("❌ EF Core: Connection failed");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"\n❌ DATABASE ERROR: {ex.Message}");
        
        if (ex.Message.Contains("already exists"))
        {
            Console.WriteLine("⚠️ Tables exist, seeding...");
            try { await DbSeeder.SeedDatabase(context); } catch { }
        }
    }
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => Results.Json(new
{
    status = "running",
    message = "Library Management System API",
    version = "1.0.0",
    timestamp = DateTime.UtcNow
}));

app.MapGet("/health", async (ApplicationDbContext context) =>
{
    try
    {
        var canConnect = await context.Database.CanConnectAsync();
        return Results.Json(new
        {
            status = canConnect ? "healthy" : "unhealthy",
            connected = canConnect,
            timestamp = DateTime.UtcNow
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

Console.WriteLine("\n✅✅✅ APPLICATION STARTED ✅✅✅\n");

app.Run();

static string MaskPassword(string connStr)
{
    try
    {
        if (connStr.Contains("Password="))
        {
            var parts = connStr.Split(';');
            return string.Join(";", parts.Select(p => 
                p.Trim().StartsWith("Password=", StringComparison.OrdinalIgnoreCase) 
                    ? "Password=***" 
                    : p));
        }
        if (connStr.Contains("://"))
        {
            var parts = connStr.Split('@');
            if (parts.Length > 1)
            {
                var userPart = parts[0].Split(':');
                return $"{userPart[0]}:{userPart[1]}:***@{parts[1]}";
            }
        }
        return connStr;
    }
    catch
    {
        return "***MASKED***";
    }
}