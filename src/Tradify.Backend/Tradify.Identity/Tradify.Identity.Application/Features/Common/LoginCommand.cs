using MediatR;
using Tradify.Identity.Application.Interfaces;

namespace Tradify.Identity.Application.Features.Common;

public record LoginCommand(string UserName,string Password) : IRequest;

public class LoginCommandHandler : IRequestHandler<LoginCommand>
{
    private IApplicationDbContext _context;

    public LoginCommandHandler(IApplicationDbContext context) =>
        (_context) = (context);
    
    public async Task Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        
    }
}


