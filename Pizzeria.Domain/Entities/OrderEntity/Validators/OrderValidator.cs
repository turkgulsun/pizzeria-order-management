using FluentValidation;
using Pizzeria.Domain.Entities.OrderEntity.Root;
using Pizzeria.Domain.Entities.OrderEntity.Root.Entities;
using Pizzeria.Domain.Entities.OrderEntity.Root.ValueObjects;

namespace Pizzeria.Domain.Entities.OrderEntity.Validators;

public class OrderValidator : AbstractValidator<Order>
{
    public OrderValidator()
    {
        RuleFor(o => o.Id).NotEmpty();
        RuleFor(o => o.DeliveryAddress).NotNull().SetValidator(new AddressValidator());
        RuleFor(o => o.CreatedAt).LessThanOrEqualTo(DateTime.UtcNow);
        RuleFor(o => o.DeliveryAt).GreaterThanOrEqualTo(o => o.CreatedAt);
        RuleFor(o => o.Items).NotEmpty();
        RuleForEach(o => o.Items).SetValidator(new OrderItemValidator());
    }
}

public class OrderItemValidator : AbstractValidator<OrderItem>
{
    public OrderItemValidator()
    {
        RuleFor(i => i.ProductId).NotEmpty();
        RuleFor(i => i.Quantity).GreaterThan(0);
    }
}

public class AddressValidator : AbstractValidator<Address>
{
    public AddressValidator()
    {
        RuleFor(a => a.Street).NotEmpty().MaximumLength(200);
        RuleFor(a => a.City).NotEmpty().MaximumLength(100);
        RuleFor(a => a.PostalCode).NotEmpty().MaximumLength(20);
    }
}