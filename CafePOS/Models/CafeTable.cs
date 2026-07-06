namespace CafePOS.Models;

public class CafeTable
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsOccupied { get; set; }
    public string? CustomerName { get; set; }
    
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
