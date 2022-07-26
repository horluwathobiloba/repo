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

namespace OnyxDoc.FormBuilderService.Application.Documents.Queries
{
    public class GetDocumentByVersionNumberQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int Id { get; set; }
        public decimal VersionNumber { get; set; }
        public string UserId { get; set; }
    }

    public class GetDocumentByVersionNumberQueryHandler : IRequestHandler<GetDocumentByVersionNumberQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetDocumentByVersionNumberQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetDocumentByVersionNumberQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                Document Document = await _context.Documents
                    .Where(a => a.SubscriberId == request.SubscriberId && a.Id == request.Id && a.VersionNumber == request.VersionNumber)
                    .Include(a => a.DocumentPages)
                    .ThenInclude(a => a.PageControlItems)
                    .ThenInclude(b => b.PageControlItemProperties)
                    .ThenInclude(b => b.PageControlItemPropertyValues)
                    .FirstOrDefaultAsync();

                if (Document == null)
                {
                    return Result.Failure("No record found!");
                }
                var result = _mapper.Map<DocumentDto>(Document);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving document. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
