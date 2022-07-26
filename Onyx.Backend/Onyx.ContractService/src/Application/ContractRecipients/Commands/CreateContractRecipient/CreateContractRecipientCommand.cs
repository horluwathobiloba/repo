using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.ContractRecipients.Queries.GetContractRecipients;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using Onyx.ContractService.Infrastructure.Services;
using ReventInject;
using ReventInject.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractRecipients.Commands.CreateContractRecipient
{
    public class CreateContractRecipientCommand : IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }

        public int ContractId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public int Rank { get; set; }

        public RecipientCategory RecipientCategory { get; set; }
        public string UserId { get; set; }
    }


    public class CreateContractRecipientCommandHandler : IRequestHandler<CreateContractRecipientCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateContractRecipientCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(CreateContractRecipientCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var exists = await _context.ContractRecipients
                    .AnyAsync(x => x.OrganisationId == request.OrganisationId && x.ContractId == request.ContractId
                    && x.Email.ToLower() == request.Email.ToLower());

                if (exists)
                {
                    return Result.Failure($"Contract recipient already exists for({request.Email})!");
                }

                var rankExists = await _context.ContractRecipients
                    .AnyAsync(a => a.OrganisationId == request.OrganisationId && a.ContractId == request.ContractId && a.Rank == request.Rank);

                var rank = 0; // get the last ranking if the requested rank already exist
                if (rankExists)
                {
                    var maxRank = await _context.ContractRecipients
                         .Where(a => a.OrganisationId == request.OrganisationId && a.ContractId == request.ContractId)
                         .MaxAsync(r => r.Rank);
                    rank = maxRank + 1;
                }
                else
                {
                    rank = request.Rank;
                }
                var entity = new ContractRecipient
                {
                    OrganisationId = request.OrganisationId,
                    OrganisationName = request.OrganisationName,
                    ContractId = request.ContractId,
                    Email = request.Email,
                    RecipientCategory = request.RecipientCategory.ToString(),
                    Designation = request.Designation,
                   // Name = request.Name,
                    Rank = rank,

                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
                };

                await _context.ContractRecipients.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<ContractRecipientDto>(entity);
                return Result.Success("Contract recipient was created successfully", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Contract recipient creation failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message} ");
            }
        }
    }

}

