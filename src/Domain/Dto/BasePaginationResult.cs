using System;

namespace Domain.Dto;

public class BasePaginationResult<T>
{
    public int TotalItems { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public T Data { get; set; }
}
