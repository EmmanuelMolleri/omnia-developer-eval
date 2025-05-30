using Domain.Dto;
using MediatR;

namespace Application.Queries.Users;

public class GetUserByIdQuery:  BasePagination, IRequest<BaseResult<UserDto?>>
{
    public int Id { get; set; }
}
