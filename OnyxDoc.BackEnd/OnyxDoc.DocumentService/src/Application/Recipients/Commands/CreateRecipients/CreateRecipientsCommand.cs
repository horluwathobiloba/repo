using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Models;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Application.Recipients.Queries.GetRecipients;
using OnyxDoc.DocumentService.Domain.Entities;
using OnyxDoc.DocumentService.Domain.Enums;
using OnyxDoc.DocumentService.Infrastructure.Services;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.Recipients.Commands.CreateRecipients
{
    public class CreateRecipientsCommand : IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public List<CreateRecipientRequest> Recipients { get; set; }
        public string UserId { get; set; }

        public int DocumentId { get; set; }
    }

    public class CreateRecipientsCommandHandler : IRequestHandler<CreateRecipientsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateRecipientsCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result> Handle(CreateRecipientsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await CreateRecipients(request, cancellationToken);             
            }
            catch (Exception ex)
            {
                return Result.Failure($" recipients creation failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }

        public async Task<Result> CreateRecipients(CreateRecipientsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var list = new List<Domain.Entities.Recipient>();

                await _context.BeginTransactionAsync();

                foreach (var item in request.Recipients)
                {
                    var exists = await _context.Recipients
                   .AnyAsync(x => x.SubscriberId == request.SubscriberId && x.DocumentId == request.DocumentId
                   && x.Email.ToLower() == item.Email.ToLower() && x.RecipientCategory == item.RecipientCategory.ToString());

                    if (exists)
                    {
                        return Result.Failure($" recipient already exists for {item.Email} ({item.Email})!");
                    }
                   
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
                   

                    var entity = new Domain.Entities.Recipient
                    {
                        SubscriberId = request.SubscriberId,
                       // SubscriberName = request.SubscriberName,
                        DocumentId = request.DocumentId,
                        Email = item.Email,
                        Rank = item.Rank,
                        RecipientCategory = item.RecipientCategory.ToString(),
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        CreatedBy = request.UserId,
                        CreatedDate = DateTime.Now,
                        LastModifiedBy = request.UserId,
                        LastModifiedDate = DateTime.Now,
                        Status = Status.Active,
                        StatusDesc = Status.Active.ToString()
                    };
                    list.Add(entity);
                }
                await _context.Recipients.AddRangeAsync(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<RecipientDto>>(list);
                return Result.Success(" recipients created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($" recipients creation failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }

        }
    }
}
