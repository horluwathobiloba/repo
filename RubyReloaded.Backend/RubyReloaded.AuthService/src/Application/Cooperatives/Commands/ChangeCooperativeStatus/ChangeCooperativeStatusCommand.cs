using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Cooperatives.Commands.ChangeCooperativeStatus
{
    public class ChangeCooperativeStatusCommand: IRequest<Result>
    {
        public int CooperativeId { get; set; }
        public string UserId { get; set; }
        public string LoggedInUserId { get; set; }
    }

    public class ChangeCooperativeStatusCommandHandler : IRequestHandler<ChangeCooperativeStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public ChangeCooperativeStatusCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<Result> Handle(ChangeCooperativeStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string message = "";
                var user = await _context.CooperativeMembers.FirstOrDefaultAsync(x => x.CooperativeId == request.CooperativeId && x.UserId == request.UserId&&x.RoleId==1);
                if (user == null)
                {
                    return Result.Failure(new string[] { "Invalid User for this operation" });
                }
                var entity = await _context.Cooperatives.FindAsync(request.CooperativeId);
                if (entity == null)
                {
                    return Result.Failure(new string[] { "Invalid User" });
                }
                switch (entity.Status)
                {
                    case Domain.Enums.Status.Active:
                        entity.Status = Domain.Enums.Status.Inactive;
                        message = "Cooperative deactivation was successful";
                        break;
                    case Domain.Enums.Status.Inactive:
                        entity.Status = Domain.Enums.Status.Active;
                        message = "Cooperative activation was successful";
                        break;
                    case Domain.Enums.Status.Deactivated:
                        entity.Status = Domain.Enums.Status.Active;
                        message = "Cooperative activation was successful";
                        break;
                    default:
                        break;
                }
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success(message);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Cooperative status change was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
