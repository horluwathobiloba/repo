using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Domain.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.Controls.Queries
{
    public class GetControlByVersionNumberQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int Id { get; set; }
        public decimal VersionNumber { get; set; }
        public string UserId { get; set; }
    }

    public class GetControlByVersionNumberQueryHandler : IRequestHandler<GetControlByVersionNumberQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetControlByVersionNumberQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetControlByVersionNumberQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                Control control = await _context.Controls
                    .Where(a => a.SubscriberId == request.SubscriberId && a.Id == request.Id && a.VersionNumber == request.VersionNumber)
                    .Include(a => a.ControlProperties)
                    .ThenInclude(b => b.ControlPropertyItems)
                    .FirstOrDefaultAsync();
                if (control == null)
                {
                    return Result.Failure("No record found!");
                }
                var result = _mapper.Map<ControlDto>(control);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving control. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
