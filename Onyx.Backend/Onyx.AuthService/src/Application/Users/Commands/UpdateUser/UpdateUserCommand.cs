using Onyx.AuthService.Application.Common.Exceptions;
using Onyx.AuthService.Application.Common.Interfaces;
using Onyx.AuthService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using Onyx.AuthService.Domain.Enums;
using Onyx.AuthService.Application.Common.Models;
using AutoMapper;
using Onyx.AuthService.Application.Users.Queries.GetUsers;

namespace Onyx.AuthService.Application.Users.Commands.UpdateUser
{
    public partial class UpdateUserCommand :  IRequest<Result>
    {

        public int OrganizationId { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public Gender Gender { get; set; }
        public int RoleId { get; set; }
        public int JobfunctionId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime EmploymentDate { get; set; }
        public string Country { get; set; }
        public string State { get; set; }

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
                var user = await _identityService.GetUserByIdAndOrganization(request.UserId, request.OrganizationId);
                if (user.staff == null)
                {
                    return Result.Failure(new string[] { "Unable to change status.Invalid User ID and Organization credentials!" });
                }

                var getUserForUpdate = await _identityService.GetUserById(request.UserId);
                if (getUserForUpdate.staff == null)
                {
                    return Result.Failure(new string[] { "Invalid User details for update" });
                }
                //var getUserForUpdate = await _identityService.GetUserByUsername(request.Email);
                //if (getUserForUpdate.staff == null)
                //{
                //    return Result.Failure(new string[] { "User for update does not exist with specified username" });
                //}
                getUserForUpdate.staff.Address = request.Address.Trim();
                getUserForUpdate.staff.LastModifiedDate = DateTime.Now;
                getUserForUpdate.staff.LastModifiedBy = user.staff.UserName;
                getUserForUpdate.staff.DateOfBirth = request.DateOfBirth;
                getUserForUpdate.staff.FirstName = request.FirstName.Trim();
                getUserForUpdate.staff.Gender = request.Gender;
                getUserForUpdate.staff.LastName = request.LastName;
                getUserForUpdate.staff.OrganizationId = request.OrganizationId;
                getUserForUpdate.staff.PhoneNumber = request.PhoneNumber;
                getUserForUpdate.staff.EmploymentDate = request.EmploymentDate;
                getUserForUpdate.staff.RoleId = request.RoleId;
               // getUserForUpdate.staff.UserName = request.Email;
                getUserForUpdate.staff.Country = request.Country;
                getUserForUpdate.staff.State = request.State;
                getUserForUpdate.staff.JobFunctionId = request.JobfunctionId;
                getUserForUpdate.staff.Name = string.Concat(getUserForUpdate.staff.FirstName, " ", getUserForUpdate.staff.LastName);
                getUserForUpdate.staff.ProfilePicture = await _base64ToFileConverter.ConvertBase64StringToFile(request.ProfilePicture, DateTime.Now.Ticks + "_"+request.FirstName + "_" + request.LastName + ".png");
                await _identityService.UpdateUserAsync(getUserForUpdate.staff);
                await _context.SaveChangesAsync(cancellationToken);
                var staffResult = _mapper.Map<UserDto>(getUserForUpdate.staff);
                return Result.Success("User details updated successfully", staffResult);
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Error updating staff:", ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
