using AutoMapper;
using MediatR;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.DocumentFeedback.Commands.CreateDocumentFeedback
{
    public class CreateDocumentFeedbackCommand : AuthToken, IRequest<Result>
    {
        public int DocumentId { get; set; }
        public string UserId { get; set; }
        public string Comment { get; set; }
        public Rating Rating { get; set; }
        public int SubscriberId { get; set; }
    }

    public class CreateDocumentFeedbackResultHandler : IRequestHandler<CreateDocumentFeedbackCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreateDocumentFeedbackResultHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(CreateDocumentFeedbackCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                if (_authService.Subscriber == null)
                {
                    return Result.Failure(new string[] { "Invalid subscriber details specified." });
                }

                var entity = new Domain.Entities.DocumentFeedback
                {
                    SubscriberId = request.SubscriberId,
                    UserId = request.UserId,
                    DocumentId = request.DocumentId,
                    Comment = request.Comment,
                    Rating = request.Rating,
                    RatingDesc = request.Rating.ToString()
                };

                await _context.DocumentFeedbacks.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success("Thank you for your feedback!", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Document feedback creation failed. Error: { ex?.Message + " " + ex?.InnerException.Message}");
            }
        }
    }

}
