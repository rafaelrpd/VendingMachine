namespace WebApp.Domain.ValueObjects;

public sealed class Denomination : IComparable<Denomination>, IEquatable<Denomination>
{
    private static readonly HashSet<decimal> AllowedValues = new()
    {
        0.01m, 0.05m, 0.10m, 0.25m, 0.50m, 1.00m,
        2.00m, 5.00m, 10.00m, 20.00m, 50.00m, 100.00m, 200.00m
    };

    public decimal Value { get; }

    public Denomination(decimal value)
    {
        if (value < 0)
            throw new ArgumentException("Denomination value cannot be negative.", nameof(value));

        if (decimal.Round(value, 2) != value)
            throw new ArgumentException("Denomination must have at most two decimal places.", nameof(value));

        if (value == 0)
            throw new ArgumentException("Denomination value cannot be zero.", nameof(value));

        if (!AllowedValues.Contains(value))
            throw new ArgumentException("Denomination value is not allowed.", nameof(value));

        Value = value;
    }

    public int CompareTo(Denomination? other)
    {
        return other is null ? 1 : Value.CompareTo(other.Value);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Denomination);
    }

    public bool Equals(Denomination? other)
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