using OnyxDoc.AuthService.Application.Common.Exceptions;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using OnyxDoc.AuthService.Domain.Enums;
using OnyxDoc.AuthService.Application.Common.Models;
using AutoMapper;
using OnyxDoc.AuthService.Application.Users.Queries.GetUsers;

namespace OnyxDoc.AuthService.Application.Users.Commands.UpdateUser
{
    public partial class EditInvitedUserCommand :  IRequest<Result>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class EditInvitedUserCommandHandler : IRequestHandler<EditInvitedUserCommand,Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        private readonly IBase64ToFileConverter _base64ToFileConverter;


        public EditInvitedUserCommandHandler(IApplicationDbContext context, IIdentityService identityService, IMapper mapper, IBase64ToFileConverter base64ToFileConverter)
        {
            _context = context;
            _identityService = identityService;
            _mapper = mapper;
            _base64ToFileConverter = base64ToFileConverter;
        }

        public async Task<Result> Handle(EditInvitedUserCommand request, CancellationToken cancellationToken)
        {

            try
            {
               
                var getUserForUpdate = await _identityService.GetUserByEmail(request.Email);
                if (getUserForUpdate.user == null)
                {
                    return Result.Failure(new string[] { "Invalid email!" });
                }
                getUserForUpdate.user.LastModifiedDate = DateTime.Now;
                getUserForUpdate.user.LastModifiedById = getUserForUpdate.user.UserId;
                getUserForUpdate.user.FirstName = request.FirstName.Trim();
                getUserForUpdate.user.LastName = request.LastName;
                getUserForUpdate.user.PhoneNumber = request.PhoneNumber;
                getUserForUpdate.user.Name = string.Concat(getUserForUpdate.user.FirstName, " ", getUserForUpdate.user.LastName);
                getUserForUpdate.user.Password = request.Password;
                await _identityService.UpdateUserAsync(getUserForUpdate.user);
                await _context.SaveChangesAsync(cancellationToken);
                var userResult = _mapper.Map<UserDto>(getUserForUpdate.user);
                return Result.Success("Invited user details updated successfully", userResult);
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Error updating invited user:", ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
