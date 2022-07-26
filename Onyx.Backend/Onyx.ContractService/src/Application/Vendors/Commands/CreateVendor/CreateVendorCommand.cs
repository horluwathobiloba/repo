using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.Vendors.Commands.CreateVendor
{
    public class CreateVendorCommand : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public string RCNumber { get; set; }
        public int VendorTypeId { get; set; }
        public string VendorCompanyName { get; set; }
        public string SupplierCode { get; set; }
        public string ShortName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhoneNumber { get; set; }

        public string SupplierClass { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public bool TouchPointVendor { get; set; }
        public string UserId { get; set; } 
    }

    public class CreateVendorCommandHandler : IRequestHandler<CreateVendorCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreateVendorCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(CreateVendorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                var exists = await _context.Vendors.AnyAsync(x => x.OrganisationId == request.OrganisationId && x.Name == request.VendorCompanyName);
                if (exists)
                {
                    return Result.Failure($"A record with the vendor name '{request.VendorCompanyName}' already exists!");
                }
                var entity = new Domain.Entities.Vendor
                {
                    SupplierClass = request.SupplierClass,
                    Address =  request.Address,
                    ContactName = request.ContactName,
                    ContactEmail = request.ContactEmail,
                    ContactPhoneNumber = request.ContactPhoneNumber,
                    Country = request.Country,
                    Email = request.Email,
                    Name = request.VendorCompanyName,
                    PhoneNumber = request.PhoneNumber,
                    RCNumber =  request.RCNumber,
                    SupplierCode = request.SupplierCode,
                    State = request.State,
                    ShortName = request.ShortName,
                    TouchPointVendor = request.TouchPointVendor,
                    VendorTypeId = request.VendorTypeId,
                    OrganisationId = request.OrganisationId,
                    OrganisationName =  _authService.Organisation.Name,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
                };

                await _context.Vendors.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Vendor created successfully!");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Vendor creation failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
