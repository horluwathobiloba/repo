using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Tags.Commands.CreateTagsCommand
{
    public class CreateTagsCommand : IRequest<Result>
    {
        public List<TagVm> TagVms { get; set; }
    }

    public class CreateTagsCommandHandler : IRequestHandler<CreateTagsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
       // private readonly IAuthService _authService;
        public CreateTagsCommandHandler(IApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<Result> Handle(CreateTagsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _context.BeginTransactionAsync();
                var getAll = await _context.Tags.ToListAsync();
                var tags = new List<Domain.Entities.Tag>();
                foreach (var inboxVm in request.TagVms)
                {
                    var inbox = new Domain.Entities.Tag
                    {
                       Name=inboxVm.Name
                    };
                    tags.Add(inbox);
                }
              //  var tagsToCreate = getAll.Where(x => tags.All(a => a.Name != x.Name));
                var tagsToCreate = getAll.Where(x=>tags.Any(a=>a.Name==x.Name));
                await _context.Tags.AddRangeAsync(tagsToCreate);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
                return Result.Success(tagsToCreate);
            }
            catch (Exception ex)
            {
                _context.RollbackTransaction();
                return Result.Failure($"Retrieving Dashboard details failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}

