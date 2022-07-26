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
    public class GetAllDocumentFeedbackQuery : AuthToken, IRequest<Result>
    {
    }

    public class GetAllDocumentFeedbackQueryHandler : IRequestHandler<GetAllDocumentFeedbackQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetAllDocumentFeedbackQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(GetAllDocumentFeedbackQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allFeedbacks = await _context.DocumentFeedbacks.ToListAsync();
                if(allFeedbacks == null)
                {
                    return Result.Failure("No Feedback found");
                }
                return Result.Success(allFeedbacks);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error occured while retrieving all document feedbacks { ex?.Message ?? ex?.InnerException?.Message}");

            }
        }
    }
}
