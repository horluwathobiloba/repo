using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ReventInject;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Application.Common.Exceptions;
using OnyxDoc.DocumentService.Domain.Entities;

namespace OnyxDoc.DocumentService.Application.Comments.Queries.GetComments
{
    public class GetCommentsDynamicQuery : IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SearchText { get; set; }
    }

    public class GetCommentsDynamicQueryHandler : IRequestHandler<GetCommentsDynamicQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCommentsDynamicQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetCommentsDynamicQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.SubscriberId <= 0)
                {
                    return Result.Failure($"Subscriber must be specified.");
                }
                if (string.IsNullOrEmpty(request.SearchText))
                {
                    return Result.Failure($"Search text must be specified.");
                }

                var list = await _context.Comments.Where(a => a.SubscriberId == request.SubscriberId).ToListAsync();
                list = list.Where(a => request.SearchText.IsIn(a.Text)).ToList();

                if (list == null)
                {
                    throw new NotFoundException(nameof(Comment));
                }
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
