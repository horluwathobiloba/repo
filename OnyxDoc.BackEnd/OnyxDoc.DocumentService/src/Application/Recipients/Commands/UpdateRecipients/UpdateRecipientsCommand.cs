using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Application.Recipients.Commands.CreateRecipient;
using OnyxDoc.DocumentService.Application.Recipients.Commands.CreateRecipients;
using OnyxDoc.DocumentService.Application.Recipients.Queries.GetRecipients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.Recipients.Commands.UpdateRecipients
{
    public class UpdateRecipientsCommand : IRequest<Result>
    {
        
        public int SubscriberId { get; set; }
        public List<UpdateRecipientRequest> Recipients { get; set; }
        public string UserId { get; set; }

        public int DocumentId { get; set; }
    }

    public class UpdateRecipientCommandHandler : IRequestHandler<UpdateRecipientsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateRecipientCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result> Handle(UpdateRecipientsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await UpdateRecipients(request, cancellationToken);
            }

            catch (Exception ex)
            {
                return Result.Failure($" recipients update failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }

        public async Task<Result> UpdateRecipients(UpdateRecipientsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var recepientEntity = new List<RecipientDto>();
                //await _context.BeginTransactionAsync();
                foreach (var recipient in request.Recipients)
                {
                    //var modfiedEntityExists = await _context.Recipients.AnyAsync(x => x.SubscriberId == request.SubscriberId && x.Id != recipient.Id && x.Email.ToLower() == recipient.Email.ToLower());

                    //if (modfiedEntityExists)
                    //{
                    //    return Result.Failure($"Another recipient with this email '{recipient.Email}' already exists. Please change the email.");
                        
                    //}

                    var entity = await _context.Recipients
                        .Where(x => x.SubscriberId == request.SubscriberId && x.Id == recipient.Id)
                        .FirstOrDefaultAsync();

                    if (entity == null)
                    {
                        //return Result.Failure($"Invalid recipient specified.");
                        var newRecipient = new CreateRecipientCommand()
                        {
                            DocumentId = request.DocumentId,
                            SubscriberId = request.SubscriberId,
                            Email = recipient.Email,
                            FirstName = recipient.FirstName,
                            LastName = recipient.LastName,
                            Rank = recipient.Rank,
                            RecipientCategory = recipient.RecipientCategory,
                            UserId = request.UserId
                        };

                        var newRecipientCommandHandler = new CreateRecipientCommandHandler(_context, _mapper);
                        var newEntiry = await newRecipientCommandHandler.Handle(newRecipient, cancellationToken);
                        
                        //var mappedEntity = _mapper.Map<Domain.Entities.Recipient>(newEntiry);
                        
                        var mappedEntityList = _mapper.Map<RecipientDto>(newEntiry.Entity);
                        recepientEntity.Add(mappedEntityList);
                        continue;
                    }

                    var rankExists = await _context.Recipients
                       .AnyAsync(a => a.SubscriberId == request.SubscriberId && a.Id == recipient.Id && a.Rank == recipient.Rank);

                    var rank = 0; // get the last ranking if the requested rank already exists
                    if (rankExists)
                    {
                        var maxRank = await _context.Recipients
                             .Where(a => a.SubscriberId == request.SubscriberId && a.Id == recipient.Id)
                             .MaxAsync(r => r.Rank);
                        rank = maxRank + 1;
                    }
                    else
                    {
                        rank = recipient.Rank;
                    }

                    entity.Email = recipient.Email;
                    entity.RecipientCategory = recipient.RecipientCategory.ToString();
                    entity.FirstName = recipient.FirstName;
                    entity.LastName = recipient.LastName;
                    entity.Rank = recipient.Rank;

                    entity.LastModifiedBy = request.UserId;
                    entity.LastModifiedDate = DateTime.Now;

                    var entityUpdated =  _mapper.Map<RecipientDto>(entity);
                    recepientEntity.Add(entityUpdated);
                    _context.Recipients.Update(entity);
                    await _context.SaveChangesAsync(cancellationToken);
                }
                //var result = _mapper.Map<List<Domain.Entities.Recipient>>(recepientEntity);
                return Result.Success("recipients were updated successfully!", recepientEntity);
            }
            catch (Exception ex)
            {
                return Result.Failure($" recipient update failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }
    }
}
