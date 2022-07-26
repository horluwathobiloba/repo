using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Application.Recipients.Queries.GetRecipients;
using OnyxDoc.DocumentService.Domain.Entities;
using OnyxDoc.DocumentService.Domain.Enums;
using OnyxDoc.DocumentService.Infrastructure.Services;
using ReventInject;
using ReventInject.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.Recipients.Commands.CreateRecipient
{
    public class CreateRecipientCommand : IRequest<Result>
    {
        public int DocumentId { get; set; }
        public int SubscriberId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Rank { get; set; }
        public RecipientCategory RecipientCategory { get; set; }
        public string UserId { get; set; }
    }


    public class CreateRecipientCommandHandler : IRequestHandler<CreateRecipientCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateRecipientCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(CreateRecipientCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var exists = await _context.Recipients
                    .AnyAsync(x => x.SubscriberId == request.SubscriberId && x.DocumentId == request.DocumentId
                    && x.Email.ToLower() == request.Email.ToLower());

                if (exists)
                {
                    return Result.Failure($" Recipient already exists for({request.Email})!");
                }

               
                var entity = new Domain.Entities.Recipient
                {
                    SubscriberId = request.SubscriberId,
                    DocumentId = request.DocumentId,
                    //SubscriberName = request.SubscriberName,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    RecipientCategory = request.RecipientCategory.ToString(),
                    Rank = request.Rank,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
                };

                await _context.Recipients.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<RecipientDto>(entity);
                return Result.Success("Recipient was created successfully", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Recipient creation failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message} ");
            }
        }
    }

}

