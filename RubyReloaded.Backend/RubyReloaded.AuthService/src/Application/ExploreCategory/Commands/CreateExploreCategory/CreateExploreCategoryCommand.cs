using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.ExploreCategory.Commands.CreateExploreCategory
{
    public class CreateExploreCategoryCommand : IRequest<Result>
    {
        public string Name { get; set; }
        public string LoggedInUser { get; set; }

    }

    public class CreateExploreCategoryCommandHandler : IRequestHandler<CreateExploreCategoryCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        public CreateExploreCategoryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(CreateExploreCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var exists = await _context.ExploreCategories.AnyAsync(a => (a.Name.ToUpper() == request.Name.ToUpper())
              && a.Status == Status.Active);

                if (exists)
                {
                    return Result.Failure(new string[] { "Create new Explore Category failed because a Explore category name already exists. Please enter a new Explore category name to continue." });
                }

                var entity = new Domain.Entities.ExploreCategory
                {
                    Name = request.Name,
                    StatusDesc = Status.Active.ToString(),
                    Status = Status.Active,
                    CreatedBy = request.LoggedInUser
                };
                _context.ExploreCategories.Add(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Explore Category created successfully!", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Explore Category creation failed!", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
