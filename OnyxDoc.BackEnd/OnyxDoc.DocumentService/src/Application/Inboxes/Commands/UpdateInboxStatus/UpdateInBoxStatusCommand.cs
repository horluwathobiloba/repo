using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.Inboxes.Commands.UpdateInboxStatus
{
    public class UpdateInBoxStatusCommand:AuthToken, IRequest<Result>
    {
        public int InboxId { get; set; }
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateInBoxStatusCommandHandler : IRequestHandler<UpdateInBoxStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        public UpdateInBoxStatusCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(UpdateInBoxStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.Inboxes.FirstOrDefaultAsync(x =>  x.Id == request.InboxId);
                if (entity == null)
                {
                    return Result.Failure("Invalid inbox!");
                }
                entity.Read = !entity.Read;
                _context.Inboxes.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success(entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"License type status update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }
}
