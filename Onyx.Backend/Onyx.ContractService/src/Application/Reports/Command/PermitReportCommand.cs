using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Exceptions;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.Contracts.Queries.GetContracts;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.Reports.Command
{
    public class PermitReportCommand : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ContractStatus? ContractStatus { get; set; }
        public int? ProductTypeId { get; set; }
        public int? JobFunctionId { get; set; }
        public int? PermitTypeId { get; set; }
        public int? Top_N { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }

    }
    public class PermitReportCommandHandler : IRequestHandler<PermitReportCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public PermitReportCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(PermitReportCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                var userLists = await _authService.GetUsersAsync(request.AccessToken, request.OrganisationId);
                if (userLists == null)
                {
                    return Result.Failure("Users do not exist on the organisation");
                }


                var reportList = new List<Domain.Entities.Contract>();
                var reportCount = new List<Domain.Entities.Contract>();
                if (!request.ContractStatus.HasValue && !request.PermitTypeId.HasValue && !request.StartDate.HasValue && !request.EndDate.HasValue
                   && !request.ProductTypeId.HasValue && !request.JobFunctionId.HasValue)
                {
                    reportCount = await _context.Contracts.Where(a => a.OrganisationId == request.OrganisationId &&
            a.DocumentType == DocumentType.Permit).OrderByDescending(a => a.CreatedDate).ToListAsync();
                    reportList = await _context.Contracts.Where(a => a.OrganisationId == request.OrganisationId &&
            a.DocumentType == DocumentType.Permit).OrderByDescending(a => a.CreatedDate).Skip(request.Skip)
                                      .Take(request.Take).ToListAsync();
                }
                else
                {
                    //get the count of it
                    reportCount = await _context.Contracts.Where(a => a.OrganisationId == request.OrganisationId &&
              a.DocumentType == DocumentType.Permit &&
              (a.ContractStatus == request.ContractStatus || a.CreatedDate >= request.StartDate &&
               a.CreatedDate <= request.EndDate || a.ProductServiceTypeId == request.ProductTypeId
               || a.PermitTypeId == request.PermitTypeId)).OrderByDescending(a => a.CreatedDate).ToListAsync();

                    reportList = await _context.Contracts.Where(a => a.OrganisationId == request.OrganisationId &&
              a.DocumentType == DocumentType.Permit &&
              (a.ContractStatus == request.ContractStatus || a.CreatedDate >= request.StartDate &&
               a.CreatedDate <= request.EndDate || a.ProductServiceTypeId == request.ProductTypeId
               || a.PermitTypeId == request.PermitTypeId)).OrderByDescending(a => a.CreatedDate).Skip(request.Skip)
                                      .Take(request.Take).ToListAsync();
                }
                  
                if ((reportList == null || reportList.Count==0)&& request.JobFunctionId == 0)
                {
                   return Result.Failure("No permit report with this search value available");
                }


                List<UserDto> filteredUsersByJobFunction = new List<UserDto>();
                var report = await _context.Contracts.Where(a => a.OrganisationId == request.OrganisationId && a.DocumentType == DocumentType.Permit).OrderByDescending(a => a.CreatedDate).Skip(request.Skip)
                                      .Take(request.Take).ToListAsync();
                var reportListCount = await _context.Contracts.Where(a => a.OrganisationId == request.OrganisationId && a.DocumentType == DocumentType.Permit).OrderByDescending(a => a.CreatedDate).ToListAsync();
                var contractTypeDict = await _context.PermitTypes.Where(a => a.OrganisationId == request.OrganisationId).ToDictionaryAsync(a => a.Id);
                if (request.JobFunctionId>0)
                {
                    filteredUsersByJobFunction = userLists.Entity.Where(a => a.JobFunctionId == request.JobFunctionId).ToList();
                    var filteredReportList = new List<Domain.Entities.Contract>();
                    if (filteredUsersByJobFunction != null || filteredUsersByJobFunction.Count > 0)
                    {
                        var filteredReport = report.Where(a => a.CreatedBy.IN(filteredUsersByJobFunction.Select(a => a.UserId).ToList()));
                        var filteredReportCount = reportListCount.Where(a => a.CreatedBy.IN(filteredUsersByJobFunction.Select(a => a.UserId).ToList())).Count();
                        foreach (var item in filteredReport)
                        {
                            if (contractTypeDict.TryGetValue(item.ContractTypeId, out Domain.Entities.PermitType permit))
                            {
                                item.PermitType = permit;
                            };
                            item.JobFunctionName = filteredUsersByJobFunction.FirstOrDefault()?.JobFunction?.Name;
                            item.CreatedBy = userLists.Entity.Where(a => a.UserId == item.LastModifiedBy).FirstOrDefault()?.FirstName + " " + userLists.Entity.Where(a => a.UserId == item.LastModifiedBy).FirstOrDefault()?.LastName;
                            filteredReportList.Add(item);
                        }
                        return Result.Success($"{filteredReportList.Count()} record(s) found", new { filteredReportList, filteredReportCount });
                        
                    }
                    return Result.Failure($"No report exists with specified job function id");
                }
                else
                {
                    //filter by organisation 
                    var filteredReport = reportList.Where(a => a.CreatedBy.IN(userLists.Entity.Select(a => a.UserId).ToList()));
                    var filteredReportCount = reportCount.Where(a => a.CreatedBy.IN(userLists.Entity.Select(a => a.UserId).ToList())).Count();
                    var filteredReportList = new List<Domain.Entities.Contract>();
                    foreach (var item in reportList)
                    {

                        if (contractTypeDict.TryGetValue(item.ContractTypeId, out Domain.Entities.PermitType permit))
                        {
                            item.PermitType = permit;
                        }
                        item.JobFunctionName = userLists.Entity.Where(a => a.UserId == item.CreatedBy).FirstOrDefault()?.JobFunction?.Name;;
                        item.CreatedBy = userLists.Entity.Where(a => a.UserId == item.LastModifiedBy).FirstOrDefault()?.FirstName + " " + userLists.Entity.Where(a => a.UserId == item.LastModifiedBy).FirstOrDefault()?.LastName;
                        filteredReportList.Add(item);
                    }

                    return Result.Success($"{filteredReportList.Count()} record(s) found", new { filteredReportList, filteredReportCount });
                }
                //return Result.Success($"{reportList.Count()} record(s) found",reportList);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving contract reports . Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
