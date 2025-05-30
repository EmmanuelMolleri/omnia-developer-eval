using System.Net;
using Application.Commands.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace auth_api.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.Status == HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }

        return Ok(new
        {
            token = result.Result
        });
    }
}