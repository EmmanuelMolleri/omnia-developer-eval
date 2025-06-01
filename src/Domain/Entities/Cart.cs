namespace Domain.Entities;
public class Cart
{
    public int CartId { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public DateTime Data { get; set; }

    public ICollection<CartProduct> CartProducts { get; set; } = new List<CartProduct>();
}
