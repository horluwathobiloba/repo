using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.ContractComments.Queries.GetContractComments;
using Onyx.ContractService.Application.ContractTag.Queries.GetContractTags;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using ReventInject;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractTags.Commands.CreateContractTag
{
    public class CreateContractTagCommand : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public int ContractId { get; set; }
       
        public string UserId { get; set; }
        public string Name { get; set; }
    }

    public class CreateContractTagCommandHandler : IRequestHandler<CreateContractTagCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreateContractTagCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _authService = authService;
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(CreateContractTagCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);


                var entity = new Domain.Entities.ContractTag
                {
                    ContractId = request.ContractId,
                    Name = request.Name,
                    OrganisationId = request.OrganisationId,
                    OrganisationName = _authService.Organisation.OrganisationName,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString(),
                };

                await _context.ContractTags.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<ContractTagDto>(entity);
                return Result.Success("Task tag created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Task tag creation failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
