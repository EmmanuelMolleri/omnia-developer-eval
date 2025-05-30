namespace Domain.Entities;

public class Product
{
    public int ProductId { get; set; }
    public ICollection<Cart> Carts { get; set; } = new List<Cart>();
}
