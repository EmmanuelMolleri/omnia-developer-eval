namespace Domain.Entities;

public class Product
{
    public int ProductId { get; set; }
    public string? Title { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public string? Image { get; set; }
    public ICollection<Rating>? Rating { get; set; }
    public ICollection<CartProduct> CartProducts { get; set; } = new List<CartProduct>();
}
