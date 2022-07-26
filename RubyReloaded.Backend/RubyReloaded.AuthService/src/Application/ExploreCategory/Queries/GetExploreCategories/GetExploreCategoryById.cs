using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Exceptions;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.ExploreCategory.Queries.GetExploreCategories
{
    public class GetExploreCategoryById : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class GetExploreCategoryByIdHandler : IRequestHandler<GetExploreCategoryById, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetExploreCategoryByIdHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetExploreCategoryById request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.ExploreCategories.FirstOrDefaultAsync(a => a.Id == request.Id && a.Status == Status.Active);
                if (entity == null)
                {
                    throw new NotFoundException(nameof(Domain.Entities.ExploreCategory), request.Id);
                }
                return Result.Success(entity);
            }
            catch (Exception ex)
            {
                return Result.Failure(ex?.Message ?? ex?.InnerException?.Message);
            }
        }
    }
}
