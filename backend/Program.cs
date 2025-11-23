// See https://aka.ms/new-console-template for more information

using backend.Data;
using backend.Repositories;
using backend.Repositories.BookRepository;
using backend.Repositories.UserQueueRepository;
using backend.Repositories.UserRepository;
using backend.Services;
using backend.Services.BookService;
using backend.Services.UserQueueService;
using backend.Services.UserService;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserQueueRepository, UserQueueRepository>();

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserQueueService, UserQueueService>();

builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });
}

// app.UseHttpsRedirection();

app.MapControllers();

app.MapGet("/", () => "Hello world !!!").WithName("Abcdef");

app.Run();