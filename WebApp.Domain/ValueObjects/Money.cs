namespace WebApp.Domain.ValueObjects;

public sealed class Money : IComparable<Money>, IEquatable<Money>
{
    public decimal Value { get; }

    public Money(decimal value)
    {
        if (value < 0)
            throw new ArgumentException("Money value cannot be negative.", nameof(value));

        if (decimal.Round(value, 2) != value)
            throw new ArgumentException("Money must have at most two decimal places.", nameof(value));

        Value = value;
    }

    public Money Add(Money other)
    {
        if (other is null)
            throw new ArgumentNullException(nameof(other));

        return new Money(Value + other.Value);
    }

    public Money Subtract(Money other)
    {
        if (other is null)
            throw new ArgumentNullException(nameof(other));

        var result = Value - other.Value;

        if (result < 0)
            throw new ArgumentException("Resulting Money cannot be negative.");

        return new Money(result);
    }

    public int CompareTo(Money? other)
    {
        return other is null ? 1 : Value.CompareTo(other.Value);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Money);
    }

    public bool Equals(Money? other)
    {
        if (other is null) return false;
        
        if (ReferenceEquals(this, other)) return true;
        
        return Value == other.Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}