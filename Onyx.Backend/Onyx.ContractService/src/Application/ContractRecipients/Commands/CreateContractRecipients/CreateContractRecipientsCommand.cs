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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractRecipients.Commands.CreateContractRecipients
{
    public class CreateContractRecipientsCommand : IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public int ContractId { get; set; }
        public List<CreateContractRecipientRequest> ContractRecipients { get; set; }
        public string UserId { get; set; }
    }

    public class CreateContractRecipientsCommandHandler : IRequestHandler<CreateContractRecipientsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        // private readonly IMediator _mediator;
        //  private readonly IBlobStorageService _blobService;

        public CreateContractRecipientsCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            //_mediator = mediator;
            //_blobService = blobService;
        }
        public async Task<Result> Handle(CreateContractRecipientsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await CreateRecipients(request, cancellationToken);             
            }
            catch (Exception ex)
            {
                return Result.Failure($"Contract recipients creation failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        public async Task<Result> CreateRecipients(CreateContractRecipientsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var list = new List<ContractRecipient>();

                await _context.BeginTransactionAsync();

                foreach (var item in request.ContractRecipients)
                {
                    var exists = await _context.ContractRecipients
                   .AnyAsync(x => x.OrganisationId == request.OrganisationId && x.ContractId == request.ContractId
                   && x.Email.ToLower() == item.Email.ToLower() && x.RecipientCategory == item.RecipientCategory.ToString());

                    if (exists)
                    {
                        return Result.Failure($"Contract recipient already exists for {item.Email} ({item.Email})!");
                    }
                    //if (string.IsNullOrEmpty(item.Name))
                    //{
                    //    return Result.Failure($"Invalid recipient name specified for {item.Email}.");
                    //}
                    if (string.IsNullOrEmpty(item.Email))
                    {
                        return Result.Failure($"Invalid recipient email specified for {item.Email}.");
                    }
                    if (item.Rank <= 0)
                    {
                        return Result.Failure($"Invalid rank specified for {item.Email}.");
                    }
                    if (item.RecipientCategory.IsEnum<RecipientCategory>() == false)
                    {
                        return Result.Failure($"Invalid recipient category specified for {item.Email}.");
                    }
                    var rank = 0; // get the last ranking if the requested rank already exist
                    bool rankExists;
                    if (list != null && list.Count > 0)
                    {
                        rankExists = list.Any(a => a.OrganisationId == request.OrganisationId && a.ContractId == request.ContractId && a.Rank == item.Rank);
                        if (rankExists)
                        {
                            var maxRank = list.Where(a => a.OrganisationId == request.OrganisationId && a.ContractId == request.ContractId)
                                 .Max(r => r.Rank);
                            rank = maxRank + 1;
                        }
                        else
                        {
                            rank = item.Rank;
                        }
                    }
                    else
                    {
                        rankExists = await _context.ContractRecipients
                     .AnyAsync(a => a.OrganisationId == request.OrganisationId && a.ContractId == request.ContractId && a.Rank == item.Rank);
                        if (rankExists)
                        {
                            var maxRank = await _context.ContractRecipients
                                 .Where(a => a.OrganisationId == request.OrganisationId && a.ContractId == request.ContractId)
                                 .MaxAsync(r => r.Rank);
                            rank = maxRank + 1;
                        }
                        else
                        {
                            rank = item.Rank;
                        }
                    }

                    var entity = new ContractRecipient
                    {
                        OrganisationId = request.OrganisationId,
                        OrganisationName = request.OrganisationName,
                        ContractId = request.ContractId,
                        Email = item.Email,
                        Designation = item.Designation,
                        Rank = rank,
                        RecipientCategory = item.RecipientCategory.ToString(),

                        CreatedBy = request.UserId,
                        CreatedDate = DateTime.Now,
                        LastModifiedBy = request.UserId,
                        LastModifiedDate = DateTime.Now,
                        Status = Status.Active,
                        StatusDesc = Status.Active.ToString()
                    };
                    list.Add(entity);
                }
                await _context.ContractRecipients.AddRangeAsync(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<ContractRecipientDto>>(list);
                return Result.Success("Contract recipients created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Contract recipients creation failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message}");
            }

        }
    }
}
