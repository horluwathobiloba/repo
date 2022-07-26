using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Application.Common.Models;
using OnyxDoc.AuthService.Application.Customizations.Queries.GetSystemSetting;
using OnyxDoc.AuthService.Application.SystemSetting.Queries.GetSystemSetting;
using OnyxDoc.AuthService.Domain.Entities;
using OnyxDoc.AuthService.Domain.Enums;
using ReventInject;
using ReventInject.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.AuthService.Application.SystemSetting.Commands.UpdateSystemSetting
{
    public class UpdateSystemSettingCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
        public List<ExpiryPeriodDto> ExpirationReminder { get; set; }
        public SettingsFrequency ExpirationSettingsFrequency { get; set; }
        public int WorkflowReminder { get; set; }
        public SettingsFrequency WorkflowReminderSettingsFrequency { get; set; }
        public string Currency { get; set; }
        public string Language { get; set; }
        public bool ShouldSentDocumentsExpire { get; set; }
        public int DocumentExpirationPeriod { get; set; }
    }

    public class UpdateSystemSettingCommandHandler : IRequestHandler<UpdateSystemSettingCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly IBase64ToFileConverter _base64ToFileConverter;

        public UpdateSystemSettingCommandHandler(IApplicationDbContext context, IMapper mapper, IIdentityService identityService, IBase64ToFileConverter base64ToFileConverter)
        {
            _context = context;
            _mapper = mapper;
            _identityService = identityService;
            _base64ToFileConverter = base64ToFileConverter;
        }
        public async Task<Result> Handle(UpdateSystemSettingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userCheck = await _identityService.GetUserByIdAndSubscriber(request.UserId, request.SubscriberId);

                if (userCheck.user == null)
                {
                    return Result.Failure("Invalid Subscriber and User Specified");
                }
                var entity = await _context.SystemSettings
                    .Where(x => x.SubscriberId == request.SubscriberId && x.Id == request.Id)
                    .FirstOrDefaultAsync();

                if (entity == null)
                {
                    return Result.Failure($"Invalid System Setting specified.");
                }
                var expirationReminder = new List<ExpiryPeriod>();
                foreach (var item in request.ExpirationReminder)
                {
                    
                    var expiryeriod = _mapper.Map<ExpiryPeriod>(item);
                    expirationReminder.Add(expiryeriod);
                }
                entity.Currency = request.Currency;
                entity.Language = request.Language;
                entity.LastModifiedDate = DateTime.Now;
                entity.LastModifiedByEmail = userCheck.user.Email;
                entity.LastModifiedById = request.UserId;
                entity.ExpirationReminder = expirationReminder;
                entity.ExpirationSettingsFrequency = request.ExpirationSettingsFrequency;
                entity.ShouldSentDocumentsExpire = request.ShouldSentDocumentsExpire;
                entity.DocumentExpirationPeriod = request.DocumentExpirationPeriod;
                entity.ExpirationSettingsFrequencyDesc = request.ExpirationSettingsFrequency.ToString();
                entity.WorkflowReminder = request.WorkflowReminder;
                entity.WorkflowReminderSettingsFrequency = request.WorkflowReminderSettingsFrequency;
                entity.WorkflowReminderSettingsFrequencyDesc = request.WorkflowReminderSettingsFrequency.ToString();
                

                _context.SystemSettings.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<SystemSettingDto>(entity);
                return Result.Success("System Setting was updated successfully", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"System Setting update failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }
    }

}
