using FluentAssertions;
using NSubstitute;
using Wigo.Domain.Entities;
using Wigo.Domain.Interfaces;
using Wigo.Service.Handlers;
using Wigo.Service.Queries;

namespace Wigo.Tests.UnitTests.Handlers;

public class GetBeneficiariesByUserIdQueryHandlerTests
{
    private readonly IBeneficiaryRepository _beneficiaryRepository;
    private readonly GetBeneficiariesByUserIdQueryHandler _handler;

    public GetBeneficiariesByUserIdQueryHandlerTests()
    {
        _beneficiaryRepository = Substitute.For<IBeneficiaryRepository>();
        _handler = new GetBeneficiariesByUserIdQueryHandler(_beneficiaryRepository);
    }

    [Fact]
    public async Task Handle_Should_Return_SuccessResult_When_Beneficiaries_Are_Found()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var beneficiaryId = Guid.NewGuid();
        var beneficiaries = new List<Beneficiary>
        {
            Beneficiary.Create(userId, Faker.Name.FullName(), Faker.Phone.Number()) with { Id = beneficiaryId }
        };
        var query = new GetBeneficiariesByUserIdQuery(userId);

        _beneficiaryRepository.GetBeneficiariesByUserIdAsync(userId).Returns(beneficiaries);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeEmpty();
        result.Data.Should().HaveCount(1);
        result.Data.First().BeneficiaryId.Should().Be(beneficiaryId);
        result.Data.First().Nickname.Should().Be(beneficiaries.First().Nickname);
        result.Data.First().PhoneNumber.Should().Be(beneficiaries.First().PhoneNumber);
    }

    [Fact]
    public async Task Handle_Should_Return_SuccessResult_When_No_Beneficiaries_Are_Found()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var beneficiaries = Enumerable.Empty<Beneficiary>();
        var query = new GetBeneficiariesByUserIdQuery(userId);

        _beneficiaryRepository.GetBeneficiariesByUserIdAsync(userId).Returns(beneficiaries);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().BeEmpty();
    }
}