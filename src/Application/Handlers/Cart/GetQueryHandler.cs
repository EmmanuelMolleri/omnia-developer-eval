using System;
using System.Net;
using Application.Queries.Cart;
using Domain.Dto;
using Infraestructure.Repositories;
using MediatR;

namespace Application.Handlers.Cart;

public class GetQueryHandler : IRequestHandler<GetQuery, BaseResult<BasePaginationResult<CartDto>?>>
{
    private readonly ICartDomain _cartDomain;

    public GetQueryHandler(ICartDomain cartDomain)
    {
        _cartDomain = cartDomain;
    }

    public async Task<BaseResult<BasePaginationResult<CartDto>?>> Handle(GetQuery request, CancellationToken cancellationToken)
    {
        var query = _cartDomain.Carts;

        var result = new BaseResult<BasePaginationResult<CartDto>?>();

        if (!query.Any())
        {
            result.Status = HttpStatusCode.NoContent;
            return result;
        }

        return result;
    }
}
