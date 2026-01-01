using WebApp.Domain.ValueObjects;

namespace WebApp.Tests.Unit.Domain.ValueObjects;

public class DenominationValueObjectTests
{
    #region Creation / Invariants

    [Fact]
    public void Denomination_Create_WhenValueIsValid_CreatesSuccessfully()
    {
        // Arrange
        var value = 1.00m;

        // Act
        var result = new Denomination(value);

        // Assert
        Assert.Equal(1.00m, result.Value);
    }

    [Fact]
    public void Denomination_Create_WhenValueIsZero_ThrowsArgumentException()
    {
        // Arrange
        var value = 0.00m;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Denomination(value));
    }

    [Fact]
    public void Denomination_Create_WhenValueIsNegative_ThrowsArgumentException()
    {
        // Arrange
        var value = -5.00m;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Denomination(value));
    }

    [Fact]
    public void Denomination_Create_WhenValueIsNotAllowed_ThrowsArgumentException()
    {
        // Arrange
        var value = 3.00m;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Denomination(value));
    }

    [Fact]
    public void Denomination_Create_WhenValueHasMoreThanTwoDecimalPlaces_ThrowsArgumentException()
    {
        // Arrange
        var value = 10.666m;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Denomination(value));
    }

    [Fact]
    public void Denomination_Create_WhenValueIsOneCent_IsAllowed()
    {
        // Arrange
        var value = 0.01m;

        // Act
        var result = new Denomination(value);

        // Assert
        Assert.Equal(0.01m, result.Value);
    }

    [Fact]
    public void Denomination_Create_WhenValueIsOneReal_IsAllowed()
    {
        // Arrange
        var value = 1.00m;

        // Act
        var result = new Denomination(value);

        // Assert
        Assert.Equal(1.00m, result.Value);
    }

    [Fact]
    public void Denomination_Create_WhenValueIsTenReais_IsAllowed()
    {
        // Arrange
        var value = 10.00m;

        // Act
        var result = new Denomination(value);

        // Assert
        Assert.Equal(10.00m, result.Value);
    }

    [Fact]
    public void Denomination_Create_WhenValueIsTwoHundredReais_IsAllowed()
    {
        // Arrange
        var value = 200.00m;

        // Act
        var result = new Denomination(value);

        // Assert
        Assert.Equal(200.00m, result.Value);
    }

    #endregion Creation / Invariants

    #region Comparison

    [Fact]
    public void Denomination_CompareTo_WhenValuesAreEqual_ReturnsZero()
    {
        // Arrange
        var denomination1 = new Denomination(50.00m);
        var denomination2 = new Denomination(50.00m);

        // Act
        var result = denomination1.CompareTo(denomination2);

        // Assert
        Assert.Equal(0, result);
    }
    
    [Fact]
    public void Denomination_CompareTo_WhenLeftIsGreater_ReturnsPositiveNumber()
    {
        // Arrange
        var denomination1 = new Denomination(100.00m);
        var denomination2 = new Denomination(50.00m);
        
        // Act
        var result = denomination1.CompareTo(denomination2);
        
        // Assert
        Assert.True(result > 0);
    }

    [Fact]
    public void Denomination_CompareTo_WhenLeftIsSmaller_ReturnsNegativeNumber()
    {
        // Arrange
        var denomination1 = new Denomination(10.00m);
        var denomination2 = new Denomination(20.00m);
        
        // Act
        var result = denomination1.CompareTo(denomination2);
        
        // Assert
        Assert.True(result < 0);
    }

    [Fact]
    public void Denomination_CompareTo_WhenOtherIsNull_ReturnsPositiveNumber()
    {
        // Arrange
        var denomination1 = new Denomination(10.00m);
        var denomination2 = (Denomination?)null;

        // Act
        var result = denomination1.CompareTo(denomination2);

        // Assert
        Assert.True(result > 0);
    }

    #endregion Comparison

    #region Equals

    [Fact]
    public void Denomination_Equals_WhenValuesAreEqual_ReturnsTrue()
    {
        // Arrange
        var denomination1 = new Denomination(50.00m);
        var denomination2 = new Denomination(50.00m);

        // Act
        var result = denomination1.Equals(denomination2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Denomination_Equals_WhenValuesAreDifferent_ReturnsFalse()
    {
        // Arrange
        var denomination1 = new Denomination(20.00m);
        var denomination2 = new Denomination(10.00m);

        // Act
        var result = denomination1.Equals(denomination2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Denomination_Equals_WhenOtherIsNull_ReturnsFalse()
    {
        // Arrange
        var denomination1 = new Denomination(20.00m);
        var denomination2 = (Denomination?)null;

        // Act
        var result = denomination1.Equals(denomination2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Denomination_Equals_ObjectOverride_WhenValuesAreEqual_ReturnsTrue()
    {
        // Arrange
        object denomination1 = new Denomination(100.00m);
        object denomination2 = new Denomination(100.00m);

        // Act
        var result = denomination1.Equals(denomination2);

        // Assert
        Assert.True(result);
    }

    #endregion Equals

    #region HashSet / GetHashCode

    [Fact]
    public void Denomination_WhenAddedToHashSet_DoesNotAllowDuplicatesWithSameValue()
    {
        // Arrange
        var denomination1 = new Denomination(20.00m);
        var denomination2 = new Denomination(50.00m);
        var denomination3 = new Denomination(20.00m);

        var hashSet = new HashSet<Denomination>();

        // Act
        hashSet.Add(denomination1);
        hashSet.Add(denomination2);
        hashSet.Add(denomination3);

        // Assert
        Assert.Equal(2, hashSet.Count);
        Assert.Contains(denomination1, hashSet);
        Assert.Contains(denomination2, hashSet);
    }

    [Fact]
    public void Denomination_GetHashCode_WhenValuesAreEqual_ReturnsSameHashCode()
    {
        // Arrange
        var denomination1 = new Denomination(100.00m);
        var denomination2 = new Denomination(100.00m);

        // Act & Assert
        Assert.Equal(denomination1.GetHashCode(), denomination2.GetHashCode());
    }

    #endregion HashSet / GetHashCode
}