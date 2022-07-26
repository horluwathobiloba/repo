using MediatR;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.AjoRole.Commands.CreateAjoRole
{
    public class CreateAjoRoleCommand:IRequest<Result>
    {

        public int AjoId { get; set; }
        public string LoggedInUserId { get; set; }
        public string Name { get; set; }
        public AccessLevel AccessLevel { get; set; }
    }
    public class CreateAjoRoleCommandHandler : IRequestHandler<CreateAjoRoleCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        public CreateAjoRoleCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<Result> Handle(CreateAjoRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var role = new Domain.Entities.AjoRole
                {
                    Name = request.Name,
                    CreatedById = request.LoggedInUserId,
                    CreatedDate = DateTime.Now,
                    AccessLevel = request.AccessLevel,
                    AccessLevelDesc = request.AccessLevel.ToString(),
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString(),
                    AjoId=request.AjoId
                };
                await _context.AjoRoles.AddAsync(role);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Role created successfully", role);
            }
            catch (Exception ex)
            {
                return Result.Failure( "Role creation was not successful " +ex?.Message ?? ex?.InnerException.Message );
            }
        }
    }
}
