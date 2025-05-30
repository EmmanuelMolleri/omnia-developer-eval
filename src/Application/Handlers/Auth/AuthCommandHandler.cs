using System.Net;
using Application.Commands.Auth;
using Domain.Dto;
using Infraestructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.Auth;

public class AuthCommandHandler : IRequestHandler<AuthCommand, BaseResult<string>>
{
    private readonly IAuthDomain _dbContext;

    public AuthCommandHandler(IAuthDomain dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BaseResult<string>> Handle(AuthCommand request, CancellationToken cancellationToken)
    {
        var result = new BaseResult<string>();
        var existingUser = await _dbContext
            .Users
            .FirstOrDefaultAsync(x => x.UserName == request.UserName &&
                        x.Password == request.Password);
        if (existingUser == null)
        {
            result.Status = HttpStatusCode.Unauthorized;
            result.Messages.Add("User name or password is incorrect.");
            result.Result = string.Empty;

            return result;
        }

        result.Status = HttpStatusCode.OK;
        result.Result = string.Empty; // TO-DO token service
        return result;
    }
}
