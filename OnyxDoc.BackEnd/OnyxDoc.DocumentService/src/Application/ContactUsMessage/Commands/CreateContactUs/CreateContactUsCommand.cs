using MediatR;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Domain.Entities;
using OnyxDoc.DocumentService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.ContactUsMessage.Commands.CreateContactUs
{
    public class CreateContactUsCommand  : IRequest<Result>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }

    public class CreateContactUsCommandHandler : IRequestHandler<CreateContactUsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public CreateContactUsCommandHandler(IApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }
        public async Task<Result> Handle(CreateContactUsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = new ContactFeedback
                {
                    Name = request.Name,
                    Email = request.Email,
                    ContactMessage = request.Message,
                    CreatedDate = DateTime.Now
                };

                EmailVm contactUsEmailMessage = new EmailVm
                {
                    FirstName = "Flowmono Team",
                    Subject = "Contact from Flowmono Website",
                    RecipientEmail = "support@flowmono.com",
                    Body = $"You have receive a new contact message from the website saying: {request.Message}"
                };

                await _emailService.SendEmail(contactUsEmailMessage);
                await _context.ContactFeedbacks.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success("Thank you. We would respond shortly!", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Retrieving Dashboard details failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
