using MediatR;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.ExploreCategory.Commands.UpdateExploreCategory
{
   public class UpdateExploreCategoryCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class UpdateExploreCategoryCommandHandler : IRequestHandler<UpdateExploreCategoryCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        public UpdateExploreCategoryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(UpdateExploreCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.PaymentChannels.FindAsync(request.Id);
                if (entity == null)
                {
                    return Result.Failure("Invalid Category specified.");
                }

                entity.Name = request.Name;
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success($"Explore Category \"{entity.Name}\" updated successfully");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
