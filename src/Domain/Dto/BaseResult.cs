using System.Net;

namespace Domain.Dto;

public class BaseResult<T>
{
    public HttpStatusCode Status { get; set; }
    public T Result { get; set; }
    public List<string> Messages { get; set; } = new List<string>();
}
