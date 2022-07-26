using MediatR;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.FAQCategory.Commands.UpdateFAQCategoryCommand
{
    public class UpdateFAQCategoryCommand :IRequest<Result>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class UpdateFAQCategoryCommandHandler : IRequestHandler<UpdateFAQCategoryCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        public UpdateFAQCategoryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(UpdateFAQCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.PaymentChannels.FindAsync(request.Id);
                if (entity == null)
                {
                    return Result.Failure("Invalid PaymentChannel specified.");
                }

                entity.Name = request.Name;
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success($"FAQ Category \"{entity.Name}\" updated successfully");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }

}
