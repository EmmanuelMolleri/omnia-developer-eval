using System.Net;
using Application.Commands.Products;
using Application.Queries.Products;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace products_api.Controllers;

[Route("api/products")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromRoute] GetProductsQuery query)
    {
        var result = await _mediator.Send(query);

        if (result.Result.Data == null)
        {
            return NoContent();
        }

        return Ok(result.Result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] GetProductByIdQuery query)
    {
        var result = await _mediator.Send(query);

        if (result.Result == null)
        {
            return NoContent();
        }

        return Ok(result.Result);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateProductCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.Status != HttpStatusCode.Created)
        {
            return BadRequest(result.Messages);
        }

        return Created("/", result.Result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] UpdateProductCommand command)
    {
        command.Id = id;

        var result = await _mediator.Send(command);

        if (result.Status != HttpStatusCode.OK)
        {
            return BadRequest(result.Messages);
        }

        return Ok(result.Result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteProductCommand(){ Id = id };

        var result = await _mediator.Send(command);

        if (result.Status != HttpStatusCode.OK)
        {
            return BadRequest(result.Messages);
        }

        return Ok(new { message = result.Result });
    }
}