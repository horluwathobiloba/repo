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
    public class GetActivitiesQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public DropDownType DropDownType { get; set; }
        public string UserId { get; set; }

    }

    public class GetActivitiesQueryHandler : IRequestHandler<GetActivitiesQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetActivitiesQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _authService = authService;
            _mapper = mapper;
            _context = context;
        }
        public async Task<Result> Handle(GetActivitiesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                if (_authService.Subscriber == null)
                {
                    return Result.Failure(new string[] { "Invalid subscriber details specified." });
                }
                var getInbox = await _context.Inboxes.Where(x => x.SubscriberId == request.SubscriberId).OrderByDescending(c => c.CreatedDate).ToListAsync();
                var getAuditTrail = await _context.AuditTrails.Where(x => x.SubscriberId == request.SubscriberId).OrderByDescending(b => b.CreatedDate).ToListAsync();

                // Date allignment
                var today = DateTime.Today;
                var yesterday = today.AddDays(-1);
                var lastSevenDays = DateTime.Now.AddDays(-7);
                var lastThirtyDays = DateTime.Now.AddDays(-30);

                IEnumerable<Domain.Entities.Inbox> inboxData;
                IEnumerable<Domain.Entities.AuditTrail> auditData;

                switch (request.DropDownType)
                {
                    case DropDownType.Today:
                        inboxData = getInbox.Where(x => x.CreatedDate >= today);
                        auditData = getAuditTrail.Where(x => x.CreatedDate >= today);
                        var activitiesForTodayEntity = new
                        {
                            inboxData,
                            auditData
                        };
                        return Result.Success("Retrieving Today's Dashboard Details was successful", activitiesForTodayEntity);
                    case DropDownType.Yesterday:
                        inboxData = getInbox.Where(x => x.CreatedDate > yesterday && x.CreatedDate < today);
                        auditData = getAuditTrail.Where(x => x.CreatedDate > yesterday && x.CreatedDate < today);
                        var activitiesForYesterdayEntity = new
                        {
                            inboxData,
                            auditData
                        };
                        return Result.Success("Retrieving yesterday's Dashboard Details was successful", activitiesForYesterdayEntity);
                    case DropDownType.SevenDays:
                        inboxData = getInbox.Where(x => x.CreatedDate > lastSevenDays);
                        auditData = getAuditTrail.Where(x => x.CreatedDate > lastSevenDays);
                        var activitiesForLastSevenDaysEntity = new
                        {
                            inboxData,
                            auditData
                        };
                        return Result.Success("Retrieving las seven days Dashboard Details was successful", activitiesForLastSevenDaysEntity);
                    case DropDownType.ThirtyDays:
                        inboxData = getInbox.Where(x => x.CreatedDate > lastThirtyDays);
                        auditData = getAuditTrail.Where(x => x.CreatedDate > lastThirtyDays);
                        var activitiesForLastThirtyDaysEntity = new
                        {
                            inboxData,
                            auditData
                        };
                        return Result.Success("Retrieving last thirty days Dashboard Details was successful", activitiesForLastThirtyDaysEntity);
                    case DropDownType.ThisYear:
                        inboxData = getInbox.Where(x => x.CreatedDate.Year == DateTime.Now.Year);
                        auditData = getAuditTrail.Where(x => x.CreatedDate.Year == DateTime.Now.Year);
                        var activitiesForThisYearEntity = new
                        {
                            inboxData,
                            auditData
                        };
                        return Result.Success("Retrieving Dashboard Details was successful", activitiesForThisYearEntity);
                    default:
                        return Result.Failure("Invalid dropdown type selected");
                }

            }
            catch (Exception ex)
            {

                return Result.Failure($"Get activities by month failed. Error: { ex?.Message + " " + ex?.InnerException.Message}");
            }
        }
    }
}
