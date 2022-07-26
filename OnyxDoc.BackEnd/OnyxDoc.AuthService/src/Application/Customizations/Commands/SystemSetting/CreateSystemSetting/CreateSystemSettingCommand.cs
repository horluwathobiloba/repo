using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Application.Common.Models;
using OnyxDoc.AuthService.Application.Customizations.Queries.GetSystemSetting;
using OnyxDoc.AuthService.Application.SystemSetting.Queries.GetSystemSetting;
using OnyxDoc.AuthService.Domain.Entities;
using OnyxDoc.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.AuthService.Application.SystemSetting.Commands.CreateSystemSetting
{
    public class CreateSystemSettingCommand : IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public ICollection<ExpiryPeriodDto> ExpirationReminder { get; set; }
        public SettingsFrequency ExpirationSettingsFrequency { get; set; }
        public int WorkflowReminder { get; set; }
        public SettingsFrequency WorkflowReminderSettingsFrequency { get; set; }
        public string Currency { get; set; }
        public string Language { get; set; }
        public string UserId { get; set; }
        public bool ShouldSentDocumentsExpire { get; set; }
        public int DocumentExpirationPeriod { get; set; }
    }


    public class CreateSystemSettingCommandHandler : IRequestHandler<CreateSystemSettingCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly IBase64ToFileConverter _base64ToFileConverter;

        public CreateSystemSettingCommandHandler(IApplicationDbContext context, IIdentityService identityService, IBase64ToFileConverter base64ToFileConverter, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _identityService = identityService;
            _base64ToFileConverter = base64ToFileConverter;
        }

        public async Task<Result> Handle(CreateSystemSettingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userCheck = await _identityService.GetUserByIdAndSubscriber(request.UserId, request.SubscriberId);

                if (userCheck.user == null)
                {
                    return Result.Failure("Invalid Subscriber and User Specified");
                }
                await _context.BeginTransactionAsync();

                //get previously existing system setting and make it inactive
                var existingSystemSetting = await _context.SystemSettings.Where(a => a.SubscriberId == request.SubscriberId).ToListAsync();
                if (existingSystemSetting != null && existingSystemSetting.Count > 0)
                {
                    List<Domain.Entities.SystemSetting> systemSettings = new List<Domain.Entities.SystemSetting>();
                    var expirationReminders = new List<ExpiryPeriod>();
                    foreach (var item in request.ExpirationReminder)
                    {
                        var expiryPeriod = _mapper.Map<ExpiryPeriod>(item);
                        expirationReminders.Add(expiryPeriod);
                        _context.ExpiryPeriods.Add(expiryPeriod);
                    }
                    foreach (var setting in existingSystemSetting)
                    {
                        //setting.ExpirationReminder = (ICollection<ExpiryPeriod>)request.ExpirationReminder;
                        if (setting.ExpirationReminder.Count > 0)
                        {
                            setting.ExpirationReminder = expirationReminders;
                            foreach (var item in expirationReminders)
                            {
                                var expiryperiodEntity = await _context.ExpiryPeriods.Where(x => x.Id == item.Id).FirstOrDefaultAsync();
                                expiryperiodEntity.Status = Status.Inactive;
                                expiryperiodEntity.StatusDesc = Status.Inactive.ToString();
                                _context.ExpiryPeriods.Update(expiryperiodEntity);
                                item.Status = Status.Inactive;
                                item.StatusDesc = Status.Inactive.ToString();
                            }
                        }
                        setting.Status = Status.Inactive;
                        setting.StatusDesc = Status.Inactive.ToString();
                        systemSettings.Add(setting);
                    }
                    _context.SystemSettings.UpdateRange(systemSettings);
                   // _context.ExpiryPeriods.UpdateRange(expirationReminders);
                    return Result.Success("SystemSetting was updated successfully", systemSettings);
                }
                var entity = new SystemSettingDto
                {
                    SubscriberId = request.SubscriberId,
                    Currency = request.Currency,
                    ExpirationReminder = request.ExpirationReminder,
                    ExpirationSettingsFrequency = request.ExpirationSettingsFrequency,
                    ExpirationSettingsFrequencyDesc = request.ExpirationSettingsFrequency.ToString(),
                    ShouldSentDocumentsExpire = request.ShouldSentDocumentsExpire,
                    DocumentExpirationPeriod = request.DocumentExpirationPeriod,
                    WorkflowReminder = request.WorkflowReminder,
                    WorkflowReminderSettingsFrequency = request.WorkflowReminderSettingsFrequency,
                    WorkflowReminderSettingsFrequencyDesc = request.WorkflowReminderSettingsFrequency.ToString(),
                    Language = request.Language,
                    CreatedById = request.UserId,
                    CreatedByEmail = userCheck.user.Email,
                    CreatedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
                };
                var result = _mapper.Map<Domain.Entities.SystemSetting>(entity);
                await _context.SystemSettings.AddAsync(result);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
                //var result = _mapper.Map<SystemSettingDto>(entity);
                return Result.Success("SystemSetting was created successfully", result);
            }
            catch (Exception ex)
            {
                _context.RollbackTransaction();
                return Result.Failure($"SystemSetting creation failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message} ");
            }
        }
    }

}

