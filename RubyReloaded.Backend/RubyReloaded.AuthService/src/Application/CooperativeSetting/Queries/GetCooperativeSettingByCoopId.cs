using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.CooperativeSetting.Queries
{
    public class GetCooperativeSettingByCoopId:IRequest<Result>
    {
        public int CooperativeId { get; set; }
    }
    public class GetCooperativeSettingByCoopIdHandler : IRequestHandler<GetCooperativeSettingByCoopId, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        public GetCooperativeSettingByCoopIdHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }
        public async Task<Result> Handle(GetCooperativeSettingByCoopId request, CancellationToken cancellationToken)
        {
            try
            {
                var cooperative = await _context.CooperativeSettings.FirstOrDefaultAsync(a => a.CooperativeId == request.CooperativeId);
                return Result.Success(cooperative);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving cooperative was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
