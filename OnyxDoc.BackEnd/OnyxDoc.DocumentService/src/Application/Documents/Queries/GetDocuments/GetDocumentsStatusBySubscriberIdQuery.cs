using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.DocumentService.Application.Common.Exceptions;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Application.Document.Queries.GetDocument;
using OnyxDoc.DocumentService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.Documents.Queries.GetDocuments
{
    public class GetDocumentsStatusBySubscriberIdQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
        public int SystemSettingsId { get; set; }
    }

    public class GetDocumentStatusBySubscriberIdQueryHandler : IRequestHandler<GetDocumentsStatusBySubscriberIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetDocumentStatusBySubscriberIdQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(GetDocumentsStatusBySubscriberIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
               
                var documents = await _context.Documents.Where(a => a.SubscriberId == request.SubscriberId)
                    .ToListAsync();
                var systemSetting = await _authService.GetSystemSettingAsync(request.AccessToken, request.SubscriberId, request.UserId);
                if (documents == null || documents.Count() == 0)
                {
                    throw new NotFoundException(nameof(Document));
                }
                var documentsList = _mapper.Map<List<DocumentListDto>>(documents);
                var SystemSettingsEntity = systemSetting.entity;
                var systemSettingsModel = new SystemSettingDto
                {
                    Id = SystemSettingsEntity.Id,
                    ExpirationReminder = SystemSettingsEntity.ExpirationReminder,
                    ExpirationSettingsFrequency = SystemSettingsEntity.ExpirationSettingsFrequency,
                    ExpirationSettingsFrequencyDesc = SystemSettingsEntity.ExpirationSettingsFrequencyDesc,
                    ShouldSentDocumentsExpire = SystemSettingsEntity.ShouldSentDocumentsExpire,
                    DocumentExpirationPeriod = SystemSettingsEntity.DocumentExpirationPeriod,
                    WorkflowReminder = SystemSettingsEntity.WorkflowReminder,
                    WorkflowReminderSettingsFrequency = SystemSettingsEntity.WorkflowReminderSettingsFrequency,
                    WorkflowReminderSettingsFrequencyDesc = SystemSettingsEntity.WorkflowReminderSettingsFrequencyDesc,
                    Currency = SystemSettingsEntity.Currency,
                    Language = SystemSettingsEntity.Language,
                    Name = SystemSettingsEntity.Name,
                    SubscriberId = SystemSettingsEntity.SubscriberId,
                    CreatedByEmail = SystemSettingsEntity.CreatedByEmail,
                    CreatedById = SystemSettingsEntity.CreatedById,
                    CreatedDate = SystemSettingsEntity.CreatedDate,
                    LastModifiedById = SystemSettingsEntity.LastModifiedById,
                    LastModifiedByEmail = SystemSettingsEntity.LastModifiedByEmail,
                    Status = SystemSettingsEntity.Status,
                    StatusDesc = SystemSettingsEntity.StatusDesc


                };

                foreach (var item in systemSettingsModel.ExpirationReminder)
                {
                    
                    foreach (var document in documentsList)
                    {
                        var period = DateTime.Now - document.CreatedDate;

                        var frequencySetting = (int)item.ExpirationSettingsFrequency;
                        switch (frequencySetting)
                        {
                            case 1:
                                if (period.Days == systemSettingsModel.DocumentExpirationPeriod && systemSettingsModel.ShouldSentDocumentsExpire == true)
                                {
                                    document.DocumentStatus = Domain.Enums.DocumentStatus.Expired;
                                }
                                break;
                            case 2:
                                if ((period.Days / 7) == systemSettingsModel.DocumentExpirationPeriod && systemSettingsModel.ShouldSentDocumentsExpire == true)
                                {
                                    document.DocumentStatus = Domain.Enums.DocumentStatus.Expired;
                                }
                                break;
                            case 3:
                                if ((period.Days / 30) == systemSettingsModel.DocumentExpirationPeriod && systemSettingsModel.ShouldSentDocumentsExpire == true)
                                {
                                    document.DocumentStatus = Domain.Enums.DocumentStatus.Expired;
                                }
                                break;
                            case 4:
                                if ((period.Days / 365) == systemSettingsModel.DocumentExpirationPeriod && systemSettingsModel.ShouldSentDocumentsExpire == true)
                                {
                                    document.DocumentStatus = Domain.Enums.DocumentStatus.Expired;
                                }
                                break;
                            default:
                                break;
                        }


                    }

                }
                return Result.Success(documentsList);

            }
            catch (Exception ex)
            {
                return Result.Failure($"Error retrieving document by subscriber id {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
