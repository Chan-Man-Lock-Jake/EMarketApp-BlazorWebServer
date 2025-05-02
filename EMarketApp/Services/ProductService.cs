using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using EMarketApp.Data;
using EMarketApp.Models;

namespace EMarketApp.Services;

public class ProductService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<ProductService> _logger;
    public ProductService(ApplicationDbContext context, ILogger<ProductService> logger)
    {
        _dbContext = context;
        _logger = logger;
    }

    // (DONE) GET /api/products - List all products (with search & category filter)
    public async Task<List<Product>> GetAllProductsAsync(string searchQuery = null, Guid? categoryId = null)
    {
        IQueryable<Product> query = _dbContext.Products.AsQueryable();

        // Filter by search query (e.g., product name or description)
        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            query = query.Where(p => p.ProductName.Contains(searchQuery) || p.Description.Contains(searchQuery));
        }

        // Filter by category ID if provided
        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        // Execute the query and return the result
        
        var products = await query.ToListAsync();
        _logger.LogInformation($"Succcessfully fetched {products.Count} products.");
        return products;
    }
    
    // (DONE) GET /api/products/{id} - Get product detail
    public async Task<Product?> GetProductByIdAsync(Guid id)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.ProductId == id);
        _logger.LogInformation($"Succcessfully fetched product, {product}.");
        return product;
    }
    
    // (DONE) GET /api/categories - List product categories
    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        var categories = await _dbContext.Categories.ToListAsync();
        _logger.LogInformation($"Succcessfully fetched {categories.Count} categories.");
        return categories;
    }



    public async Task AddCategoryAsync(Category category)
    {
        try
        {
            category.CategoryId = Guid.NewGuid();
            _dbContext.Add(category);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public async Task AddProductAsync(Product product)
    {
        try
        {
            product.ProductId = Guid.NewGuid();
            _dbContext.Add(product);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}