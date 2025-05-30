using System;
using Domain.Dto;
using MediatR;

namespace Application.Queries.Users;

public class GetUsersQuery: IRequest<BaseResult<BasePaginationResult<UserDto>?>>
{
    public string? Category { get; set; }
    public string? _order { get; set; }
}

