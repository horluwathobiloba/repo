using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.Contracts.Commands.CreateContract;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using Onyx.ContractService.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.Contractaudit.Commands.CreateContractaudit
{
    public class CreateContractAuditLogCommand : AuthToken, IRequest<Result>
    {
        public string Module { get; set; }
        public string LastModifiedBy { get; set; }
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public string Action { get; set; }
        public string UserId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int JobFunctionId { get; set; }
        public string JobFunctionName { get; set; }
        public object NewValue { get; set; }
        public object OldValue { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }


    }
    public class CreateContractAuditLogCommandHandler : IRequestHandler<CreateContractAuditLogCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateContractAuditLogCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result> Handle(CreateContractAuditLogCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //create audit log for contract request
                var auditEntity = new AuditLog
                {
                    OrganisationId = request.OrganisationId,
                    OrganisationName = request.OrganisationName,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    RoleId = request.RoleId,
                    RoleName = request.RoleName,
                    JobFunctionId = request.JobFunctionId,
                    JobFunctionName = request.JobFunctionName,
                    Module = request.Module,
                    NewValue = JsonConvert.SerializeObject(request.NewValue),
                    Action = request.Action,
                    UserId = request.UserId,
                    OldValue = JsonConvert.SerializeObject(request.OldValue),
                    FirstName = request.FirstName,
                    LastName=request.LastName
                };

                await _context.AuditLogs.AddAsync(auditEntity);
                await _context.SaveChangesAsync(cancellationToken);


                return Result.Success("Contract audit created successfully!", auditEntity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Contract audit creation failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
