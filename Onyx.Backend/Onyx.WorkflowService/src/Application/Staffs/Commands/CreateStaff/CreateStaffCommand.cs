using Onyx.WorkFlowService.Application.Common.Interfaces;
using Onyx.WorkFlowService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Onyx.WorkFlowService.Domain.Enums;
using System;
using Onyx.WorkFlowService.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AutoMapper;
using Onyx.WorkFlowService.Application.Staffs.Queries.GetStaffs;

namespace Onyx.WorkFlowService.Application.Staffs.Commands.CreateStaff
{
    public class CreateStaffCommand : IRequest<Result>
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
        public int MarketId { get; set; }
        public int RoleId { get; set; }
        public string Password { get; set; }
        public string CreatedBy { get; set; }
        public int DepartmentId { get; set; }
        public string SupervisorId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime EmploymentDate { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
    }

    public class CreateStaffCommandHandler : IRequestHandler<CreateStaffCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public CreateStaffCommandHandler(IApplicationDbContext context, IIdentityService identityService, IMapper mapper)
        {
            _context = context;
            _identityService = identityService;
            _mapper = mapper;
        }

        public async Task<Result> Handle(CreateStaffCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var createdBy ="";
                var user = await _identityService.GetUserByIdAndOrganization(request.UserId, request.OrganizationId);
                //check userid only if default users exist for an organization, this would bypass validation of user on admin creation for organization
                var checkUsers = await _identityService.GetStaffsByOrganizationId(request.OrganizationId);
                if (checkUsers.staffs.Any())
                { 
                    if (user.staff == null)
                    {
                        return Result.Failure(new string[] { "Unable to create staff.Invalid User ID and Organization credentials!" });
                    }
                    else
                    {
                        createdBy = user.staff.Email;
                    }
                }
                else
                {
                    createdBy = request.CreatedBy;
                }
               
               
                var staffDetail = await _identityService.GetUserByUsername(request.Email);
                if (staffDetail.staff != null)
                {
                    return Result.Failure(new string[] { "Staff already exists with this email" });
                }
                var lastCount = await _context.StaffCount.Where(a => a.OrganizationId == request.OrganizationId).FirstOrDefaultAsync();
                if (lastCount == null)
                {
                    lastCount = new StaffCount();
                    lastCount.Count = 0;
                    lastCount.OrganizationId = request.OrganizationId;
                }
                var staff = new Staff
                {
                    Address = request.Address.Trim(),
                    DateOfBirth = request.DateOfBirth,
                    DepartmentId = request.DepartmentId,
                    Email = request.Email.Trim(),
                    FirstName = request.FirstName.Trim(),
                    Gender = request.Gender,
                    LastName = request.LastName.Trim(),
                    MarketId = request.MarketId,
                    MiddleName = request.MiddleName.Trim(),
                    OrganizationId = request.OrganizationId,
                    Password = request.Password.Trim(),
                    SupervisorId = request.SupervisorId,
                    UserName = request.Email.Trim(),
                    RoleId = request.RoleId,
                    Status  = Status.Inactive,
                    CreatedDate= DateTime.Now,
                    CreatedBy= createdBy,
                    Country = request.Country,
                    State = request.State,
                    EmploymentDate = request.EmploymentDate
                };
                if (!string.IsNullOrEmpty(staff.MiddleName))
                {
                    staff.Name = string.Concat(staff.FirstName, " ", staff.MiddleName, " ", staff.LastName);
                }
                staff.Name = string.Concat(staff.FirstName, " ", staff.LastName);
                lastCount.Count += 1;
                staff.StaffCode = "00" + lastCount.Count;
               var result =  await _identityService.CreateStaffAsync(staff);
                if (!result.Result.Succeeded )
                {
                    return Result.Failure( result.Result.Messages );
                }
                 _context.StaffCount.Update(lastCount);
                await _context.SaveChangesAsync(cancellationToken);
                staff.StaffId = result.UserId;
                var staffResult =  _mapper.Map<StaffDto>(staff);
                return Result.Success( "Staff creation was successful", staffResult);
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Staff creation was not successful", ex?.Message??ex?.InnerException.Message });
            }

        }
    }
}
