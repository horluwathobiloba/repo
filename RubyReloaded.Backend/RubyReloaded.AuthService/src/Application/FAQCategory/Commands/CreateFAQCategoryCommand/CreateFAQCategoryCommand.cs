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

namespace RubyReloaded.AuthService.Application.FAQCategory.Commands.CreateFAQCategoryCommand
{
    public class CreateFAQCategoryCommand:IRequest<Result>
    {
        public string Name { get; set; }
        public string LoggedInUser { get; set; }

    }

    public class CreateFAQCategoryCommandHandler : IRequestHandler<CreateFAQCategoryCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        public CreateFAQCategoryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(CreateFAQCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var exists = await _context.FAQCategories.AnyAsync(a => (a.Name.ToUpper() == request.Name.ToUpper())
              && a.Status == Status.Active);

                if (exists)
                {
                    return Result.Failure(new string[] { "Create new FAQ Category failed because a FAQ category name already exists. Please enter a new FAQ category name to continue." });
                }

                var entity = new Domain.Entities.FAQCategory
                {
                    Name = request.Name,
                    StatusDesc = Status.Active.ToString(),
                    Status=Status.Active,
                    CreatedBy=request.LoggedInUser
                };
                _context.FAQCategories.Add(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("FAQ Category created successfully!", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "FAQ Category creation failed!", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
