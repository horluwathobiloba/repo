using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Application.Recipients.Queries.GetRecipients; 
using OnyxDoc.DocumentService.Domain.Enums;
using System; 
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.Recipients.Commands.UpdateRecipientStatus
{
    public class UpdateRecipientStatusCommand : IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int Id { get; set; }
        public Status Status { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateRecipientStatusCommandHandler : IRequestHandler<UpdateRecipientStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateRecipientStatusCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(UpdateRecipientStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.Recipients.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId && x.Id == request.Id);
                if (entity == null)
                {
                    return Result.Failure("Invalid  recipient!");
                }

                string message = "";
                switch (request.Status)
                {
                    case Status.Inactive:
                        entity.Status = Status.Inactive;
                        message = " recipient is now inactive!";
                        break;
                    case Status.Active:
                        entity.Status = Status.Active;
                        message = " recipient was successfully activated!";
                        break;
                    case Status.Deactivated:
                        message = " recipient was deactivated!";
                        break;
                    default:
                        break;
                }

                entity.Status = request.Status;
                entity.StatusDesc = request.Status.ToString();
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<RecipientDto>(entity);
                return Result.Success(message, entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Recipient status update failed { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }
}
