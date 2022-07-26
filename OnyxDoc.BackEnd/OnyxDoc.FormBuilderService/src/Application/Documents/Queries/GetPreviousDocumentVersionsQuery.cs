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

namespace OnyxDoc.FormBuilderService.Application.Documents.Queries
{
    public class GetPreviousDocumentVersionsQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int InitialDocumentVersionId { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }
    }

    public class GetPreviousDocumentQueryHandler : IRequestHandler<GetPreviousDocumentVersionsQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetPreviousDocumentQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetPreviousDocumentVersionsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                Document document = await _context.Documents.FirstOrDefaultAsync(a => a.SubscriberId == request.SubscriberId && a.Id == request.Id);

                if (document == null)
                {
                    throw new Exception("Document not found!");
                }

                //TODO : Finalize testing when document version update is done
                var list = await _context.Documents.Include(a => a.DocumentPages).Where(a => a.SubscriberId == request.SubscriberId && 
                a.InitialDocumentVersionId == request.Id && a.VersionNumber < document.VersionNumber).ToListAsync();

                if (list == null || list.Count() == 0)
                {
                    throw new Exception("No record found!");
                }

                var result = _mapper.Map<List<DocumentDto>>(list);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving the previous Document versions. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
