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
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ProductServiceTypes.Commands.UpdateProductServiceTypes
{
    public class UpdateProductServiceTypesCommand : AuthToken, IRequest<Result>
    {
        public List<UpdateProductServiceTypeRequest> ProductServiceTypes { get; set; }
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateProductServiceTypesCommandHandler : IRequestHandler<UpdateProductServiceTypesCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UpdateProductServiceTypesCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(UpdateProductServiceTypesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                var list = new List<ProductServiceType>();

                await _context.BeginTransactionAsync();

                foreach (var item in request.ProductServiceTypes)
                {
                    //check if the name of the product service type already exists and conflicts with this new name 
                    var UpdatedEntityExists = await _context.ProductServiceTypes
                        .AnyAsync(x => x.OrganisationId == request.OrganisationId && x.Id != item.Id
                        && x.Name.ToLower() == item.Name.ToLower());

                    if (UpdatedEntityExists)
                    {
                        return Result.Failure($"Another product service type named {item.Name} already exists. Please change the name and try again.");
                    }
                    var entity = await _context.ProductServiceTypes.Where(x => x.OrganisationId == request.OrganisationId && x.Id == item.Id)
                                           .FirstOrDefaultAsync();

                    if (entity == null)
                    {
                        entity = new ProductServiceType
                        {
                            Name = item.Name,
                            OrganisationId = request.OrganisationId,
                            OrganisationName = request.OrganisationName,
                            VendorTypeId = item.VendorTypeId,

                            CreatedBy = request.UserId,
                            CreatedDate = DateTime.Now,
                            LastModifiedBy = request.UserId,
                            LastModifiedDate = DateTime.Now,
                            Status = Status.Active,
                            StatusDesc = Status.Active.ToString()
                        };
                    }
                    else
                    {
                        entity.Name = item.Name;
                        entity.VendorTypeId = item.VendorTypeId;
                        entity.LastModifiedBy = request.UserId;
                        entity.LastModifiedDate = DateTime.Now;
                    }
                    list.Add(entity);
                }

                _context.ProductServiceTypes.UpdateRange(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<ProductServiceTypeDto>>(list);
                return Result.Success("Product service types update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Product service type update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }


    }


}
