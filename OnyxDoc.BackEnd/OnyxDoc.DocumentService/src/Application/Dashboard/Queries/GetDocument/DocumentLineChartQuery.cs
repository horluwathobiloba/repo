using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Domain.Enums;
using OnyxDoc.DocumentService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.Dashboard.Queries.GetDocument
{
    public class DocumentLineChartQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int Year { get; set; }
        public string UserId { get; set; }
    }

    public class DocumentLineChartQueryHandler : IRequestHandler<DocumentLineChartQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public DocumentLineChartQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _authService = authService;
            _mapper = mapper;
            _context = context;
        }

        public async Task<Result> Handle(DocumentLineChartQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                if (_authService.Subscriber == null)
                {
                    return Result.Failure(new string[] { "Invalid subscriber details specified." });
                }
                
                var document = await _context.Documents.Where(x => x.SubscriberId == request.SubscriberId && x.CreatedDate.Year == request.Year && x.SubscriberType == _authService.Subscriber.SubscriberTypeDesc).ToListAsync();
                if (document == null || document.Count() == 0)
                {
                    
                    return Result.Failure(new string[] { "Fetching documents based on subscriber and year specified is null." });
                }

                var resultList = new List<ChartVm>();
                foreach (Month month in Enum.GetValues(typeof(Month)))
                {
                    var chart = new ChartVm();
                    chart.Month = month.ToString();
                    chart.Sent = document.Where(x => x.DocumentStatus == DocumentStatus.Sent && x.CreatedDate.Month == (int)month).Count();
                    chart.Awaiting = document.Where(x => x.DocumentStatus == DocumentStatus.WaitingForOthers && x.CreatedDate.Month == (int)month).Count();
                    chart.Completed = document.Where(x => x.DocumentStatus == DocumentStatus.Completed && x.CreatedDate.Month == (int)month).Count();
                    chart.Expired = document.Where(x => x.DocumentStatus == DocumentStatus.Expired && x.CreatedDate.Month == (int)month).Count();
                    resultList.Add(chart);

                }

                return Result.Success("Retrieving Dashboard Details was successful", resultList);
            }
            catch (Exception ex)
            {

                return Result.Failure($"Retrieving Document line chart details failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
