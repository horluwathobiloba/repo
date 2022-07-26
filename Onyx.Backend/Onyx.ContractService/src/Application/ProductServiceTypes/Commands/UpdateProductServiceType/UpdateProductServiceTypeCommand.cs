using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ReventInject;

namespace Onyx.ContractService.Application.ProductServiceTypes.Commands.UpdateProductServiceType
{
    public class UpdateProductServiceTypeCommand : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public int OrganisationId { get; set; }
        public string Name { get; set; }
        public int VendorTypeId { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateProductServiceTypeCommandHandler : IRequestHandler<UpdateProductServiceTypeCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UpdateProductServiceTypeCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(UpdateProductServiceTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                //check if the name of the other record has the new name
                var UpdatedNameExists = await _context.ProductServiceTypes.AnyAsync(x => x.OrganisationId == request.OrganisationId && x.Id != request.Id
                && x.Name.ToLower() == request.Name.ToLower());

                if (UpdatedNameExists)
                {
                    return Result.Failure($"A record with this product service type name {request.Name} already exists. Please change the name.");
                }

                var entity = await _context.ProductServiceTypes.FirstOrDefaultAsync(x => x.OrganisationId == request.OrganisationId && x.Id == request.Id);
                if (entity == null)
                {
                    return Result.Failure($"Invalid product service type specified.");
                }
                entity.Name = request.Name;
                entity.VendorTypeId = request.VendorTypeId;
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;

                _context.ProductServiceTypes.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Product was updated successfully");
            }
            catch (Exception ex)
            {
                return Result.Failure($"product service type update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }


    }

}
