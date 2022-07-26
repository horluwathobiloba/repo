using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.ContractTag.Queries.GetContractTags;
using Onyx.ContractService.Application.ContractTags.Queries.GetContractTags;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractTags.Commands.CreateContractTags
{
    public class CreateContractTagsCommand : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public List<string> ContractTagNames { get; set; }
        public string UserId { get; set; }
        public int ContractId { get; set; }
    }

    public class CreateContractTagsCommandHandler : IRequestHandler<CreateContractTagsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreateContractTagsCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(CreateContractTagsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);

                var list = new List<Domain.Entities.ContractTag>();

                await _context.BeginTransactionAsync();

                foreach (var item in request.ContractTagNames)
                {
                    var exists = await _context.ContractTags.AnyAsync(x => x.OrganisationId == request.OrganisationId && x.Name.ToLower().Trim() == item.ToLower().Trim());
                    if (exists)
                    {
                        return Result.Failure($"Vendor type name '{item}' already exists!");
                    }
                    var entity = new Domain.Entities.ContractTag
                    {
                        ContractId = request.ContractId,
                        Name = item,
                        OrganisationId = request.OrganisationId,
                        OrganisationName = request.OrganisationName,
                        CreatedBy = request.UserId,
                        CreatedDate = DateTime.Now,
                        LastModifiedBy = request.UserId,
                        LastModifiedDate = DateTime.Now,
                        Status = Status.Active,
                        StatusDesc = Status.Active.ToString(),
                    };
                    list.Add(entity);
                }
                await _context.ContractTags.AddRangeAsync(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<ContractTagDto>>(list);
                return Result.Success("ContractTags created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"ContractTags creation failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
