using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Domain.Entities;
using OnyxDoc.FormBuilderService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.Controls.Queries
{
    public class GetPreviousControlVersionsQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }
    }

    public class GetPreviousControlQueryHandler : IRequestHandler<GetPreviousControlVersionsQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetPreviousControlQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetPreviousControlVersionsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                Control entity = await _context.Controls.FirstOrDefaultAsync(a => a.SubscriberId == request.SubscriberId);

                if (entity == null)
                {
                    throw new Exception("Record not found!");
                }

                var list = await _context.Controls.Include(a => a.ControlProperties).Where(a => a.SubscriberId == request.SubscriberId && a.Id == request.Id
                && a.VersionNumber < entity.VersionNumber).ToListAsync();

                if (list == null)
                {
                    throw new Exception("No record found!");
                }

                var result = _mapper.Map<List<ControlDto>>(list);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving the previous control versions. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
