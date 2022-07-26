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
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.VendorTypes.Commands.UpdateVendorTypes
{
    public class UpdateVendorTypesCommand : AuthToken, IRequest<Result>
    {
        public List<UpdateVendorTypeRequest> VendorTypes { get; set; }
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateVendorTypesCommandHandler : IRequestHandler<UpdateVendorTypesCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;


        public UpdateVendorTypesCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(UpdateVendorTypesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                var list = new List<VendorType>();
                await _context.BeginTransactionAsync();

                foreach (var item in request.VendorTypes)
                {
                    //check if the name of the vendor type already exists and conflicts with this new name 
                    var UpdatedEntityExists = await _context.VendorTypes
                        .AnyAsync(x => x.OrganisationId == request.OrganisationId && x.Id != item.Id && x.Name.ToLower().Trim() == item.Name.ToLower().Trim());

                    if (UpdatedEntityExists)
                    {
                        return Result.Failure($"Another vendor type named {item.Name} already exists. Please change the name and try again.");
                    }
                    var entity = await _context.VendorTypes.Where(x => x.OrganisationId == request.OrganisationId && x.Id == item.Id)
                                           .FirstOrDefaultAsync();

                    if (entity == null)
                    {
                        entity = new VendorType
                        {
                            Name = item.Name,
                            OrganisationId = request.OrganisationId,
                            OrganisationName =  _authService.Organisation?.OrganisationName,
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
                        entity.LastModifiedBy = request.UserId;
                        entity.LastModifiedDate = DateTime.Now;
                    }
                    list.Add(entity);
                }

                _context.VendorTypes.UpdateRange(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<VendorTypeDto>>(list);
                return Result.Success("Vendor types update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"VendorType update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }


    }


}
