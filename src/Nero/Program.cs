using Microsoft.EntityFrameworkCore;
using Nero.Data;
using Nero.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<NeroDbContext>(options =>
    options.UseSqlite("Data Source=NeroData.db"));

builder.Services.AddScoped<IBalanceService, BalanceService>();

var app = builder.Build();

// Seed initial data
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<NeroDbContext>();
    dbContext.Database.EnsureCreated(); // Ensure the database is created
    dbContext.SeedData(); // Seed initial data
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();