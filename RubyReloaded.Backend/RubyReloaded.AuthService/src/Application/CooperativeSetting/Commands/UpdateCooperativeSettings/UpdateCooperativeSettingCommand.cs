using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.CooperativeSetting.Commands.UpdateCooperativeSettings
{
    public class UpdateCooperativeSettingCommand:IRequest<Result>
    {
        public int CooperativeId { get; set; }
        public bool RequestToJoin { get; set; }
    }

    public class UpdateCooperativeSettingCommandHandler : IRequestHandler<UpdateCooperativeSettingCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        public UpdateCooperativeSettingCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(UpdateCooperativeSettingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var cooperative = await _context.CooperativeSettings.FirstOrDefaultAsync(a => a.CooperativeId == request.CooperativeId);
                cooperative.RequestToJoin = request.RequestToJoin;
                _context.CooperativeSettings.Update(cooperative);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success(cooperative);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Error updating setting: ", ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
