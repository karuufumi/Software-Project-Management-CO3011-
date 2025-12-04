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

// Parse and show connection details
try
{
    var builder2 = new NpgsqlConnectionStringBuilder(connectionString);
    Console.WriteLine("\n🔍 PARSED CONNECTION DETAILS:");
    Console.WriteLine($"  Protocol: postgresql://");
    Console.WriteLine($"  Host: {builder2.Host}");
    Console.WriteLine($"  Port: {builder2.Port}");
    Console.WriteLine($"  Database: {builder2.Database}");
    Console.WriteLine($"  Username: {builder2.Username}");
    Console.WriteLine($"  Password: {(string.IsNullOrEmpty(builder2.Password) ? "NOT SET" : "***SET***")}");
    Console.WriteLine($"  SSL Mode: {builder2.SslMode}");
    Console.WriteLine($"  Timeout: {builder2.Timeout}s");
    Console.WriteLine($"  Command Timeout: {builder2.CommandTimeout}s");
}
catch (Exception ex)
{
    Console.WriteLine($"⚠️ Could not parse: {ex.Message}");
}

// Test different SSL configurations
Console.WriteLine("\n🔍 TESTING CONNECTION VARIATIONS:");

var testConnections = new Dictionary<string, string>
{
    ["Original"] = connectionString,
    ["With SSL Require"] = AddOrUpdateQueryParam(connectionString, "sslmode", "require"),
    ["With SSL Prefer"] = AddOrUpdateQueryParam(connectionString, "sslmode", "prefer"),
    ["With SSL Disable"] = AddOrUpdateQueryParam(connectionString, "sslmode", "disable"),
    ["SSL Require + Trust Cert"] = AddOrUpdateQueryParam(AddOrUpdateQueryParam(connectionString, "sslmode", "require"), "Trust Server Certificate", "true")
};

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
        
        Console.WriteLine("   ✅ Connected! Checking server version...");
        
        using var cmd = new NpgsqlCommand("SELECT version();", conn);
        var version = await cmd.ExecuteScalarAsync();
        
        Console.WriteLine($"   ✅ PostgreSQL: {version?.ToString()?.Substring(0, Math.Min(60, version.ToString().Length))}");
        
        workingConnection = testConn;
        workingMethod = name;
        Console.WriteLine($"\n✅✅✅ SUCCESS WITH: {name} ✅✅✅");
        break;
    }
    catch (PostgresException pgEx)
    {
        Console.WriteLine($"   ❌ PostgreSQL Error: {pgEx.Message}");
        Console.WriteLine($"      SqlState: {pgEx.SqlState}");
        Console.WriteLine($"      Severity: {pgEx.Severity}");
    }
    catch (NpgsqlException npgEx)
    {
        Console.WriteLine($"   ❌ Npgsql Error: {npgEx.Message}");
        if (npgEx.InnerException != null)
        {
            Console.WriteLine($"      Inner: {npgEx.InnerException.Message}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"   ❌ Error: {ex.GetType().Name}: {ex.Message}");
        if (ex.InnerException != null)
        {
            Console.WriteLine($"      Inner: {ex.InnerException.Message}");
        }
    }
}

if (workingConnection == null)
{
    Console.WriteLine("\n❌❌❌ ALL CONNECTION ATTEMPTS FAILED! ❌❌❌");
    Console.WriteLine("\nPossible issues:");
    Console.WriteLine("1. Database is not 'Available' in Render dashboard");
    Console.WriteLine("2. Wrong database credentials");
    Console.WriteLine("3. Network connectivity issue");
    Console.WriteLine("4. Database hostname is incorrect");
    Console.WriteLine("5. Firewall blocking connection");
    
    // Use original anyway to see EF error
    workingConnection = connectionString;
}
else
{
    Console.WriteLine($"\n✅ Will use working connection method: {workingMethod}");
    connectionString = workingConnection;
}

// Configure DbContext
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
        Console.WriteLine("🔄 EF Core: CanConnectAsync()...");
        var canConnect = await context.Database.CanConnectAsync();
        
        if (canConnect)
        {
            Console.WriteLine("✅ EF Core: Connected!");
            
            var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
            var pendingList = pendingMigrations.ToList();
            
            if (pendingList.Any())
            {
                Console.WriteLine($"📋 Found {pendingList.Count} pending migration(s):");
                foreach (var m in pendingList)
                {
                    Console.WriteLine($"   - {m}");
                }
                
                Console.WriteLine("🔄 Applying migrations...");
                await context.Database.MigrateAsync();
                Console.WriteLine("✅ Migrations applied!");
            }
            else
            {
                Console.WriteLine("✅ No pending migrations");
            }
            
            Console.WriteLine("🌱 Seeding database...");
            await DbSeeder.SeedDatabase(context);
            Console.WriteLine("✅ Database setup complete!");
        }
        else
        {
            Console.WriteLine("❌ EF Core: CanConnectAsync() returned false");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"\n❌ EF CORE ERROR:");
        Console.WriteLine($"   Type: {ex.GetType().Name}");
        Console.WriteLine($"   Message: {ex.Message}");
        
        if (ex.InnerException != null)
        {
            Console.WriteLine($"   Inner Type: {ex.InnerException.GetType().Name}");
            Console.WriteLine($"   Inner Message: {ex.InnerException.Message}");
        }
        
        if (ex.Message.Contains("already exists"))
        {
            Console.WriteLine("\n⚠️ Tables already exist, attempting seed...");
            try
            {
                await DbSeeder.SeedDatabase(context);
                Console.WriteLine("✅ Seeding succeeded");
            }
            catch (Exception seedEx)
            {
                Console.WriteLine($"⚠️ Seeding failed: {seedEx.Message}");
            }
        }
        else
        {
            Console.WriteLine("\n⚠️ Continuing despite database error...");
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
            database = "PostgreSQL",
            connected = canConnect,
            timestamp = DateTime.UtcNow
        });
    }
    catch (Exception ex)
    {
        return Results.Json(new
        {
            status = "unhealthy",
            database = "PostgreSQL",
            connected = false,
            error = ex.Message,
            timestamp = DateTime.UtcNow
        });
    }
});

Console.WriteLine("\n✅✅✅ APPLICATION STARTED ✅✅✅\n");

app.Run();

// Helper functions
static string AddOrUpdateQueryParam(string connStr, string key, string value)
{
    try
    {
        var builder = new NpgsqlConnectionStringBuilder(connStr);
        
        if (key.Equals("sslmode", StringComparison.OrdinalIgnoreCase))
        {
            builder.SslMode = Enum.Parse<SslMode>(value, true);
        }
                
        return builder.ConnectionString;
    }
    catch
    {
        // Fallback to simple query string append
        var separator = connStr.Contains("?") ? "&" : "?";
        return $"{connStr}{separator}{key}={value}";
    }
}

static string MaskPassword(string connStr)
{
    try
    {
        if (connStr.Contains("://"))
        {
            var uri = new Uri(connStr.Split('?')[0]);
            var queryString = connStr.Contains("?") ? "?" + connStr.Split('?')[1] : "";
            return $"postgresql://{uri.UserInfo.Split(':')[0]}:***@{uri.Host}{uri.AbsolutePath}{queryString}";
        }
        return connStr;
    }
    catch
    {
        return "***MASKED***";
    }
}