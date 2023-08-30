using FluentAssertions;
using Gatekeeper.Core.Policies;
using Gatekeeper.Core.Repositories;
using Moq;

namespace Gatekeeper.Core.Test.Policies;

public class UnitIdentifierDuplicatedPolicyTest
{
    private Mock<IUnitRepository> _unitRepositoryMock = null!;

    [SetUp]
    public void BeforeEach()
    {
        _unitRepositoryMock = new Mock<IUnitRepository>();
    }

    [Test]
    public async Task ShouldReturnTrueWhenIdentifierDoesNotExists()
    {
        var policy = new UnitIdentifierDuplicatedPolicy(_unitRepositoryMock.Object);

        _unitRepositoryMock.Setup(r => r.ExistsIdentifierAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var result = await policy.IsValidAsync("identifier", CancellationToken.None);

        result.Should().BeTrue();

        _unitRepositoryMock.Verify(r => r.ExistsIdentifierAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
    [Test]
    public async Task ShouldReturnFalseWhenIdentifierExists()
    {
        var policy = new UnitIdentifierDuplicatedPolicy(_unitRepositoryMock.Object);

        _unitRepositoryMock.Setup(r => r.ExistsIdentifierAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await policy.IsValidAsync("identifier", CancellationToken.None);

        result.Should().BeFalse();

        _unitRepositoryMock.Verify(r => r.ExistsIdentifierAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}