using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.Vendors.Commands.CreateVendors
{
    public class CreateVendorsCommand : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }

        public List<CreateVendorRequest> Vendors { get; set; }
        public string UserId { get; set; }
    }

    public class CreateVendorsCommandHandler : IRequestHandler<CreateVendorsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreateVendorsCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(CreateVendorsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                var list = new List<Vendor>();

                await _context.BeginTransactionAsync();

                foreach (var vendor in request.Vendors)
                {
                    var exists = await _context.Vendors.AnyAsync(x => x.OrganisationId == request.OrganisationId && x.Name.ToLower() == vendor.VendorCompanyName.ToLower());

                    if (exists)
                    {
                        return Result.Failure($"A record with the vendor name '{vendor.VendorCompanyName}' already exists!");
                    }

                    var entity = new Vendor
                    {
                        Address =  vendor.Address,
                        SupplierClass = vendor.SupplierClass,
                        ContactName = vendor.ContactName,
                        ContactEmail = vendor.ContactEmail,
                        ContactPhoneNumber = vendor.ContactPhoneNumber,
                        Country = vendor.Country,
                        Email = vendor.Email,
                        Name = vendor.VendorCompanyName,
                        PhoneNumber = vendor.PhoneNumber,
                        SupplierCode = vendor.SupplierCode,
                        State = vendor.State,
                        ShortName = vendor.ShortName,
                        TouchPointVendor = vendor.TouchPointVendor,
                        VendorTypeId = vendor.VendorTypeId,
                        OrganisationId = request.OrganisationId,
                        OrganisationName = _authService.Organisation.Name,
                        CreatedBy = request.UserId,
                        CreatedDate = DateTime.Now,
                        LastModifiedBy = request.UserId,
                        LastModifiedDate = DateTime.Now,
                        Status = Status.Active,
                        StatusDesc = Status.Active.ToString()
                    };
                    list.Add(entity);
                }
                await _context.Vendors.AddRangeAsync(list);
                await _context.SaveChangesAsync(cancellationToken);

                await _context.CommitTransactionAsync();

                return Result.Success("Vendors created successfully!");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Vendors creation failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
