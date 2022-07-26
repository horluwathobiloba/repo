using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.LicenseTypes.Queries.GetLicenseTypes;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.LicenseTypes.Commands.CreateLicenseTypes
{
    public class CreateLicenseTypesCommand : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public List<string> LicenseTypeNames { get; set; }
        public string UserId { get; set; }
    }

    public class CreateLicenseTypesCommandHandler : IRequestHandler<CreateLicenseTypesCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreateLicenseTypesCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(CreateLicenseTypesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);

                var list = new List<LicenseType>();

                await _context.BeginTransactionAsync();

                foreach (var item in request.LicenseTypeNames)
                {
                    var exists = await _context.LicenseTypes.AnyAsync(x => x.OrganisationId == request.OrganisationId && x.Name.ToLower().Trim() == item.ToLower().Trim());
                    if (exists)
                    {
                        return Result.Failure($"License type name '{item}' already exists!");
                    }
                    var entity = new LicenseType
                    {
                        Name = item,
                        OrganisationId = request.OrganisationId,
                        OrganisationName = _authService.Organisation?.Name,
                        CreatedBy = request.UserId,
                        CreatedDate = DateTime.Now,
                        LastModifiedBy = request.UserId,
                        LastModifiedDate = DateTime.Now,
                        Status = Status.Active,
                        StatusDesc = Status.Active.ToString()
                    };
                    list.Add(entity);
                }
                await _context.LicenseTypes.AddRangeAsync(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<LicenseTypeDto>>(list);
                return Result.Success("LicenseTypes created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"LicenseTypes creation failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
