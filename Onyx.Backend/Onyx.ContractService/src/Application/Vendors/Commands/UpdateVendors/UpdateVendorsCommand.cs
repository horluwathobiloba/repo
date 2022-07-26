using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Domain.Entities;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.Vendors.Commands.UpdateVendors
{
    public class UpdateVendorsCommand : AuthToken,IRequest<Result>
    {
        public List<UpdateVendorRequest> Vendors { get; set; }
        public int OrganisationId { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateVendorsCommandHandler : IRequestHandler<UpdateVendorsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper; 
        private readonly IAuthService _authService;          

        public UpdateVendorsCommandHandler(IApplicationDbContext context, IMapper mapper,IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(UpdateVendorsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                var list = new List<Vendor>();

                await _context.BeginTransactionAsync();

                foreach (var item in request.Vendors)
                {
                    var entity = await _context.Vendors.FirstOrDefaultAsync(x => x.OrganisationId == request.OrganisationId && x.Id == item.Id);
                    if (entity == null)
                    {
                        return Result.Failure($"Invalid vendor specified.");
                    }

                    //check if the name of the vendor already exists and conflicts with this new name 
                    var UpdatedEntityExists = await _context.Vendors.AnyAsync(x => x.OrganisationId == request.OrganisationId && x.Id != item.Id
                    && x.Name.ToLower() == item.VendorCompanyName.ToLower());

                    if (UpdatedEntityExists)
                    {
                        return Result.Failure($"A record with this vendor name {item.VendorCompanyName} already exists. Please change the name.");
                    }

                    entity.Address = item.Address;
                    entity.ShortName = item.ShortName;
                    entity.ContactName = item.ContactName;
                    entity.ContactEmail = item.ContactEmail;
                    entity.ContactPhoneNumber = item.ContactPhoneNumber;
                    entity.Country = item.Country;
                    entity.Email = item.Email;
                    entity.Name = item.VendorCompanyName;
                    entity.PhoneNumber = item.PhoneNumber;
                    entity.SupplierClass = item.SupplierClass;
                    entity.State = item.State;
                    entity.SupplierCode = item.SupplierCode;
                    entity.TouchPointVendor = item.TouchPointVendor;
                    entity.VendorTypeId = item.VendorTypeId; 

                    entity.LastModifiedBy = request.UserId;
                    entity.LastModifiedDate = DateTime.Now;
                    list.Add(entity);
                }

                _context.Vendors.UpdateRange(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                return Result.Success("Vendors update was successful!");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Vendors update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }


    }


}
