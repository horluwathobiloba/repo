using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.ContractRecipients.Queries.GetContractRecipients;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractRecipients.Commands.UpdateContractRecipients
{
    public class UpdateContractRecipientsCommand : IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public int ContractId { get; set; }
        public string UserId { get; set; }
        public List<UpdateContractRecipientRequest> ContractRecipients { get; set; }

    }

    public class UpdateContractRecipientsCommandHandler : IRequestHandler<UpdateContractRecipientsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateContractRecipientsCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result> Handle(UpdateContractRecipientsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var list = new List<ContractRecipient>();
                var rank = 0; 
                await _context.BeginTransactionAsync();

                foreach (var item in request.ContractRecipients)
                {
                    var modfiedEntityExists = await _context.ContractRecipients
                        .AnyAsync(x => x.OrganisationId == request.OrganisationId && x.ContractId == request.ContractId && x.Email.ToLower() == item.Email.ToLower()
                         && x.RecipientCategory == item.RecipientCategory.ToString());

                    if (modfiedEntityExists)
                    {
                        continue;
                        //return Result.Failure($"Another Contract recipient with this email '{item.Email}' already exists. Please change the email.");
                    }
                    if (string.IsNullOrEmpty(item.Name))
                    {
                        return Result.Failure($"Invalid recipient name specified for {item.Email}.");
                    }
                    if (string.IsNullOrEmpty(item.Email))
                    {
                        return Result.Failure($"Invalid recipient email specified for {item.Email}.");
                    }
                    if (item.Rank <= 0)
                    {
                        return Result.Failure($"Invalid rank specified for {item.Email}.");
                    }
                    if (!item.RecipientCategory.IsEnum<RecipientCategory>())
                    {
                        return Result.Failure($"Invalid recipient category specified for {item.Email}.");
                    }

                    var entity = await _context.ContractRecipients
                        .Where(x => x.OrganisationId == request.OrganisationId && x.Id == item.Id)
                        .Include(a => a.Contract)
                        .FirstOrDefaultAsync();

                    var rankExists = await _context.ContractRecipients
                           .AnyAsync(a => a.OrganisationId == request.OrganisationId && a.ContractId == request.ContractId && a.Rank == item.Rank);


                    // get the last ranking if the requested rank already exists
                    if (rankExists)
                    {
                        if (list != null && list.Count > 0)
                        {
                            if (rank > 0)
                            {
                                rank = rank + 1;
                            }
                        }
                        else
                        {
                            var maxRank = await _context.ContractRecipients
                           .Where(a => a.OrganisationId == request.OrganisationId && a.ContractId == request.ContractId)
                           .MaxAsync(r => r.Rank);
                            rank = maxRank + 1;
                        }
                      
                    }
                    else
                    {
                            rank = rank + 1;
                        //rank = item.Rank++;
                    }

                    if (entity == null)
                    {
                        entity = new ContractRecipient
                        {
                            OrganisationId = request.OrganisationId,
                            OrganisationName = request.OrganisationName,
                            ContractId = request.ContractId,
                            Email = item.Email,
                            RecipientCategory = item.RecipientCategory.ToString(),
                            Designation = item.Designation,
                            Rank = rank,

                            CreatedBy = request.UserId,
                            CreatedDate = DateTime.Now,
                            LastModifiedBy = request.UserId,
                            LastModifiedDate = DateTime.Now,
                            Status = Status.Active,
                            StatusDesc = Status.Active.ToString()
                        };
                    }
                    else
                    {
                        entity.Email = item.Email;
                        entity.RecipientCategory = item.RecipientCategory.ToString();
                        entity.Designation = item.Designation;
                        entity.Name = item.Name;
                        entity.Rank = item.Rank;

                        entity.LastModifiedBy = request.UserId;
                        entity.LastModifiedDate = DateTime.Now;
                    }

                    list.Add(entity);
                }

                _context.ContractRecipients.UpdateRange(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<ContractRecipientDto>>(list);
                return Result.Success("Contract Recipient update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Contract Recipient update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }


    }


}
