using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Application.Recipients.Queries.GetRecipients;
using OnyxDoc.DocumentService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.Recipients.Commands.DeleteRecipient
{
    public class DeleteRecipientCommand : IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int Id { get; set; } 
    }

    public class DeleteRecipientCommandHandler : IRequestHandler<DeleteRecipientCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DeleteRecipientCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(DeleteRecipientCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.Recipients.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId && x.Id == request.Id);
                if (entity == null)
                {
                    return Result.Failure("Invalid  recipient!");
                }
                 
                _context.Recipients.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<RecipientDto>(entity);
                return Result.Success(" recipient is now deleted!", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($" Recipient status delete failed { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }
}
