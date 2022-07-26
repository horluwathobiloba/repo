using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ProductServiceTypes.Commands.CreateProductServiceType
{
    public class CreateProductServiceTypeCommand : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public string Name { get; set; }
        public int VendorTypeId { get; set; }
        public string UserId { get; set; }
    }

    public class CreateProductServiceTypeCommandHandler : IRequestHandler<CreateProductServiceTypeCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        public CreateProductServiceTypeCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(CreateProductServiceTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                var exists = await _context.ProductServiceTypes.AnyAsync(x => x.OrganisationId == request.OrganisationId && x.Name == request.Name);
                if (exists)
                {
                    return Result.Failure($"Product service type name already exists!");
                }

                var entity = new ProductServiceType
                {
                    Name = request.Name,
                    OrganisationId = request.OrganisationId,
                    OrganisationName = _authService.Organisation?.Name,
                    VendorTypeId = request.VendorTypeId,

                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
                };

                await _context.ProductServiceTypes.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Product service type was created successfully");
            }
            catch (Exception ex)
            {
                return Result.Failure($"ProductServiceType creation failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }
}
