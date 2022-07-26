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

namespace OnyxDoc.DocumentService.Application.Comments.Commands.DeleteComment
{
    public class DeleteCommentCommand : IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int Id { get; set; }
    }


    public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        public DeleteCommentCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.Comments.FirstOrDefaultAsync(x => x.Id == request.Id);
                _context.Comments.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Comments deleted successfully",entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($" comment delete failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }

        }
    }
}
