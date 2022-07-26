using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.Dashboard.Queries.GetDocument
{
    public class DocumentDonutChartQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public DropDownType DropDownType { get; set; }
        public string UserId { get; set; }
    }

    public class DocumentDonutChartQueryHandler : IRequestHandler<DocumentDonutChartQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public DocumentDonutChartQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _authService = authService;
            _mapper = mapper;
            _context = context;
        }
        public async Task<Result> Handle(DocumentDonutChartQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                if (_authService.Subscriber == null)
                {
                    return Result.Failure(new string[] { "Invalid subscriber details specified." });
                }
                var document = await _context.Documents.Where(x => x.SubscriberId == request.SubscriberId && x.SubscriberType == _authService.Subscriber.SubscriberTypeDesc).ToListAsync();
                if (document == null || document.Count() == 0)
                {
                    return Result.Failure(new string[] { "No document was found for that subscriber and subscriber type." });
                }

                // Date allignment
                var today = DateTime.Today;
                var yesterday = today.AddDays(-1);
                var lastSevenDays = DateTime.Now.AddDays(-7);
                var lastThirtyDays = DateTime.Now.AddDays(-30);

                switch (request.DropDownType)
                {
                    case DropDownType.Today:
                        var documentForToday = document.Where(x => x.CreatedDate >= today);
                        var documentForTodayEntity = new
                        {
                            Sent = documentForToday.Where(x => x.DocumentStatus == DocumentStatus.Sent).Count(),
                            Expired = documentForToday.Where(x => x.DocumentStatus == DocumentStatus.Expired).Count(),
                            AwaitingSignatories = documentForToday.Where(x => x.DocumentStatus == DocumentStatus.WaitingForOthers).Count(),
                            Completed = documentForToday.Where(x => x.DocumentStatus == DocumentStatus.Completed).Count(),
                        };
                        return Result.Success("Retrieving Dashboard Details was successful", documentForTodayEntity);
                    case DropDownType.Yesterday:
                        var documentForYesterday = document.Where(x => x.CreatedDate > yesterday && x.CreatedDate < today);
                        var documentForYesterdayEntity = new
                        {
                            Sent = documentForYesterday.Where(x => x.DocumentStatus == DocumentStatus.Sent).Count(),
                            Expired = documentForYesterday.Where(x => x.DocumentStatus == DocumentStatus.Expired).Count(),
                            AwaitingSignatories = documentForYesterday.Where(x => x.DocumentStatus == DocumentStatus.WaitingForOthers).Count(),
                            Completed = documentForYesterday.Where(x => x.DocumentStatus == DocumentStatus.Completed).Count(),
                        };
                        return Result.Success("Retrieving Dashboard Details was successful", documentForYesterdayEntity);
                    case DropDownType.SevenDays:
                        var documentForSevenDays = document.Where(x => x.CreatedDate > lastSevenDays);
                        var documentForSevenDaysEntity = new
                        {
                            Sent = documentForSevenDays.Where(x => x.DocumentStatus == DocumentStatus.Sent).Count(),
                            Expired = documentForSevenDays.Where(x => x.DocumentStatus == DocumentStatus.Expired).Count(),
                            AwaitingSignatories = documentForSevenDays.Where(x => x.DocumentStatus == DocumentStatus.WaitingForOthers).Count(),
                            Completed = documentForSevenDays.Where(x => x.DocumentStatus == DocumentStatus.Completed).Count(),
                        };
                        return Result.Success("Retrieving Dashboard Details was successful", documentForSevenDaysEntity);
                    case DropDownType.ThirtyDays:
                        var documentForThirtyDays = document.Where(x => x.CreatedDate > lastThirtyDays);
                        var documentForThirtyDaysEntity = new
                        {
                            Sent = documentForThirtyDays.Where(x => x.DocumentStatus == DocumentStatus.Sent).Count(),
                            Expired = documentForThirtyDays.Where(x => x.DocumentStatus == DocumentStatus.Expired).Count(),
                            AwaitingSignatories = documentForThirtyDays.Where(x => x.DocumentStatus == DocumentStatus.WaitingForOthers).Count(),
                            Completed = documentForThirtyDays.Where(x => x.DocumentStatus == DocumentStatus.Completed).Count(),
                        };
                        return Result.Success("Retrieving Dashboard Details was successful", documentForThirtyDaysEntity);
                    case DropDownType.ThisYear:
                        var documentForThisYear = document.Where(x => x.CreatedDate.Year == DateTime.Now.Year);
                        var documentForThisYearEntity = new
                        {
                            Sent = documentForThisYear.Where(x => x.DocumentStatus == DocumentStatus.Sent).Count(),
                            Expired = documentForThisYear.Where(x => x.DocumentStatus == DocumentStatus.Expired).Count(),
                            AwaitingSignatories = documentForThisYear.Where(x => x.DocumentStatus == DocumentStatus.WaitingForOthers).Count(),
                            Completed = documentForThisYear.Where(x => x.DocumentStatus == DocumentStatus.Completed).Count(),
                        };
                        return Result.Success("Retrieving Dashboard Details was successful", documentForThisYearEntity);
                    default:
                        return Result.Failure("Invalid dropdown type");
                }

            }
            catch (Exception ex)
            {

                return Result.Failure($"Get document activities (doughnut chart) by month failed. Error: { ex?.Message + " " + ex?.InnerException.Message}");
            }
        }
    }
}
