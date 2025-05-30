using System;

namespace Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public ICollection<Cart> Carts { get; set; } = new List<Cart>();
}
