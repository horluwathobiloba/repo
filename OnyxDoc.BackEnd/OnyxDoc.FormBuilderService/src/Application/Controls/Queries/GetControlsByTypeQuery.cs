using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Domain.Enums;
using System; 
using System.Linq; 
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.Controls.Queries
{
    public class GetControlsByTypeQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; } 
        public ControlType ControlType { get; set; }
        public string UserId { get; set; }
    }
    public class GetControlsByTypeQueryHandler : IRequestHandler<GetControlsByTypeQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        public GetControlsByTypeQueryHandler(IApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<Result> Handle(GetControlsByTypeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId ,request.UserId);

                var list = await _context.Controls.Include(a => a.ControlProperties) 
                    .Where(x => x.SubscriberId == request.SubscriberId 
                    && x.ControlType == request.ControlType).ToListAsync();

                if (list == null || list.Count == 0)
                {
                    return Result.Failure("No record found!");
                }
                return Result.Success($"{list.Count}(s) found.", list);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get controls failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
