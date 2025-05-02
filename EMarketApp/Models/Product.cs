namespace EMarketApp.Models;

public class Product
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string ImageUrl { get; set; }
    public Guid CategoryId { get; set; }
    public Category Category { get; set; }
}