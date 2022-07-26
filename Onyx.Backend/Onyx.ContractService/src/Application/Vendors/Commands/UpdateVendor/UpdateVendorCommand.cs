using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.Vendors.Commands.UpdateVendor
{
    public class UpdateVendorCommand :AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public int VendorTypeId { get; set; }
        public string VendorCompanyName { get; set; }
        public string SupplierCode { get; set; }
        public string ShortName { get; set; }
        public string SupplierClass { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhoneNumber { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public bool TouchPointVendor { get; set; }
        public int OrganisationId { get; set; }
        public string UserId { get; set; }
        public string RCNumber { get; set; }
    }

    public class UpdateVendorCommandHandler : IRequestHandler<UpdateVendorCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UpdateVendorCommandHandler(IApplicationDbContext context, IMapper mapper,IAuthService authService)
        {
            _context = context;
            _mapper = mapper; 
         _authService = authService;
    }
        public async Task<Result> Handle(UpdateVendorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);

                var entity = await _context.Vendors.FirstOrDefaultAsync(x => x.OrganisationId == request.OrganisationId && x.Id == request.Id);
                if (entity == null)
                {
                    return Result.Failure($"Invalid vendor specified for update.");
                }

                //check if the name of the service types are
                var UpdatedEntityExists = await _context.Vendors.AnyAsync(x => x.OrganisationId == request.OrganisationId && x.Id != request.Id
                && x.Name.ToLower() == request.VendorCompanyName.ToLower());

                if (UpdatedEntityExists)
                {
                    return Result.Failure($"A record with this vendor name {request.VendorCompanyName} already exists. Please change the name.");
                }

                entity.Address = request.Address;
                entity.SupplierCode = request.SupplierCode;
                entity.ContactName = request.ContactName;
                entity.ContactEmail = request.ContactEmail;
                entity.ContactPhoneNumber = request.ContactPhoneNumber;
                entity.Country = request.Country;
                entity.Email = request.Email;
                entity.Name = request.VendorCompanyName;
                entity.PhoneNumber = request.PhoneNumber;
                entity.SupplierClass = request.SupplierClass;
                entity.State = request.State;
                entity.ShortName = request.ShortName;
                entity.TouchPointVendor = request.TouchPointVendor;
                entity.VendorTypeId = request.VendorTypeId;
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;
                entity.RCNumber = request.RCNumber;
                entity.Address = request.Address;
                _context.Vendors.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Vendor updated was successful!");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Vendor update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }


    }


}
