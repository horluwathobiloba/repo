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
    public class ContractReportCommand : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ContractStatus? ContractStatus { get; set; }
        public int? ProductTypeId { get; set; }
        public int? JobFunctionId { get; set; }
        public int? ContractTypeId { get; set; }
        public int? Top_N { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
    public class ContractReportCommandHandler : IRequestHandler<ContractReportCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public ContractReportCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(ContractReportCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                var userLists = await _authService.GetUsersAsync(request.AccessToken, request.OrganisationId);          
                if (userLists == null)
                {
                    return Result.Failure("Users do not exist with this organisation");
                }
                //everything is nullable , so send the default
                var reportList = new List<Domain.Entities.Contract>();
                var reportCount = new List<Domain.Entities.Contract>(); 
                if (!request.ContractStatus.HasValue && !request.ContractTypeId.HasValue && !request.StartDate.HasValue && !request.EndDate.HasValue
                    && !request.ProductTypeId.HasValue && !request.JobFunctionId.HasValue)
                {
                    reportList = await _context.Contracts.Where(a => a.OrganisationId == request.OrganisationId &&
            a.DocumentType == DocumentType.Contract).OrderByDescending(a => a.CreatedDate).Skip(request.Skip)
                                      .Take(request.Take).ToListAsync();
                    reportCount = await _context.Contracts.Where(a => a.OrganisationId == request.OrganisationId &&
            a.DocumentType == DocumentType.Contract).OrderByDescending(a => a.CreatedDate).ToListAsync();
                }
                else
                {
                    reportCount = await _context.Contracts.Where(a => a.OrganisationId == request.OrganisationId &&
            a.DocumentType == DocumentType.Contract &&
            (a.ContractStatus == request.ContractStatus || a.CreatedDate >= request.StartDate && a.CreatedDate <= request.EndDate || a.ProductServiceTypeId == request.ProductTypeId
             || a.ContractTypeId == request.ContractTypeId)).OrderByDescending(a => a.CreatedDate).ToListAsync();
                    reportList = await _context.Contracts.Where(a => a.OrganisationId == request.OrganisationId &&
            a.DocumentType == DocumentType.Contract &&
            (a.ContractStatus == request.ContractStatus || a.CreatedDate >= request.StartDate && a.CreatedDate <= request.EndDate || a.ProductServiceTypeId == request.ProductTypeId
             || a.ContractTypeId == request.ContractTypeId)).OrderByDescending(a => a.CreatedDate).Skip(request.Skip)
                                      .Take(request.Take).ToListAsync();
                }

                if ((reportList == null||reportList.Count==0) && request.JobFunctionId==0)
                {
                    return Result.Failure("No contract report with this search value is available");
                }

                List<UserDto> filteredUsersByJobFunction = new List<UserDto>();
                var report = await _context.Contracts.Where(a => a.OrganisationId == request.OrganisationId && a.DocumentType == DocumentType.Contract).OrderByDescending(a => a.CreatedDate).Skip(request.Skip)
                                      .Take(request.Take).ToListAsync();
                var reportListCount = await _context.Contracts.Where(a => a.OrganisationId == request.OrganisationId && a.DocumentType == DocumentType.Contract).OrderByDescending(a => a.CreatedDate).ToListAsync();
                var contractTypeDict = await _context.ContractTypes.Where(a => a.OrganisationId == request.OrganisationId).ToDictionaryAsync(a => a.Id);
                
                if (request.JobFunctionId > 0)
                {
                    filteredUsersByJobFunction = userLists.Entity.Where(a => a.JobFunctionId == request.JobFunctionId).ToList();
                    if (filteredUsersByJobFunction != null || filteredUsersByJobFunction.Count > 0)
                    {
                        //var filteredReport = reportList.Where(a => a.UserId.IN(filteredUsersByJobFunction.Select(a => a.UserId).ToList()));
                        var filteredReport = report.Where(a => a.CreatedBy.IN(filteredUsersByJobFunction.Select(a => a.UserId).ToList()));
                        var filteredReportCount = reportListCount.Where(a => a.CreatedBy.IN(filteredUsersByJobFunction.Select(a => a.UserId).ToList())).Count();
                        var filteredReportList = new List<Domain.Entities.Contract>();
                        foreach (var item in filteredReport)
                        {
                            if (contractTypeDict.TryGetValue(item.ContractTypeId, out Domain.Entities.ContractType existingContractType))
                            {
                                item.ContractType = existingContractType;
                            }
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
                    foreach (var item in filteredReport)
                    {

                        if (contractTypeDict.TryGetValue(item.ContractTypeId, out Domain.Entities.ContractType existingContractType))
                        {
                            item.ContractType = existingContractType;
                        }
                        var user = userLists.Entity.Where(a => a.UserId == item.CreatedBy).FirstOrDefault();
                        if ( user != null)
                        {
                            item.JobFunctionId = user.JobFunctionId;
                            item.JobFunctionName = user?.JobFunction?.Name;
                            item.CreatedBy = user?.FirstName + " " + user?.LastName;
                        }
                        filteredReportList.Add(item);
                    }

                    return Result.Success($"{filteredReportList.Count()} record(s) found", new { filteredReportList, filteredReportCount });
                }
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving contract reports . Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
