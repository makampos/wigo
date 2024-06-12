using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Wigo.Domain.Interfaces;
using Wigo.Infrastructure.Data;
using Wigo.Infrastructure.Interfaces;
using Wigo.Infrastructure.Repositories;
using Wigo.Infrastructure.Services;
using Wigo.Service.Handlers;
using Wigo.Service.Validators;

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
builder.Services.AddScoped<ITopUpRepository, TopUpRepository>();
builder.Services.AddScoped<ITopUpTransactionRepository, TopUpTransactionRepository>();

builder.Services.AddMediatR(typeof(Program).Assembly,
    typeof(AddUserCommandHandler).Assembly,
    typeof(AddBeneficiaryCommandHandler).Assembly,
    typeof(GetBeneficiariesByUserIdQueryHandler).Assembly);

// Register ExternalBalanceService
var externalAccountBalanceServiceBaseUrl = builder.Configuration.GetSection("ExternalAccountBalanceService:BaseUrl").Value;
builder.Services.AddSingleton<IExternalAccountBalanceService>(new ExternalAccountBalanceService(externalAccountBalanceServiceBaseUrl));


// Register FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<AddBeneficiaryCommandValidator>();
builder.Services.AddFluentValidationAutoValidation();

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

