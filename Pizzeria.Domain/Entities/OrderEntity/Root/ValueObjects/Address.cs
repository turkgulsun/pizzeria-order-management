using Pizzeria.Domain.SeedWork;

namespace Pizzeria.Domain.Entities.OrderEntity.Root.ValueObjects;

public class Address : ValueObject
{
    public string Street { get; }
    public string City { get; }
    public string PostalCode { get; }

    public Address(string street, string city, string postalCode)
    {
        Street = street;
        City = city;
        PostalCode = postalCode;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return PostalCode;
    }
    
    public override string ToString()
    {
        return $"{Street}, {City} {PostalCode}";
    }
}