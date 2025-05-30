using System;
using Domain.Enums;

namespace Domain.Dto;

public class BasePagination
{
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
    public OrderEnums? Order { get; set; }
}
