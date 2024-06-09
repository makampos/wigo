using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Wigo.Domain.Entities;
using Wigo.Infrastructure.Interfaces;
using Wigo.Service.Commands;
using Wigo.Service.Handlers;

namespace Wigo.Tests.UnitTests.Handlers;

public class AddUserCommandHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly AddUserCommandHandler _handler;

    public AddUserCommandHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _handler = new AddUserCommandHandler(_userRepository);
    }

    [Fact]
    public async Task Handle_Should_Return_SuccessResult_When_User_Is_Created()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new AddUserCommand(Faker.Name.FullName());

        _userRepository.AddUserAsync(Arg.Any<User>()).Returns(userId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().Be(userId);
    }

    [Fact]
    public async Task Handle_Should_Return_FailureResult_When_User_Creation_Fails()
    {
        // Arrange
        var command = new AddUserCommand(Faker.Name.FullName());

        _userRepository.AddUserAsync(Arg.Any<User>()).Throws(new Exception("Failed to create user"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Failure.Should().BeTrue();
        result.ErrorMessage.Should().Be("Failed to create user: Failed to create user");
    }
}