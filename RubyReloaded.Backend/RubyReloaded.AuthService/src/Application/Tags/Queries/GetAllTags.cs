using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Tags.Queries
{
    public class GetAllTags:IRequest<Result>
    {
    }

    public class GetAllTagsHandler : IRequestHandler<GetAllTags, Result>
    {
        private readonly IApplicationDbContext _context;
        
        public GetAllTagsHandler(IApplicationDbContext context)
        {
            _context = context;
           
        }
        public async Task<Result> Handle(GetAllTags request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.Tags.ToListAsync();
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving inbox. Error: {ex?.Message + " " + ex?.InnerException.Message}");
            }
        }
    }
}
