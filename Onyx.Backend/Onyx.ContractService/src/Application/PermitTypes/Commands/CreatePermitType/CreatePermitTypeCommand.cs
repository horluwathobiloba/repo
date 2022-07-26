using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.PermitTypes.Queries.GetPermitTypes;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using ReventInject;
using System;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.PermitTypes.Commands.CreatePermitType
{
    public class CreatePermitTypeCommand : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; } 
    }

    public class CreatePermitTypeCommandHandler : IRequestHandler<CreatePermitTypeCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService; 

        public CreatePermitTypeCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _authService = authService;            
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(CreatePermitTypeCommand request, CancellationToken cancellationToken)
        {
            try
            { 
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);              
                
                var exists = await _context.PermitTypes.AnyAsync(x => x.OrganisationId == request.OrganisationId && x.Name.ToLower().Trim() == request.Name.ToLower().Trim());

                if (exists)
                {
                    return Result.Failure($"Permit type name already exists!");
                }

                var entity = new Domain.Entities.PermitType
                {
                    Name = request.Name,
                    OrganisationId = request.OrganisationId,
                    OrganisationName = _authService.Organisation?.Name,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
                };

                await _context.PermitTypes.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<PermitTypeDto>(entity);
                return Result.Success("PermitType created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"PermitType creation failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
