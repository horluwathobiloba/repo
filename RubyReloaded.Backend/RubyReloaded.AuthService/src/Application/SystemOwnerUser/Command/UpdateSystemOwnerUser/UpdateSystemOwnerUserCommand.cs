using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.SuperAdmin.Command.UpdateSuperAdmin
{
    public class UpdateSystemOwnerUserCommand:IRequest<Result>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string LoggeInUser { get; set; }
    }


    public class UpdateSuperAdminCommandHandler : IRequestHandler<UpdateSystemOwnerUserCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        public UpdateSuperAdminCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _identityService = identityService;
            _context = context;
        }
        public async Task<Result> Handle(UpdateSystemOwnerUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var getUserForUpdate = await _identityService.GetUserByEmail(request.Email);
                var getSystemOwner = await _context.SystemOwnerUsers.FirstOrDefaultAsync(x => x.Email == request.Email);

                if (getUserForUpdate.user == null)
                {
                    return Result.Failure(new string[] { "Invalid Staff details for update" });
                }
                var index = request.Email.IndexOf('@');
                getUserForUpdate.user.LastModifiedDate = DateTime.Now;
                getUserForUpdate.user.Email = request.Email.Trim();
                getUserForUpdate.user.FirstName = request.FirstName.Trim();
                getUserForUpdate.user.LastName = request.LastName;
                getUserForUpdate.user.PhoneNumber = request.PhoneNumber;
                getUserForUpdate.user.Name = string.Concat(getUserForUpdate.user.FirstName, " ", getUserForUpdate.user.LastName);
                await _identityService.UpdateUserAsync(getUserForUpdate.user);
                getSystemOwner.Name = request.FirstName + " " + request.LastName;
                getSystemOwner.LastModifiedDate = DateTime.Now;
              
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("User details updated successfully", getUserForUpdate.user);
            }
            catch (Exception ex)
            {
                return Result.Failure("Staff update was not successful "+ ex?.Message ?? ex?.InnerException.Message);
            }

           

        }
    }
}
