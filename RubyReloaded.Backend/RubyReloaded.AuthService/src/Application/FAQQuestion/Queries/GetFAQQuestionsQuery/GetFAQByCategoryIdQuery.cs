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
    public class GetFAQByCategoryIdQuery : IRequest<Result>
    {
        public int Id { get; set; }
    }
    public class GetFAQByCategoryIdQueryHandler : IRequestHandler<GetFAQByCategoryIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetFAQByCategoryIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
       

        public async Task<Result> Handle(GetFAQByCategoryIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.FAQQuestions.
                    Where(x => x.Id == request.Id)
                    .ToListAsync();
                return Result.Success(entity);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retreival was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
