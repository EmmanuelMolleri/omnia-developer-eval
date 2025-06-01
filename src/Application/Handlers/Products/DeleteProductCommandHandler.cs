using System;
using System.Net;
using Application.Commands.Products;
using Domain.Dto;
using Infraestructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.Products;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, BaseResult<string>>
{
    private readonly IProductDomain _productDomain;

    public DeleteProductCommandHandler(IProductDomain productDomain)
    {
        _productDomain = productDomain;
    }

    public async Task<BaseResult<string>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productDomain.Products.FirstOrDefaultAsync(p => p.ProductId == request.Id, cancellationToken);
        if (product == null)
        {
            return new BaseResult<string> { Status = HttpStatusCode.NotFound, Messages = ["Produto não encontrado"] };
        }

        _productDomain.Products.Remove(product);
        _productDomain.SaveChanges();

        return new BaseResult<string> { Status = HttpStatusCode.OK, Result = "Produto removido com sucesso" };
    }
}
