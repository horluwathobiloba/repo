using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.ProductServiceTypes.Queries.GetProductServiceTypes;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ProductServiceTypes.Commands.CreateProductServiceTypes
{
    public class CreateProductServiceTypesCommand : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public List<CreateProductServiceTypeRequest> ProductServiceTypes { get; set; }
        public string UserId { get; set; }
    }

    public class CreateProductServiceTypesCommandHandler : IRequestHandler<CreateProductServiceTypesCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreateProductServiceTypesCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(CreateProductServiceTypesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                var list = new List<ProductServiceType>();
                await _context.BeginTransactionAsync();

                foreach (var item in request.ProductServiceTypes)
                {
                    var exists = await _context.ProductServiceTypes.AnyAsync(x => x.OrganisationId == request.OrganisationId && x.Name == item.Name);
                    if (exists)
                    {
                        return Result.Failure($"Product service type name already exists!");
                    }

                    var entity = new ProductServiceType
                    {
                        Name = item.Name,
                        VendorTypeId = item.VendorTypeId,
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
                await _context.ProductServiceTypes.AddRangeAsync(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<ProductServiceTypeDto>>(list);
                return Result.Success("ProductServiceTypes created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"ProductServiceTypes creation failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
