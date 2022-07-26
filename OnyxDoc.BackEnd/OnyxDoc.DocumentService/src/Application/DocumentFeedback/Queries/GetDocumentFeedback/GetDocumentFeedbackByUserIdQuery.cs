using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.DocumentFeedback.Queries.GetDocumentFeedback
{
    public class GetDocumentFeedbackByUserIdQuery : AuthToken, IRequest<Result>
    {
        public string UserId { get; set; }
        public int SubscriberId { get; set; }
    }

    public class GetDocumentFeedbackByUserIdQueryHandler : IRequestHandler<GetDocumentFeedbackByUserIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;

        public GetDocumentFeedbackByUserIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(GetDocumentFeedbackByUserIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.SubscriberId <= 0)
                {
                    return Result.Failure("Subscriber Id must be specified");
                }
                var documentFeedbacksByUser = await _context.DocumentFeedbacks.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId && x.UserId == request.UserId);
                if (documentFeedbacksByUser == null)
                {
                    return Result.Failure("Error retrieving documents assigned to a particular user");
                }
                return Result.Success(documentFeedbacksByUser);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving document feedbacks by particular user. Error: {ex?.Message + " " + ex?.InnerException.Message}");
            }
        }
    }
}
