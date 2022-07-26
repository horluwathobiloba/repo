using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.Comments.Queries.GetComments
{
    public class GetCommentsQuery:IRequest<Result>
    {
        public int SubscriberId { get; set; }
    }

    public class GetCommentsQueryHandler : IRequestHandler<GetCommentsQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCommentsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.SubscriberId <= 0)
                {
                    return Result.Failure($"Subscriber must be specified.");
                } 

                var list = await _context.Comments.Include(a=>a.Coordinate).Where(a => a.SubscriberId == request.SubscriberId).ToListAsync();
                var result = _mapper.Map<List<CommentDto>>(list);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving contract comments. Error: {ex?.Message +" "+ ex?.InnerException.Message}");
            }
        }
    }
}
