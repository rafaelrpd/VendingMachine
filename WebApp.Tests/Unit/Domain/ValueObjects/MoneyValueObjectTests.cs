using WebApp.Domain.ValueObjects;

namespace WebApp.Tests.Unit.Domain.ValueObjects;

public class MoneyValueObjectTests
{
    #region Creation / Invariants

    [Fact]
    public void Money_Create_WhenValueIsZero_CreatesValidMoney()
    {
        // Arrange
        var value = 0.00m;

        // Act
        var money = new Money(value);

        // Assert
        Assert.Equal(0.00m, money.Value);
    }

    [Fact]
    public void Money_Create_WhenValueHasMoreThanTwoDecimalPlaces_ThrowsArgumentException()
    {
        // Arrange
        var value = 10.123m;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Money(value));
    }

    [Fact]
    public void Money_Create_WhenValueIsNegative_ThrowsArgumentException()
    {
        // Arrange
        var value = -1.00m;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Money(value));
    }

    [Fact]
    public void Money_Create_WhenValueHasExactlyTwoDecimalPlaces_CreatesValidMoney()
    {
        // Arrange
        var value = 10.99m;

        // Act
        var money = new Money(value);

        // Assert
        Assert.Equal(10.99m, money.Value);
    }

    #endregion Creation / Invariants

    #region Add

    [Fact]
    public void Money_Add_WhenBothValuesAreValid_ReturnsSum()
    {
        // Arrange
        var money1 = new Money(10.50m);
        var money2 = new Money(15.25m);

        // Act
        var result = money1.Add(money2);

        // Assert
        Assert.Equal(25.75m, result.Value);
    }

    [Fact]
    public void Money_Add_WhenAddingZero_ReturnsSameValue()
    {
        // Arrange
        var money1 = new Money(10.00m);
        var money2 = new Money(0.00m);

        // Act
        var result = money1.Add(money2);

        // Assert
        Assert.Equal(10.00m, result.Value);
    }

    [Fact]
    public void Money_Add_WhenResultHasTwoDecimalPlaces_PreservesPrecision()
    {
        // Arrange
        var money1 = new Money(10.10m);
        var money2 = new Money(0.25m);

        // Act
        var result = money1.Add(money2);

        // Assert
        Assert.Equal(10.35m, result.Value);
    }

    [Fact]
    public void Money_Add_WhenOtherIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var money1 = new Money(10.00m);
        var money2 = (Money?)null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => money1.Add(money2!));
    }

    #endregion Add

    #region Subtract

    [Fact]
    public void Money_Subtract_WhenResultIsPositive_ReturnsDifference()
    {
        // Arrange
        var money1 = new Money(20.00m);
        var money2 = new Money(5.50m);

        // Act
        var result = money1.Subtract(money2);

        // Assert
        Assert.Equal(14.50m, result.Value);
    }

    [Fact]
    public void Money_Subtract_WhenSubtractingSameValue_ReturnsZero()
    {
        // Arrange
        var money1 = new Money(10.00m);
        var money2 = new Money(10.00m);

        // Act
        var result = money1.Subtract(money2);

        // Assert
        Assert.Equal(0.00m, result.Value);
    }

    [Fact]
    public void Money_Subtract_WhenResultWouldBeNegative_ThrowsArgumentException()
    {
        // Arrange
        var money1 = new Money(10.00m);
        var money2 = new Money(15.00m);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => money1.Subtract(money2));
    }

    [Fact]
    public void Money_Subtract_WhenOtherIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var money1 = new Money(10.00m);
        var money2 = (Money?)null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => money1.Subtract(money2!));
    }

    #endregion Subtract

    #region Comparison

    [Fact]
    public void Money_CompareTo_WhenValuesAreEqual_ReturnsZero()
    {
        // Arrange
        var money1 = new Money(10.00m);
        var money2 = new Money(10.00m);

        // Act
        var result = money1.CompareTo(money2);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void Money_CompareTo_WhenLeftIsGreater_ReturnsPositiveNumber()
    {
        // Arrange
        var money1 = new Money(15.00m);
        var money2 = new Money(10.00m);

        // Act
        var result = money1.CompareTo(money2);

        // Assert
        Assert.True(result > 0);
    }

    [Fact]
    public void Money_CompareTo_WhenLeftIsSmaller_ReturnsNegativeNumber()
    {
        // Arrange
        var money1 = new Money(5.00m);
        var money2 = new Money(10.00m);

        // Act
        var result = money1.CompareTo(money2);

        // Assert
        Assert.True(result < 0);
    }

    [Fact]
    public void Money_CompareTo_WhenOtherIsNull_ReturnsPositiveNumber()
    {
        // Arrange
        var money1 = new Money(10.00m);
        var money2 = (Money?)null;

        // Act
        var result = money1.CompareTo(money2);

        // Assert
        Assert.True(result > 0);
    }

    #endregion Comparison

    #region Equals

    [Fact]
    public void Money_Equals_WhenValuesAreEqual_ReturnsTrue()
    {
        // Arrange
        var money1 = new Money(10.00m);
        var money2 = new Money(10.00m);

        // Act
        var result = money1.Equals(money2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Money_Equals_WhenValuesAreDifferent_ReturnsFalse()
    {
        // Arrange
        var money1 = new Money(10.00m);
        var money2 = new Money(15.00m);

        // Act
        var result = money1.Equals(money2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Money_Equals_WhenOtherIsNull_ReturnsFalse()
    {
        // Arrange
        var money1 = new Money(10.00m);
        var money2 = (Money?)null;

        // Act
        var result = money1.Equals(money2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Money_Equals_ObjectOverride_WhenValuesAreEqual_ReturnsTrue()
    {
        // Arrange
        object money1 = new Money(10.00m);
        object money2 = new Money(10.00m);

        // Act
        var result = money1.Equals(money2);

        // Assert
        Assert.True(result);
    }

    #endregion Equals

    #region HashSet / GetHashCode

    [Fact]
    public void Money_WhenAddedToHashSet_DoesNotAllowDuplicatesWithSameValue()
    {
        // Arrange
        var money1 = new Money(10.00m);
        var money2 = new Money(15.00m);
        var money3 = new Money(10.00m);

        var hashSet = new HashSet<Money>();

        // Act
        hashSet.Add(money1);
        hashSet.Add(money2);
        hashSet.Add(money3);

        // Assert
        Assert.Equal(2, hashSet.Count);
        Assert.Contains(money1, hashSet);
        Assert.Contains(money2, hashSet);
    }

    [Fact]
    public void Money_GetHashCode_WhenValuesAreEqual_ReturnsSameHashCode()
    {
        var m1 = new Money(10.00m);
        var m2 = new Money(10.00m);

        Assert.Equal(m1.GetHashCode(), m2.GetHashCode());
    }

    #endregion HashSet / GetHashCode
}