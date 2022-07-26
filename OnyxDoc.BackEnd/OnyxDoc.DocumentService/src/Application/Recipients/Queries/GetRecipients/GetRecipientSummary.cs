//using AutoMapper;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using OnyxDoc.DocumentService.Application.Common.Interfaces;
//using OnyxDoc.DocumentService.Application.Common.Models;
//using OnyxDoc.DocumentService.Domain.Enums;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace OnyxDoc.DocumentService.Application.WorkflowPhases.Queries.GetWorkflowPhases
//{
//    public class GetRecipientSummaryQuery : IRequest<Result>
//    {
//        public int SubscriberId { get; set; }
//    }

//    public class GetRecipientSummaryQueryHandler : IRequestHandler<GetRecipientSummaryQuery, Result>
//    {
//        private readonly IApplicationDbContext _context;
//        private readonly IMapper _mapper;
//        public GetRecipientSummaryQueryHandler(IApplicationDbContext context, IMapper mapper)
//        {
//            _context = context;
//            _mapper = mapper;
//        }

//        public async Task<Result> Handle(GetRecipientSummaryQuery request, CancellationToken cancellationToken)
//        {
//            // To do: refactor this code
//            var approved = await _context.WorkflowPhases.Where(x => x.SubscriberId == request.SubscriberId).CountAsync();
//            var  = await _context.WorkflowPhases.Where(x => x.SubscriberId == request.SubscriberId).ToListAsync();
//            var total = .Count;
//            var inProgress = await _context.WorkflowPhases.Where(x => x.SubscriberId == request.SubscriberId).CountAsync();
//            var expired = await _context.WorkflowPhases.Where(x => x.SubscriberId == request.SubscriberId).CountAsync();
//            var pending = await _context.WorkflowPhases.Where(x => x.SubscriberId == request.SubscriberId).CountAsync();
//            var disApproved = await _context.WorkflowPhases.Where(x => x.SubscriberId == request.SubscriberId).CountAsync();

//            return Result.Success(
//                new
//                {
//                    total = total,
//                    inProgress = inProgress,
//                    expired = expired,
//                    pending = pending,
//                    approved = approved,
//                    disApproved = disApproved
//                });

//        }
//    }
//}
