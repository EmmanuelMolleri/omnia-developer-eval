using System.Net;
using Application.Commands.User;
using Application.Queries.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace users_api.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    
    [HttpGet]
    public async Task<IActionResult> Get([FromRoute] GetUsersQuery query)
    {
        var result = await _mediator.Send(query);

        if (result.Result.Data == null)
        {
            return NoContent();
        }

        return Ok(result.Result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] GetUserByIdQuery query)
    {
        var result = await _mediator.Send(query);

        if (result.Result == null)
        {
            return NoContent();
        }

        return Ok(result.Result);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateUserCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.Status != HttpStatusCode.Created)
        {
            return BadRequest(result.Messages);
        }

        return Created("/", result.Result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] UpdateUserCommand command)
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
        var command = new DeleteUserCommand(){ Id = id };

        var result = await _mediator.Send(command);

        if (result.Status != HttpStatusCode.OK)
        {
            return BadRequest(result.Messages);
        }

        return Ok(new { message = result.Result });
    }
}