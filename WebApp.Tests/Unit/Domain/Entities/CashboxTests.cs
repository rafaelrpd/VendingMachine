using WebApp.Domain.Entities;
using WebApp.Domain.ValueObjects;

namespace WebApp.Tests.Unit.Domain.Entities;

public class CashboxTests
{
    #region Creation / Invariants

    [Fact]
    public void Cashbox_Create_WhenEmpty_HasZeroTotal()
    {
        // Arrange
        var cashbox = new Cashbox();

        // Act
        var total = cashbox.GetTotal();

        // Assert
        Assert.Equal(new Money(0.00m), total);
    }

    #endregion Creation / Invariants

    #region Add

    [Fact]
    public void Cashbox_Add_WhenQuantityIsPositive_IncreasesQuantityAndTotal()
    {
        // Arrange
        var cashbox = new Cashbox();
        var denomination = new Denomination(50.00m);

        // Act
        cashbox.Add(denomination, 1);

        // Assert
        Assert.Equal(1, cashbox.GetQuantity(denomination));
        Assert.Equal(new Money(50.00m), cashbox.GetTotal());
    }

    [Fact]
    public void Cashbox_Add_WhenQuantityIsZero_ThrowsArgumentException()
    {
        // Arrange
        var cashbox = new Cashbox();
        var denomination = new Denomination(20.00m);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => cashbox.Add(denomination, 0));
    }

    [Fact]
    public void Cashbox_Add_WhenQuantityIsNegative_ThrowsArgumentException()
    {
        // Arrange
        var cashbox = new Cashbox();
        var denomination = new Denomination(20.00m);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => cashbox.Add(denomination, -1));
    }

    [Fact]
    public void Cashbox_Add_WhenSameDenominationAddedTwice_AccumulatesQuantity()
    {
        // Arrange
        var cashbox = new Cashbox();
        var denomination = new Denomination(10.00m);

        // Act
        cashbox.Add(denomination, 1);
        cashbox.Add(denomination, 2);

        // Assert
        Assert.Equal(3, cashbox.GetQuantity(denomination));
        Assert.Equal(new Money(30.00m), cashbox.GetTotal());
    }
    
    [Fact]
    public void Cashbox_Add_WhenDenominationIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var cashbox = new Cashbox();
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => cashbox.Add(null!, 1));
    }

    #endregion Add

    #region Remove

    [Fact]
    public void Cashbox_Remove_WhenEnoughQuantity_DecreasesQuantityAndTotal()
    {
        // Arrange
        var cashbox = new Cashbox();
        var denomination = new Denomination(10.00m);

        // Act
        cashbox.Add(denomination, 3);
        cashbox.Remove(denomination, 1);

        // Assert
        Assert.Equal(2, cashbox.GetQuantity(denomination));
        Assert.Equal(new Money(20.00m), cashbox.GetTotal());
    }

    [Fact]
    public void Cashbox_Remove_WhenNotEnoughQuantity_ThrowsInvalidOperationException()
    {
        // Arrange
        var cashbox = new Cashbox();
        var denomination = new Denomination(5.00m);

        // Act
        cashbox.Add(denomination, 1);

        // Assert
        Assert.Throws<InvalidOperationException>(() => cashbox.Remove(denomination, 2));
    }

    [Fact]
    public void Cashbox_Remove_WhenQuantityIsNegative_ThrowsArgumentException()
    {
        // Arrange
        var cashbox = new Cashbox();
        var denomination = new Denomination(5.00m);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => cashbox.Remove(denomination, -1));
    }

    [Fact]
    public void Cashbox_Remove_WhenQuantityIsZero_ThrowsArgumentException()
    {
        // Arrange
        var cashbox = new Cashbox();
        var denomination = new Denomination(5.00m);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => cashbox.Remove(denomination, 0));
    }

    [Fact]
    public void Cashbox_Remove_WhenRemovingAllQuantity_SetsQuantityToZero()
    {
        // Arrange
        var cashbox = new Cashbox();
        var denomination = new Denomination(10.00m);

        // Act
        cashbox.Add(denomination, 2);
        cashbox.Remove(denomination, 2);

        // Assert
        Assert.Equal(0, cashbox.GetQuantity(denomination));
        Assert.Equal(new Money(0.00m), cashbox.GetTotal());
    }

    [Fact]
    public void Cashbox_Remove_WhenDenominationWasNeverAdded_ThrowsInvalidOperationException()
    {
        // Arrange
        var cashbox = new Cashbox();
        var denomination = new Denomination(10.00m);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => cashbox.Remove(denomination, 1));
    }

    [Fact]
    public void Cashbox_Remove_WhenDenominationIsNull_ThrowsArgumentNullException()
    {
        var cashbox = new Cashbox();
        Assert.Throws<ArgumentNullException>(() => cashbox.Remove(null!, 1));
    }

    #endregion Remove

    #region Total

    [Fact]
    public void Cashbox_GetTotal_WhenHasMultipleDenominations_ReturnsCorrectTotal()
    {
        // Arrange
        var cashbox = new Cashbox();
        var denomination1 = new Denomination(20.00m);
        var denomination2 = new Denomination(50.00m);
        var denomination3 = new Denomination(10.00m);

        // Act
        cashbox.Add(denomination1, 2);
        cashbox.Add(denomination2, 1);
        cashbox.Add(denomination3, 1);

        // Assert
        Assert.Equal(new Money(100.00m), cashbox.GetTotal());
    }

    [Fact]
    public void Cashbox_GetTotal_WhenEmpty_ReturnsZero()
    {
        // Arrange
        var cashbox = new Cashbox();

        // Act
        var total = cashbox.GetTotal();

        // Assert
        Assert.Equal(new Money(0.00m), total);
    }

    #endregion Total

    #region Queries

    [Fact]
    public void Cashbox_GetQuantity_WhenDenominationWasNeverAdded_ReturnsZero()
    {
        // Arrange
        var cashbox = new Cashbox();
        var denomination = new Denomination(100.00m);

        // Act
        var quantity = cashbox.GetQuantity(denomination);

        // Assert
        Assert.Equal(0, quantity);
    }

    [Fact]
    public void Cashbox_GetQuantity_WhenDenominationIsNull_ThrowsArgumentNullException()
    {
        var cashbox = new Cashbox();
        Assert.Throws<ArgumentNullException>(() => cashbox.GetQuantity(null!));
    }

    #endregion Queries
}