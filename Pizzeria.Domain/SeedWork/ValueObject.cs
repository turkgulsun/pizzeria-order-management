namespace Pizzeria.Domain.SeedWork;

public abstract class ValueObject : IEquatable<ValueObject>
{
    protected abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object? obj) => obj is ValueObject other && Equals(other);

    public bool Equals(ValueObject? other) =>
        other is not null && GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());

    public override int GetHashCode() => GetEqualityComponents()
        .Aggregate(1, (current, obj) => current * 23 + (obj?.GetHashCode() ?? 0));
}