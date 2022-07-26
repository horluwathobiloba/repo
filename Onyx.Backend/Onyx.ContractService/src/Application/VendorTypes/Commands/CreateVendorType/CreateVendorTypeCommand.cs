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
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.VendorTypes.Commands.CreateVendorType
{
    public class CreateVendorTypeCommand : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; } 
    }

    public class CreateVendorTypeCommandHandler : IRequestHandler<CreateVendorTypeCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
         

        public CreateVendorTypeCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _authService = authService;            
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(CreateVendorTypeCommand request, CancellationToken cancellationToken)
        {
            try
            { 
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);              
                
                var exists = await _context.VendorTypes.AnyAsync(x => x.OrganisationId == request.OrganisationId && x.Name.ToLower().Trim() == request.Name.ToLower().Trim());

                if (exists)
                {
                    return Result.Failure($"Vendor type name already exists!");
                }

                var entity = new Domain.Entities.VendorType
                {
                    Name = request.Name,
                    OrganisationId = request.OrganisationId,
                    OrganisationName = _authService.Organisation.OrganisationName,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
                };

                await _context.VendorTypes.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<VendorTypeDto>(entity);
                return Result.Success("VendorType created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"VendorType creation failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}

//var orgRresponse = await _authService.GetOrganisationAsync(request.OrganisationId);
                //var userRresponse = await _authService.GetUserAsync(request.UserId);

                //if (orgRresponse == null || !orgRresponse.Succeeded || orgRresponse.Entity == null)
                //{
                //    return Result.Failure($"Invalid organisation specified!");
                //}
                //var org = orgRresponse.Entity;
                //if (org.Name.ToLower() != request.OrganisationName.ToLower())
                //{
                //    return Result.Failure($"Invalid organisation name specified!");
                //}

                //if (userRresponse == null || !userRresponse.Succeeded || userRresponse.Entity.UserId != request.UserId)
                //{
                //    return Result.Failure($"Invalid user specified!");
                //}