using System;

namespace Domain.Dto;

public class RatingDto
{
    public int Id { get; set; }
    public decimal Rate { get; set; }
    public int Count { get; set; }
}
