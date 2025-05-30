namespace Domain.Dto;

public class CartDto
{
    public int CartId { get; set; }
    public int UserId { get; set; }
    public DateTime Data { get; set; }
    public List<ProductResumeDto> Products { get; set; } = [];
}
