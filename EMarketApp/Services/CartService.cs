using Microsoft.EntityFrameworkCore;
using EMarketApp.Data;
using EMarketApp.Models;

namespace EMarketApp.Services;

public class CartService
{
    private readonly ApplicationDbContext _dbContext;
    public CartService(ApplicationDbContext context)
    {
        _dbContext = context;
    }

    // (DONE) GET /api/cart - View current user's cart
    public async Task<List<CartItem>> GetAllCartItemsAsync() =>
        await _dbContext.CartItems.ToListAsync();

    // (DONE) POST /api/cart - Add product to cart
    public async Task<CartItem> AddCartItemAsync(CartItem newCartItem)
    {
        _dbContext.CartItems.Add(newCartItem);
        await _dbContext.SaveChangesAsync();
        return newCartItem;
    }

    // (DONE) PUT /api/cart/(itemId) - Update quantity
    public async Task<bool> UpdateCartItemAsync(Guid cartItemId, CartItem updatedCartItem)
    {
        var existingCartItem = await _dbContext.CartItems.FindAsync(cartItemId);
        if (existingCartItem == null)
            return false;
        
        // existingCartItem.UserId = updatedCartItem.UserId;
        // existingCartItem.ProductId = updatedCartItem.ProductId;
        existingCartItem.Quantity = updatedCartItem.Quantity;

        _dbContext.CartItems.Update(existingCartItem);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    // (DONE) DELETE /api/cart/{itemId) - Remove item
    public async Task<bool> DeleteCartItemAsync(Guid cartItemId)
    {
        var existingCartItem = await _dbContext.CartItems.FindAsync(cartItemId);
        if (existingCartItem == null)
            return false;

            _dbContext.CartItems.Remove(existingCartItem);
            await _dbContext.SaveChangesAsync();
            return true;
    }
}