using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Exceptions;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.FAQQuestion.Queries.GetFAQQuestionsQuery
{
    public class GetAllFAQQuestionsQuery:IRequest<Result>
    {
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class GetAllFAQQuestionsQueryHandler : IRequestHandler<GetAllFAQQuestionsQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public GetAllFAQQuestionsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result> Handle(GetAllFAQQuestionsQuery request, CancellationToken cancellationToken)
        {

            try
            {
                var entities = await _context.FAQQuestions.Where(a => a.Status != Status.Deactivated)
                   .Skip(request.Skip)
                   .Take(request.Take)
                     .ToListAsync();

                if (entities == null && entities.Count() <= 0)
                {
                    throw new NotFoundException(nameof(Domain.Entities.FAQQuestion), request);
                }
                // var paymentChannels = _mapper.Map<List<PaymentChannelListDto>>(entities);
                return Result.Success(entities);

            }
            catch (Exception ex)
            {
                return Result.Failure(ex?.Message ?? ex?.InnerException?.Message);
            }
        }
    }
}
