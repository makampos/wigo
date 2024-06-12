using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nero.Data;
using Nero.Entities;
using Nero.Requests;
using Nero.Services;
using Newtonsoft.Json;

namespace Nero.Tests.Controllers;

public class BalanceControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public BalanceControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddDbContext<NeroDbContext>(options =>
                {
                    options.UseSqlite("DataSource=:memory:");
                });

                services.AddScoped<IAccountBalanceService, AccountBalanceService>();

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<NeroDbContext>();
                    db.Database.OpenConnection();
                    db.Database.EnsureCreated();
                }
            });
        });

        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task CreateBalanceAccountAsync_ShouldReturnUserAccountBalanceNumber()
    {
        // Arrange
        var request = new CreateAccountBalanceRequest(Guid.NewGuid(), Faker.Name.Middle());

        // Act
        var response = await _client.PostAsJsonAsync("/api/account-balance/create", request);
        response.EnsureSuccessStatusCode();

        var userAccountBalanceNumber = await response.Content.ReadAsStringAsync();

        // Assert
        userAccountBalanceNumber.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetBalance_ShouldReturnBalance()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var accountBalance = await _client.PostAsJsonAsync("/api/account-balance/create", new CreateAccountBalanceRequest(userId, Faker.Name.Middle()));
        accountBalance.EnsureSuccessStatusCode();

        var userAccountBalanceNumber = await accountBalance.Content.ReadAsStringAsync();

        // Act
        var response = await _client.GetAsync($"/api/account-balance/{userId}/{userAccountBalanceNumber}");
        response.EnsureSuccessStatusCode();

        var balanceDTO = JsonConvert.DeserializeObject<AccountBalanceDTO>(await response.Content.ReadAsStringAsync());

        // Assert
        balanceDTO.Should().NotBeNull();
        balanceDTO.Amount.Should().Be(0,"because the account balance is new");
    }

    [Fact]
    public async Task DebitBalance_ShouldReturnSuccess()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var accountBalance = await _client.PostAsJsonAsync("/api/account-balance/create", new CreateAccountBalanceRequest(userId, Faker.Name.Middle()));
        accountBalance.EnsureSuccessStatusCode();

        var userAccountBalanceNumber = await accountBalance.Content.ReadAsStringAsync();

        var creditResponse = await _client.PutAsJsonAsync("/api/account-balance/credit", new CreateCreditRequest(userId, userAccountBalanceNumber, 100));
        creditResponse.EnsureSuccessStatusCode();

        // Act
        var response = await _client.PutAsJsonAsync("/api/account-balance/debit", new CreateDebitRequest(userId, userAccountBalanceNumber, 100));
        response.EnsureSuccessStatusCode();

        var success = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());

        // Assert
        success.Should().BeTrue();
    }

    [Fact]
    public async Task DebitBalance_ShouldReturnBadRequest_WhenInsufficientBalance()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var accountBalance = await _client.PostAsJsonAsync("/api/account-balance/create", new CreateAccountBalanceRequest(userId, Faker.Name.Middle()));
        accountBalance.EnsureSuccessStatusCode();

        var userAccountBalanceNumber = await accountBalance.Content.ReadAsStringAsync();

        // Act
        var response = await _client.PutAsJsonAsync("/api/account-balance/debit", new CreateDebitRequest(userId, userAccountBalanceNumber, 100));

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        var errorMessage = await response.Content.ReadAsStringAsync();
        errorMessage.Should().Be("Insufficient balance.");
    }

    [Fact]
    public async Task CreditBalance_ShouldReturnSuccess()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var accountBalance = await _client.PostAsJsonAsync("/api/account-balance/create", new CreateAccountBalanceRequest(userId, Faker.Name.Middle()));
        accountBalance.EnsureSuccessStatusCode();

        var userAccountBalanceNumber = await accountBalance.Content.ReadAsStringAsync();

        // Act
        var response = await _client.PutAsJsonAsync("/api/account-balance/credit", new CreateCreditRequest(userId, userAccountBalanceNumber, 100));
        response.EnsureSuccessStatusCode();

        var success = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());

        // Assert
        success.Should().BeTrue();
    }

    [Fact]
    public async Task CreditBalance_ShouldReturnBadRequest_WhenFailedToCreditBalance()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var accountBalance = await _client.PostAsJsonAsync("/api/account-balance/create", new CreateAccountBalanceRequest(userId, Faker.Name.Middle()));
        accountBalance.EnsureSuccessStatusCode();

        var userAccountBalanceNumber = await accountBalance.Content.ReadAsStringAsync();

        // Act
        var response = await _client.PutAsJsonAsync("/api/account-balance/credit", new CreateCreditRequest(Guid.NewGuid(), userAccountBalanceNumber, 100));

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        var errorMessage = await response.Content.ReadAsStringAsync();
        errorMessage.Should().Be("Failed to credit balance.");
    }

}