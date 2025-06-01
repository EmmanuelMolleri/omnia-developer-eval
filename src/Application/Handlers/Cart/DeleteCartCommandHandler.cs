using System.Net;
using Application.Commands.Cart;
using Domain.Dto;
using Infraestructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.Cart;

public class DeleteCartCommandHandler : IRequestHandler<DeleteCartCommand, BaseResult<string>>
{
    private readonly ICartDomain _cartDomain;

    public DeleteCartCommandHandler(ICartDomain cartDomain)
    {
        _cartDomain = cartDomain;
    }

    public async Task<BaseResult<string>> Handle(DeleteCartCommand request, CancellationToken cancellationToken)
    {
        var result = new BaseResult<string>();
        var cart = await _cartDomain.Carts.FindAsync(new object[] { request.Id }, cancellationToken);

        if (cart == null)
        {
            result.Status = HttpStatusCode.NotFound;
            result.Messages.Add($"Cart with ID {request.Id} not found.");
            return result;
        }

        try
        {
            _cartDomain.Carts.Remove(cart);
            _cartDomain.SaveChanges();

            result.Status = HttpStatusCode.OK;
            result.Result = $"Cart {request.Id} deleted successfully.";
        }
        catch (Exception ex)
        {
            result.Status = HttpStatusCode.InternalServerError;
            result.Messages.Add($"Error deleting cart: {ex.Message}");
        }

        return result;
    }
}
