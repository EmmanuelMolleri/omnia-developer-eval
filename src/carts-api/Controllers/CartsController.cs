using System.Net;
using Application.Commands.Cart;
using Application.Queries.Cart;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace carts_api.Controllers;

[Route("api/carts")]
[ApiController]
public class CartsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CartsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromRoute] GetQuery query)
    {
        var result = await _mediator.Send(query);

        if (result.Result.Data == null)
        {
            return NoContent();
        }

        return Ok(result.Result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] GetByIdQuery query)
    {
        var result = await _mediator.Send(query);

        if (result.Result == null)
        {
            return NoContent();
        }

        return Ok(result.Result);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateCartCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.Status != HttpStatusCode.Created)
        {
            return BadRequest(result.Messages);
        }

        return Created("/", result.Result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] UpdateCartCommand command)
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
        var command = new DeleteCartCommand(){ Id = id };

        var result = await _mediator.Send(command);

        if (result.Status != HttpStatusCode.OK)
        {
            return BadRequest(result.Messages);
        }

        return Ok(new { message = result.Result });
    }
}