using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Application.Recipients.Queries.GetRecipients;
using OnyxDoc.DocumentService.Domain.Entities;
using OnyxDoc.DocumentService.Domain.Enums;
using ReventInject;
using ReventInject.Utilities.Enums;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.Recipients.Commands.UpdateRecipient
{
    public class UpdateRecipientCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public int DocumentId { get; set; }
        public int SubscriberId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public int Rank { get; set; }
        public RecipientCategory RecipientCategory { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateRecipientCommandHandler : IRequestHandler<UpdateRecipientCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateRecipientCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result> Handle(UpdateRecipientCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //var modfiedEntityExists = await _context.Recipients.AnyAsync(x => x.SubscriberId == request.SubscriberId && x.Id != request.Id
                //&& x.Email.ToLower() == request.Email.ToLower());

                //if (modfiedEntityExists)
                //{
                //    return Result.Failure($"Another recipient with this email '{request.Email}' already exists. Please change the email.");
                //}

                var entity = await _context.Recipients
                    .Where(x => x.SubscriberId == request.SubscriberId && x.Id == request.Id)
                    .FirstOrDefaultAsync();

                if (entity == null)
                {
                    return Result.Failure($"Invalid recipient specified.");
                } 
               

                var rankExists = await _context.Recipients
                       .AnyAsync(a => a.SubscriberId == request.SubscriberId && a.Id == request.Id && a.Rank == request.Rank);

                var rank = 0; // get the last ranking if the requested rank already exists
                if (rankExists)
                {
                    var maxRank = await _context.Recipients
                         .Where(a => a.SubscriberId == request.SubscriberId && a.Id == request.Id)
                         .MaxAsync(r => r.Rank);
                    rank = maxRank + 1;
                }
                else
                {
                    rank = request.Rank;
                }
 
                entity.Email = request.Email;
                entity.RecipientCategory = request.RecipientCategory.ToString();
                entity.Name = request.Name;
                entity.Rank = request.Rank;

                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;

                _context.Recipients.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<RecipientDto>(entity);
                return Result.Success(" recipient was updated successfully", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($" recipient update failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }
    }

}
