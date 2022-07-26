using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.FAQQuestion.Queries.GetFAQQuestionsQuery
{
    public class GetFAQByTagQuery:IRequest<Result>
    {
        public int TagId { get; set; }
    }

    public class GetFAQByTagQueryHandler : IRequestHandler<GetFAQByTagQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public GetFAQByTagQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetFAQByTagQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var questions = await _context.TagFAQs
                    .Where(x => x.TagId == request.TagId)
                    .Include(x => x.FAQQuestion)
                    .ToListAsync();
                return Result.Success(questions);
            }
            catch (Exception ex)
            {
                return Result.Failure(ex?.Message ?? ex?.InnerException?.Message);
            }
        }
    }
}
