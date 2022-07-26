using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.AuthService.Application.Customizations.Commands.ExpirationReminder.DeleteExpirationReminder
{
    public class DeleteExpirationReminderCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public int SystemSettingsId { get; set; }
        public string UserId { get; set; }
    }

    public class DeleteExpirationReminderCommandHandler : IRequestHandler<DeleteExpirationReminderCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public DeleteExpirationReminderCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(DeleteExpirationReminderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var subscriber = await _context.Subscribers.Where(x=>x.Id == request.SubscriberId).FirstOrDefaultAsync();
                if (subscriber == null)
                {
                    return Result.Failure("Invalid Subscriber");
                }

                var expirationReminderEntity = await _context.ExpiryPeriods.FirstOrDefaultAsync(x => x.Id == request.Id && x.SystemSettingId == request.SystemSettingsId);
                if (expirationReminderEntity == null)
                {
                    return Result.Failure("Invalid expiration reminder setting!");
                }

                var systemSettings = await _context.SystemSettings.Where(x => x.Id == request.SystemSettingsId && x.SubscriberId == request.SubscriberId).FirstOrDefaultAsync();
                if (systemSettings == null)
                {
                    return Result.Failure("System Settings not existing on this subscriber. Please create a setting");
                }
                foreach (var item in systemSettings.ExpirationReminder)
                {
                    if (item.Id == request.Id)
                    {
                        item.Status = Domain.Enums.Status.Inactive;
                        item.StatusDesc = Domain.Enums.Status.Inactive.ToString();
                    }
                }
                _context.ExpiryPeriods.Remove(expirationReminderEntity);
                await _context.SaveChangesAsync(cancellationToken);


                return Result.Success("Expiration reminder setting have been deleted successfully");
            }
            catch (Exception ex)
            {

                return Result.Failure($" Failed to delete Expiration Reminder { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }
}
