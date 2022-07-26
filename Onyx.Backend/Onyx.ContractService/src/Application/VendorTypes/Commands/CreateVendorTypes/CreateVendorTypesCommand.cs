using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.VendorTypes.Queries.GetVendorTypes;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.VendorTypes.Commands.CreateVendorTypes
{
    public class CreateVendorTypesCommand : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public List<string> VendorTypeNames { get; set; }
        public string UserId { get; set; }
    }

    public class CreateVendorTypesCommandHandler : IRequestHandler<CreateVendorTypesCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreateVendorTypesCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(CreateVendorTypesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);

                var list = new List<VendorType>();

                await _context.BeginTransactionAsync();

                foreach (var item in request.VendorTypeNames)
                {
                    var exists = await _context.VendorTypes.AnyAsync(x => x.OrganisationId == request.OrganisationId && x.Name.ToLower().Trim() == item.ToLower().Trim());
                    if (exists)
                    {
                        return Result.Failure($"Vendor type name '{item}' already exists!");
                    }
                    var entity = new VendorType
                    {
                        Name = item,
                        OrganisationId = request.OrganisationId,
                        OrganisationName = request.OrganisationName,
                        CreatedBy = request.UserId,
                        CreatedDate = DateTime.Now,
                        LastModifiedBy = request.UserId,
                        LastModifiedDate = DateTime.Now,
                        Status = Status.Active,
                        StatusDesc = Status.Active.ToString()
                    };
                    list.Add(entity);
                }
                await _context.VendorTypes.AddRangeAsync(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<VendorTypeDto>>(list);
                return Result.Success("VendorTypes created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"VendorTypes creation failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
