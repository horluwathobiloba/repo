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

namespace OnyxDoc.DocumentService.Application.Comments.Queries.GetComments
{
    public class GetCommentByIdQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int Id { get; set; }
    }

    public class GetCommentByIdQueryHandler : IRequestHandler<GetCommentByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCommentByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.SubscriberId <= 0)
                {
                    return Result.Failure($"Subscriber Id must be specified.");
                } 

                var contractComment = await _context.Comments.Include(a=>a.Coordinate).FirstOrDefaultAsync(a => a.SubscriberId == request.SubscriberId && a.Id == request.Id);
                if (contractComment == null)
                {
                    return Result.Failure("Invalid comment Id");
                }

                var result = _mapper.Map<CommentDto>(contractComment);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving comments. Error: {ex?.Message +" "+ ex?.InnerException.Message}");
            }
        }
    }
}
