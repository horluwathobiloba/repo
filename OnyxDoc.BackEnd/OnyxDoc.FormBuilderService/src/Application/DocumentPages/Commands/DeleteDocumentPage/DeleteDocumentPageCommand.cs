using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.DocumentPages.Commands
{
    public class DeleteDocumentPageCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int DocumentId { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }
    }


    public class DeleteDocumentPageCommandHandler : IRequestHandler<DeleteDocumentPageCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public DeleteDocumentPageCommandHandler(IApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<Result> Handle(DeleteDocumentPageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId ,request.UserId);

                var entity = await _context.DocumentPages.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId 
                && x.DocumentId == request.DocumentId && x.Id == request.Id);

                _context.DocumentPages.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Document page deleted successfully", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Delete document page failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }

        }
    }
}
