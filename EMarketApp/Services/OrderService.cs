using Microsoft.EntityFrameworkCore;
using EMarketApp.Data;
using EMarketApp.Models;

namespace EMarketApp.Services;

public class OrderService
{
    private readonly ApplicationDbContext _dbContext;
    public OrderService(ApplicationDbContext context)
    {
        _dbContext = context;
    }

    // (DONE) POST /api/orders - Place order (from cart)
    public async Task<Order?> PlaceOrderAsync(Guid userId)
    {
        // Get the user's cart items
        var cartItems = await _dbContext.CartItems
            .Where(ci => ci.UserId == userId)
            .ToListAsync();

        if (!cartItems.Any())
            return null;  // No items in cart, nothing to order

        // Create a new Order
        Guid OrderId = Guid.NewGuid();
        var order = new Order
        {
            OrderId = OrderId,
            UserId = userId,
            OrderDate = DateOnly.FromDateTime(DateTime.UtcNow),
            Status = "Pending",
            TotalAmount = cartItems.Sum(item => item.Quantity * item.Product.Price),  // assuming navigation property Product is included
            DeliveryAddress = ""
        };
        var OrderItems = cartItems.Select(ci => new OrderItem
        {
            OrderItemId = Guid.NewGuid(),
            OrderId = OrderId,
            ProductId = ci.ProductId,
            Quantity = ci.Quantity,
            Price = ci.Product.Price  // Save the price at time of order
        }).ToList();

        // Save order
        _dbContext.Orders.Add(order);
        _dbContext.OrderItems.AddRange(OrderItems);

        // Clear cart
        _dbContext.CartItems.RemoveRange(cartItems);

        // Commit transaction
        await _dbContext.SaveChangesAsync();

        return order;
    }

    // (DONE) GET /api/orders - List current user's orders
    public async Task<List<Order>> GetAllOrdersAync() =>
        await _dbContext.Orders.ToListAsync();

    // (DONE) GET /api/orders/{id) - Order detail
    public async Task<Order?> GetOrderById(Guid id) =>
        await _dbContext.Orders.FirstOrDefaultAsync(o => o.OrderId == id);

    // POST /aps/orders/{id)/pay - Mock payment endpoint
}