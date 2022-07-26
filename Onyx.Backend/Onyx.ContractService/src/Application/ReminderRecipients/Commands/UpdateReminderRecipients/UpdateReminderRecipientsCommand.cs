using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using Onyx.ReminderService.Application.ReminderRecipients.Queries.GetReminderRecipients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ConractService.Application.ReminderRecipients.Commands.UpdateReminderRecipients
{
    public class UpdateReminderRecipientsCommand : IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public int ContractId { get; set; }
        public string UserId { get; set; }
        public List<UpdateReminderRecipientRequest> ReminderRecipients { get; set; }

    }

    public class UpdateReminderRecipientsCommandHandler : IRequestHandler<UpdateReminderRecipientsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateReminderRecipientsCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result> Handle(UpdateReminderRecipientsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var list = new List<ReminderRecipient>();

                await _context.BeginTransactionAsync();

                foreach (var item in request.ReminderRecipients)
                {
                    var modfiedEntityExists = await _context.ReminderRecipients
                        .AnyAsync(x => x.OrganisationId == request.OrganisationId && x.ContractId == request.ContractId && x.Email.ToLower() == item.Email.ToLower());

                    if (modfiedEntityExists)
                    {
                        continue;
                        //return Result.Failure($"Another Reminder recipient with this email '{item.Email}' already exists. Please change the email.");
                    }
                  
                    if (string.IsNullOrEmpty(item.Email))
                    {
                        return Result.Failure($"Invalid recipient email specified for {item.Email}.");
                    }
                  

                    var entity = await _context.ReminderRecipients
                        .Where(x => x.OrganisationId == request.OrganisationId && x.Id == item.Id)
                        .FirstOrDefaultAsync();

                    if (entity == null)
                    {
                        entity = new ReminderRecipient
                        {
                            OrganisationId = request.OrganisationId,
                            OrganisationName = request.OrganisationName,
                            ContractId = request.ContractId,
                            Email = item.Email,

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
                        entity.LastModifiedBy = request.UserId;
                        entity.LastModifiedDate = DateTime.Now;
                    }

                    list.Add(entity);
                }

                _context.ReminderRecipients.UpdateRange(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<ReminderRecipientDto>>(list);
                return Result.Success("Reminder Recipient update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Reminder Recipient update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }


    }


}
