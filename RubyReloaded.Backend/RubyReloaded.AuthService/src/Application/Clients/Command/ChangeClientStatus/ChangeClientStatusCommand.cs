using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Application.Common.Interfaces;

namespace RubyReloaded.AuthService.Application.Clients.Commands.ChangeClientStatus
{
    public class ChangeClientStatusCommand :  IRequest<Result>
    {
        public string Id { get; set; }
    }
     
    public class ChangeClientStatusCommandHandler : IRequestHandler<ChangeClientStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public ChangeClientStatusCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(ChangeClientStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingClient = await _context.Clients.FindAsync(request.Id);
                if (existingClient == null)
                {
                    return Result.Failure(new string[] { "Invalid Client for status change" });
                };
                string message = "";
                switch (existingClient.Status)
                {
                    case Domain.Enums.Status.Active:
                        existingClient.Status = Domain.Enums.Status.Inactive;
                        message = "Client deactivation was successful";
                        break;
                    case Domain.Enums.Status.Inactive:
                        existingClient.Status = Domain.Enums.Status.Active;
                        message = "Client activation was successful";
                        break;
                    case Domain.Enums.Status.Deactivated:
                        existingClient.Status = Domain.Enums.Status.Active;
                        message = "Client activation was successful";
                        break;
                    default:
                        break;
                }
                _context.Clients.Update(existingClient);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success(new string[] { message });
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Client deactivation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
