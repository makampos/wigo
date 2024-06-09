using FluentAssertions;
using NSubstitute;
using Wigo.Domain.Entities;
using Wigo.Domain.Interfaces;

namespace Wigo.Tests.UnitTests.Repositories;

public class BeneficiaryRepository
{
    private readonly IBeneficiaryRepository _beneficiaryRepository;

    public BeneficiaryRepository()
    {
        _beneficiaryRepository = Substitute.For<IBeneficiaryRepository>();
    }

    [Fact]
    public async Task CreateBeneficiaryAsync_Should_Create_Beneficiary()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var beneficiary = Beneficiary.Create(
            userId: userId,
            nickname: Faker.Name.Middle(),
            phoneNumber: Faker.Phone.Number());

        _beneficiaryRepository.GetBeneficiariesByUserIdAsync(beneficiary.UserId).Returns(new List<Beneficiary>() {beneficiary });

         await _beneficiaryRepository.AddBeneficiaryAsync(beneficiary);

        // Act
        var retrievedBeneficiary = await _beneficiaryRepository.GetBeneficiariesByUserIdAsync(beneficiary.UserId);

        // Assert
        retrievedBeneficiary.Should().NotBeNull();
        retrievedBeneficiary.Should().HaveCount(1);
        retrievedBeneficiary.First().UserId.Should().Be(beneficiary.UserId);
        retrievedBeneficiary.First().Nickname.Should().Be(beneficiary.Nickname);
        retrievedBeneficiary.First().PhoneNumber.Should().Be(beneficiary.PhoneNumber);
    }
}