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
    public partial class UpdateUserCommand :  IRequest<Result>
    {

        public int SubscriberId { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int RoleId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime EmploymentDate { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Signature { get; set; }
        public string JobTitle { get; set; }
        public string ProfilePicture { get; set; }
    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand,Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        private readonly IBase64ToFileConverter _base64ToFileConverter;


        public UpdateUserCommandHandler(IApplicationDbContext context, IIdentityService identityService, IMapper mapper, IBase64ToFileConverter base64ToFileConverter)
        {
            _context = context;
            _identityService = identityService;
            _mapper = mapper;
            _base64ToFileConverter = base64ToFileConverter;
        }

        public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {

            try
            {
               
                var user = await _identityService.GetUserByIdAndSubscriber(request.UserId, request.SubscriberId);
                if (user.user == null)
                {
                    return Result.Failure(new string[] { "Unable to change status.Invalid User ID and Subscriber credentials!" });
                }

                var getUserForUpdate = await _identityService.GetUserById(request.UserId);
                if (getUserForUpdate.user == null)
                {
                    return Result.Failure(new string[] { "Invalid User details for update" });
                }
                //var getUserForUpdate = await _identityService.GetUserByUsername(request.Email);
                //if (getUserForUpdate.user == null)
                //{
                //    return Result.Failure(new string[] { "User for update does not exist with specified username" });
                //}
                getUserForUpdate.user.LastModifiedDate = DateTime.Now;
                getUserForUpdate.user.LastModifiedById = user.user.UserId;
                getUserForUpdate.user.FirstName = request.FirstName.Trim();
                getUserForUpdate.user.LastName = request.LastName;
                getUserForUpdate.user.SubscriberId = request.SubscriberId;
                getUserForUpdate.user.PhoneNumber = request.PhoneNumber;
                getUserForUpdate.user.RoleId = request.RoleId;
               // getUserForUpdate.user.UserName = request.Email;
                getUserForUpdate.user.Country = request.Country;
                getUserForUpdate.user.City = request.City;
                getUserForUpdate.user.Name = string.Concat(getUserForUpdate.user.FirstName, " ", getUserForUpdate.user.LastName);
                getUserForUpdate.user.JobTitle = request.JobTitle;
                getUserForUpdate.user.Signature = await _base64ToFileConverter.ConvertBase64StringToFile(request.Signature, DateTime.Now.Ticks + "_" + request.FirstName + "_" + request.LastName + "_sign.png");
                getUserForUpdate.user.ProfilePicture = await _base64ToFileConverter.ConvertBase64StringToFile(request.ProfilePicture, DateTime.Now.Ticks + "_"+request.FirstName + "_" + request.LastName + ".png");
                await _identityService.UpdateUserAsync(getUserForUpdate.user);
                await _context.SaveChangesAsync(cancellationToken);
                var userResult = _mapper.Map<UserDto>(getUserForUpdate.user);
                return Result.Success("User details updated successfully", userResult);
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Error updating user:", ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
