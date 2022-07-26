using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Exceptions;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.DocumentPages.Queries
{
    public class GetDocumentPageByIdQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }
    }

    public class GetSubscriptionControlByIdQueryHandler : IRequestHandler<GetDocumentPageByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetSubscriptionControlByIdQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetDocumentPageByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var documentPage = await _context.DocumentPages
                    .Where(a => a.SubscriberId == request.SubscriberId && a.Id == request.Id)
                    .Include(a => a.PageControlItems)
                    .ThenInclude(b => b.PageControlItemProperties)
                    .ThenInclude(b => b.PageControlItemPropertyValues)
                    .FirstOrDefaultAsync();

                if (documentPage == null)
                {
                    return Result.Failure("Invalid document page");
                }

                var result = _mapper.Map<DocumentPageDto>(documentPage);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving document page. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
