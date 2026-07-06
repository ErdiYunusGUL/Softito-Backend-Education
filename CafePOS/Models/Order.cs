namespace CafePOS.Models;

public class Order
{
    public int Id { get; set; }
    
    public int TableId { get; set; }
    public CafeTable? Table { get; set; }
    
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    
    public int Quantity { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public bool IsPaid { get; set; } = false;
    public DateTime? PaidDate { get; set; }
    public string? CustomerName { get; set; }
}
