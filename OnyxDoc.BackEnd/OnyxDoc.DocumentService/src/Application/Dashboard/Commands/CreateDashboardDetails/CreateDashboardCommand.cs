using AutoMapper;
/*using DocumentFormat.OpenXml.Bibliography;*/
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Domain.Enums;
using OnyxDoc.DocumentService.Domain.ViewModels;
using OnyxDoc.DocumentService.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.Dashboard.CreateDashboardDetails
{
    public class CreateDashboardCommand : AuthToken, IRequest<Result>
    {
        public string UserId { get; set; }
        public int SubscriberId { get; set; }
        public int Year { get; set; }
    }

    public class CreateDashboardCommandHandler : IRequestHandler<CreateDashboardCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IStringHashingService _stringHashingService;
        private readonly IAuthService _authService;

        public CreateDashboardCommandHandler(IApplicationDbContext context, IMapper mapper, IBlobStorageService blobStorageService,
                                                 IStringHashingService stringHashingService, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _blobStorageService = blobStorageService;
            _stringHashingService = stringHashingService;
            _authService = authService;
        }
        public async Task<Result> Handle(CreateDashboardCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                if (_authService.Subscriber == null)
                {
                    return Result.Failure(new string[] { "Invalid subscriber details specified." });
                }
                var documents = await _context.Documents.Where(x => x.SubscriberId == request.SubscriberId && x.SubscriberType == _authService.Subscriber.SubscriberTypeDesc).ToListAsync();
                var DocumentActivities = documents.Where(b => b.SubscriberId == request.SubscriberId && b.CreatedDate.Year == request.Year);
                var resultList = new List<DocumentStatsVM>();
                foreach (Month month in Enum.GetValues(typeof(Month)))
                {
                    var stat = new DocumentStatsVM();
                    //var c = DocumentActivities.Where(x => x.DocumentStatus == DocumentStatus.Completed && x.CreatedDate.Month == (int)month).Count();

                    stat.Month = month.ToString();
                    stat.Document = DocumentActivities.Where(x => x.DocumentStatus == DocumentStatus.Completed && x.CreatedDate.Month == (int)month).Count();
                    resultList.Add(stat);
                }
                var entity = new
                {
                    Draft = documents.Where(x =>x.DocumentStatus == DocumentStatus.Draft).Count(),
                    Sent = documents.Where(x =>x.DocumentStatus == DocumentStatus.Sent).Count(),
                    Expired = documents.Where(x => x.DocumentStatus == DocumentStatus.Expired).Count(),
                    WaitingForOthers = documents.Where(x => x.DocumentStatus == DocumentStatus.WaitingForOthers).Count(),
                    Completed = documents.Where(x => x.DocumentStatus == DocumentStatus.Completed).Count(),
                    TotalDocument = documents.Count()
                };
                return Result.Success("Retrieving Dashboard Details was successful", new { entity, resultList });
            }
            catch (Exception ex)
            {
                return Result.Failure($"Retrieving Dashboard details failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}


