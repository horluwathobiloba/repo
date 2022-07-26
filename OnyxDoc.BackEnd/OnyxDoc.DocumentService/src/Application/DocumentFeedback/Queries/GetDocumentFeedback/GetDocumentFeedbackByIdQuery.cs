using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.DocumentFeedback.Queries.GetDocumentFeedback
{
    public class GetDocumentFeedbackByIdQuery : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
    }

    public class GetDocumentFeedbackByIdQueryHandler : IRequestHandler<GetDocumentFeedbackByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;

        public GetDocumentFeedbackByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(GetDocumentFeedbackByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if(request.SubscriberId <= 0)
                {
                    return Result.Failure("Subscriber Id must be specified");
                }
                var documentFeedback = await _context.DocumentFeedbacks.FirstOrDefaultAsync(x => x.Id == request.Id && x.SubscriberId == request.SubscriberId);
                if(documentFeedback == null)
                {
                    return Result.Failure("Error retrieving document feedback by ID");
                }
                return Result.Success(documentFeedback);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving document feedback. Error: {ex?.Message + " " + ex?.InnerException.Message}");
            }
        }
    }
}
