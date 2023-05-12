using FluentAssertions;
using ResourceManager.Domain.Resources.Exceptions;
using Xunit;

namespace ResourceManager.Domain.Resources.UnitTests;

public class ResourceTests
{

    [Fact]
    public void CreateNew_ShouldCreateNewAggregateWithValidState()
    {
        // Arrange & Act
        Resource resource = Resource.CreateNew(Constants.ResourceId, Constants.ResourceName);

        // Assert
        resource.Id.Should().Be(Constants.ResourceId);
        resource.Name.Should().Be(Constants.ResourceName);
        resource.IsWithdrawn.Should().BeFalse();
        resource.IsLockedAtTheMoment(Constants.CurrentDate).Should().BeFalse();
    }

    [Fact]
    public void LockPermanently_ShouldMakeLockOnResource()
    {
        // Arrange
        Resource resource = Resource.CreateNew(Constants.ResourceId, Constants.ResourceName);

        // Act
        resource.LockPermanently(Constants.Username, Constants.CurrentDate);

        // Assert
        resource.Id.Should().Be(Constants.ResourceId);
        resource.Name.Should().Be(Constants.ResourceName);
        resource.IsWithdrawn.Should().BeFalse();
        resource.IsLockedAtTheMoment(Constants.CurrentDate).Should().BeTrue();
    }

    [Fact]
    public void LockTemporary_ShouldMakeLockOnResource()
    {
        // Arrange
        Resource resource = Resource.CreateNew(Constants.ResourceId, Constants.ResourceName);
        DateTimeOffset lockingDate = Constants.CurrentDate.AddDays(1);

        // Act
        resource.LockTemporary(Constants.Username, Constants.CurrentDate, lockingDate);

        // Assert
        resource.Id.Should().Be(Constants.ResourceId);
        resource.Name.Should().Be(Constants.ResourceName);
        resource.IsWithdrawn.Should().BeFalse();
        resource.IsLockedAtTheMoment(Constants.CurrentDate).Should().BeTrue();
    }

    [Fact]
    public void LockTemporary_ShouldThrown_WhenLockingDateIsInvalid()
    {
        // Arrange
        Resource resource = Resource.CreateNew(Constants.ResourceId, Constants.ResourceName);
        DateTimeOffset lockingDateTheSameAsCurrentDate = Constants.CurrentDate;
        DateTimeOffset lockingDateLowerThanCurrentDate = Constants.CurrentDate.AddHours(-1);

        // Act & Assert
        Assert.Throws<LockingDateArgumentException>(
            () => resource.LockTemporary(Constants.Username, Constants.CurrentDate, lockingDateTheSameAsCurrentDate));
        Assert.Throws<LockingDateArgumentException>(
            () => resource.LockTemporary(Constants.Username, Constants.CurrentDate, lockingDateLowerThanCurrentDate));
    }
    
    [Fact(DisplayName = "1. Nie można zablokować Zasobu, jeżeli jest wycofany")]
    public void LockTemporary_ShouldThrown_WhenResourceIsWithdrawnAlready()
    {
        // Arrange
        Resource resource = Resource.CreateNew(Constants.ResourceId, Constants.ResourceName);
        DateTimeOffset lockingDate = Constants.CurrentDate.AddHours(1);
        resource.Withdraw();

        // Act & Assert
        Assert.Throws<ResourceIsWithdrawnException>(
            () => resource.LockTemporary(Constants.Username, Constants.CurrentDate, lockingDate));
    }
    
    [Fact(DisplayName = "2. Nie można zablokować Zasobu, jeżeli jest zablokowany przez innego użytkownika")]
    public void LockTemporary_ShouldThrown_WhenResourceIsLockedByAnotherUser()
    {
        // Arrange
        Resource resource = Resource.CreateNew(Constants.ResourceId, Constants.ResourceName);
        DateTimeOffset lockingDate = Constants.CurrentDate.AddHours(1);
        resource.LockTemporary(Constants.Username, Constants.CurrentDate, lockingDate);

        // Act & Assert
        Assert.Throws<ResourceLockedByAnotherUserException>(
            () => resource.LockTemporary("elza", Constants.CurrentDate, lockingDate));
    }
    
    [Fact]
    public void LockTemporary_ShouldThrown_WhenRequestedLockingDateIsWeakerThanCurrentOne()
    {
        // Arrange
        Resource resource = Resource.CreateNew(Constants.ResourceId, Constants.ResourceName);
        DateTimeOffset lockingDate = Constants.CurrentDate.AddHours(2);
        resource.LockTemporary(Constants.Username, Constants.CurrentDate, lockingDate);

        // Act & Assert
        Assert.Throws<LockingDateWeakerThanCurrentLockException>(
            () => resource.LockTemporary(Constants.Username, Constants.CurrentDate, lockingDate));
        Assert.Throws<LockingDateWeakerThanCurrentLockException>(
            () => resource.LockTemporary(Constants.Username, Constants.CurrentDate, lockingDate.AddHours(-1)));
    }
    
    [Fact]
    public void LockTemporary_ShouldCreateANewLock_WhenExistedLockIsAWeaker()
    {
        // Arrange
        Resource resource = Resource.CreateNew(Constants.ResourceId, Constants.ResourceName);
        DateTimeOffset lockingDate = Constants.CurrentDate.AddHours(2);
        resource.LockTemporary(Constants.Username, Constants.CurrentDate, lockingDate);

        // Act
        DateTimeOffset newLockingDate = lockingDate.AddHours(2);
        resource.LockTemporary(Constants.Username, Constants.CurrentDate, newLockingDate);
        
        // Assert
        resource.Id.Should().Be(Constants.ResourceId);
        resource.Name.Should().Be(Constants.ResourceName);
        resource.IsWithdrawn.Should().BeFalse();
        resource.IsLockedAtTheMoment(Constants.CurrentDate).Should().BeTrue();
        resource.LockedTo(Constants.CurrentDate).Should().Be(newLockingDate);
    }

    [Fact]
    public void Unlock_ShouldChangeAggregateStateProperly_WhenThereIsSomeActiveLock()
    {
        // Arrange
        Resource resource = Resource.CreateNew(Constants.ResourceId, Constants.ResourceName);
        DateTimeOffset lockingDate = Constants.CurrentDate.AddDays(1);
        resource.LockTemporary(Constants.Username, Constants.CurrentDate, lockingDate);

        // Act
        resource.Unlock(Constants.Username, Constants.CurrentDate.AddHours(1));

        // Assert
        resource.Id.Should().Be(Constants.ResourceId);
        resource.Name.Should().Be(Constants.ResourceName);
        resource.IsWithdrawn.Should().BeFalse();
        resource.IsLockedAtTheMoment(Constants.CurrentDate).Should().BeFalse();
    }

    [Fact]
    public void Unlock_ShouldThrown_WhenThereIsNoLockAtTheMoment()
    {
        // Arrange
        Resource resource = Resource.CreateNew(Constants.ResourceId, Constants.ResourceName);

        // Act
        Assert.Throws<ResourceNotLockedException>(() =>
            resource.Unlock(Constants.Username, Constants.CurrentDate.AddHours(1)));
    }

    [Fact(DisplayName = "3. Nie można odblokować Zasobu, jeżeli jest zablokowany przez innego użytkownika")]
    public void Unlock_ShouldThrown_WhenThereIsSomeActiveLockByOtherUser()
    {
        // Arrange
        Resource resource = Resource.CreateNew(Constants.ResourceId, Constants.ResourceName);
        DateTimeOffset lockingDate = Constants.CurrentDate.AddDays(1);
        resource.LockTemporary(Constants.Username, Constants.CurrentDate, lockingDate);

        // Act
        Assert.Throws<ResourceLockedByAnotherUserException>(() =>
            resource.Unlock("elza", Constants.CurrentDate.AddHours(1)));
    }
   
    [Fact]
    public void Unlock_ShouldThrown_WhenResourceIsWithdrawn()
    {
        // Arrange
        Resource resource = Resource.CreateNew(Constants.ResourceId, Constants.ResourceName);
        DateTimeOffset lockingDate = Constants.CurrentDate.AddDays(1);
        resource.LockTemporary(Constants.Username, Constants.CurrentDate, lockingDate);
        resource.Withdraw();

        // Act
        Assert.Throws<ResourceIsWithdrawnException>(
            () => resource.Unlock(Constants.Username, Constants.CurrentDate));
    } 

    private static class Constants
    {
        public static readonly string Username = "anna";
        public static readonly Guid ResourceId = Guid.Parse("9f455590-695c-4743-8f76-7bddf16b3fb7");
        public static readonly string ResourceName = "my-resource-#01";
        public static readonly DateTimeOffset CurrentDate = DateTimeOffset.Parse("2023-05-12T12:00:00+02:00");
    }
}
