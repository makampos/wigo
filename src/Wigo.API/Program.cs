using MediatR;
using Microsoft.EntityFrameworkCore;
using Wigo.Domain.Interfaces;
using Wigo.Infrastructure.Data;
using Wigo.Infrastructure.Interfaces;
using Wigo.Infrastructure.Repositories;
using Wigo.Service.Handlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBeneficiaryRepository, BeneficiaryRepository>();

builder.Services.AddMediatR(typeof(Program).Assembly,
    typeof(AddUserCommandHandler).Assembly,
    typeof(AddBeneficiaryCommandHandler).Assembly,
    typeof(GetBeneficiariesByUserIdQueryHandler).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();
app.Run();

