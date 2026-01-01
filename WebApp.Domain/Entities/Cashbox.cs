using WebApp.Domain.ValueObjects;

namespace WebApp.Domain.Entities;

public sealed class Cashbox
{
    private readonly Dictionary<Denomination, int> _contents = new();

    public Cashbox()
    {
    }

    public Money GetTotal()
    {
        decimal total = _contents.Sum(kvp => kvp.Key.Value * kvp.Value);

        return new Money(total);
    }

    public void Add(Denomination denomination, int quantity)
    {
        if (denomination is null)
            throw new ArgumentNullException(nameof(denomination));

        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be positive.");

        if (!_contents.TryAdd(denomination, quantity))
            _contents[denomination] += quantity;
    }

    public void Remove(Denomination denomination, int quantity)
    {
        if (denomination is null)
            throw new ArgumentNullException(nameof(denomination));

        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be positive.");
        
        if (!_contents.TryGetValue(denomination, out var current) || current < quantity)
            throw new InvalidOperationException("Not enough denomination in cashbox to remove.");

        var remaining = current - quantity;

        if (remaining == 0) _contents.Remove(denomination);
        else _contents[denomination] = remaining;
    }

    public int GetQuantity(Denomination denomination)
    {
        return denomination is null ? throw new ArgumentNullException(nameof(denomination)) : _contents.GetValueOrDefault(denomination, 0);
    }

    public IReadOnlyDictionary<Denomination, int> GetSnapshot()
    {
        return new Dictionary<Denomination, int>(_contents);
    }
}