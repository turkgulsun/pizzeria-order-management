namespace Pizzeria.Domain.SeedWork;

public abstract class Entity<TId> where TId : notnull
{
    public TId Id { get; protected set; }
    protected Entity(TId id) => Id = id;
    
    protected Entity() {} // ORM için

    public override bool Equals(object? obj)
    {
        if (obj is not Entity<TId> other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return Id.Equals(other.Id);
    }

    public override int GetHashCode() => Id.GetHashCode();
}