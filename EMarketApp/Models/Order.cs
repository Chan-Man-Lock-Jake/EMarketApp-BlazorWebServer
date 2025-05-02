namespace EMarketApp.Models;

public class Order
{
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public DateOnly OrderDate { get; set; }
    public string Status { get; set; }
    public decimal TotalAmount { get; set; }
    public string DeliveryAddress { get; set; }
}