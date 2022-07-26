using Onyx.WorkFlowService.Application.Common.Exceptions;
using Onyx.WorkFlowService.Application.Common.Interfaces;
using Onyx.WorkFlowService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using Onyx.WorkFlowService.Domain.Enums;
using Onyx.WorkFlowService.Application.Common.Models;
using AutoMapper;
using Onyx.WorkFlowService.Application.Staffs.Queries.GetStaffs;

namespace Onyx.WorkFlowService.Application.Staffs.Commands.UpdateStaff
{
    public partial class UpdateStaffCommand :  IRequest<Result>
    {

        public int OrganizationId { get; set; }
        public string UserId { get; set; }
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string PhoneNumber { get; set; }
        //public string Email { get; set; }
        public string Address { get; set; }
        public Gender Gender { get; set; }
        public int MarketId { get; set; }
        public int RoleId { get; set; }
        public int DepartmentId { get; set; }
        public string SupervisorId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime EmploymentDate { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
    }

    public class UpdateStaffCommandHandler : IRequestHandler<UpdateStaffCommand,Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;


        public UpdateStaffCommandHandler(IApplicationDbContext context, IIdentityService identityService, IMapper mapper)
        {
            _context = context;
            _identityService = identityService;
            _mapper = mapper;
        }

        public async Task<Result> Handle(UpdateStaffCommand request, CancellationToken cancellationToken)
        {

            try
            {
                var user = await _identityService.GetUserByIdAndOrganization(request.UserId, request.OrganizationId);
                if (user.staff == null)
                {
                    return Result.Failure(new string[] { "Unable to change status.Invalid User ID and Organization credentials!" });
                }

                var getUserForUpdate = await _identityService.GetUserById(request.StaffId);
                if (getUserForUpdate.staff == null)
                {
                    return Result.Failure(new string[] { "Invalid Staff details for update" });
                }

                //var getUserForUpdate = await _identityService.GetUserByUsername(request.Email);
                //if (getUserForUpdate.staff == null)
                //{
                //    return Result.Failure(new string[] { "Staff for update does not exist with specified username" });
                //}

                getUserForUpdate.staff.Address = request.Address.Trim();
                getUserForUpdate.staff.LastModifiedDate = DateTime.Now;
                getUserForUpdate.staff.LastModifiedBy = user.staff.UserName;
                getUserForUpdate.staff.DateOfBirth = request.DateOfBirth;
                getUserForUpdate.staff.DepartmentId = request.DepartmentId;
                //getUserForUpdate.staff.Email = request.Email.Trim();
                getUserForUpdate.staff.FirstName = request.FirstName.Trim();
                getUserForUpdate.staff.Gender = request.Gender;
                getUserForUpdate.staff.LastName = request.LastName;
                getUserForUpdate.staff.MarketId = request.MarketId;
                getUserForUpdate.staff.MiddleName = request.MiddleName;
                getUserForUpdate.staff.OrganizationId = request.OrganizationId;
                getUserForUpdate.staff.PhoneNumber = request.PhoneNumber;
                getUserForUpdate.staff.SupervisorId = request.SupervisorId;
                getUserForUpdate.staff.EmploymentDate = request.EmploymentDate;
                getUserForUpdate.staff.RoleId = request.RoleId;
               // getUserForUpdate.staff.UserName = request.Email;
                getUserForUpdate.staff.Country = request.Country;
                getUserForUpdate.staff.State = request.State;
                if (!string.IsNullOrEmpty(getUserForUpdate.staff.MiddleName))
                {
                    getUserForUpdate.staff.Name = string.Concat(getUserForUpdate.staff.FirstName, " ", getUserForUpdate.staff.MiddleName, " ", getUserForUpdate.staff.LastName);
                }
                getUserForUpdate.staff.Name = string.Concat(getUserForUpdate.staff.FirstName, " ", getUserForUpdate.staff.LastName);
                await _identityService.UpdateStaffAsync(getUserForUpdate.staff);
                await _context.SaveChangesAsync(cancellationToken);
                var staffResult = _mapper.Map<StaffDto>(getUserForUpdate.staff);
                return Result.Success("Staff details updated successfully", staffResult);
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Error updating staff:", ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
