using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Domain.Enums;
using ReventInject;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.Utilities.Queries
{
    public class GetCurrencyBySystemSettingsQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
        public int SystemSettingsId { get; set; }
    }

    public class GetCurrencyBySystemSettingsHandler : IRequestHandler<GetCurrencyBySystemSettingsQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetCurrencyBySystemSettingsHandler(IApplicationDbContext context, IMapper mapper,
            IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(GetCurrencyBySystemSettingsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var systemSetting = await _authService.GetSystemSettingsAsync(request.AccessToken, request.SubscriberId, request.UserId, request.SystemSettingsId);
                if (systemSetting == null)
                {
                    return Result.Failure("No system settings found");
                }

                var systemSettingsEntity = systemSetting.Entity;
                var systemSettingsModel = new SystemSettingDto
                {
                    Id = systemSettingsEntity.Id,
                    ExpirationReminder = systemSettingsEntity.ExpirationReminder,
                    ExpirationSettingsFrequency = systemSettingsEntity.ExpirationSettingsFrequency,
                    ExpirationSettingsFrequencyDesc = systemSettingsEntity.ExpirationSettingsFrequencyDesc,
                    ShouldSentDocumentsExpire = systemSettingsEntity.ShouldSentDocumentsExpire,
                    DocumentExpirationPeriod = systemSettingsEntity.DocumentExpirationPeriod,
                    WorkflowReminder = systemSettingsEntity.WorkflowReminder,
                    WorkflowReminderSettingsFrequency = systemSettingsEntity.WorkflowReminderSettingsFrequency,
                    WorkflowReminderSettingsFrequencyDesc = systemSettingsEntity.WorkflowReminderSettingsFrequencyDesc,
                    Currency = systemSettingsEntity.Currency,
                    Language = systemSettingsEntity.Language,
                    Name = systemSettingsEntity.Name,
                    SubscriberId = systemSettingsEntity.SubscriberId,
                    CreatedByEmail = systemSettingsEntity.CreatedByEmail,
                    CreatedById = systemSettingsEntity.CreatedById,
                    CreatedDate = systemSettingsEntity.CreatedDate,
                    LastModifiedById = systemSettingsEntity.LastModifiedById,
                    LastModifiedByEmail = systemSettingsEntity.LastModifiedByEmail,
                    Status = systemSettingsEntity.Status,
                    StatusDesc = systemSettingsEntity.StatusDesc
                };

                var currency = await _context.Currencies.FirstOrDefaultAsync(x => x.SubscriberId == systemSettingsModel.Id);

                if (currency == null)
                {
                    return Result.Failure("No currency set for this subscriber!");
                }

                if (currency.CurrencyCode.ToString() != systemSettingsModel.Currency)
                {

                    currency.CurrencyCode = (CurrencyCode)Enum.Parse(typeof(CurrencyCode), systemSettingsModel.Currency);
                    await _context.SaveChangesAsync(cancellationToken);
                }
                return Result.Success(currency);
            }
            catch (Exception ex)
            {

                return Result.Failure($"Some Error occoured, Currency is not set {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }

       
    }
}
