using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Exceptions;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace OnyxDoc.FormBuilderService.Application.Documents.Queries
{
    public class GetDocumentByIdQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }
    }

    public class GetDocumentByIdQueryHandler : IRequestHandler<GetDocumentByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetDocumentByIdQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetDocumentByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var subscription = await _context.Documents
                    .Where(a => a.SubscriberId == request.SubscriberId && a.Id == request.Id)
                    .Include(a => a.DocumentPages)
                    .ThenInclude(a => a.PageControlItems)
                    .ThenInclude(b => b.PageControlItemProperties)
                    .ThenInclude(b => b.PageControlItemPropertyValues)
                    .FirstOrDefaultAsync();
                if (subscription == null)
                {
                    return Result.Failure("Invalid document");
                }

                var result = _mapper.Map<DocumentDto>(subscription);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving document. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
