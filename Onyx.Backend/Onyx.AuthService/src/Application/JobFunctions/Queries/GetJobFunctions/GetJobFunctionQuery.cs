////using AutoMapper;
////using MediatR;
////using Microsoft.EntityFrameworkCore;
////using Onyx.AuthService.Application.Common.Interfaces;
////using Onyx.AuthService.Application.Common.Models;
////using System;
////using System.Collections.Generic;
////using System.Text;
////using System.Threading;
////using System.Threading.Tasks;

////namespace Onyx.AuthService.Application.JobFunctions.Queries.GetJobFunction
////{
////    public class GetJobFunctionQuery:IRequest<Result>
////    {

////    }
////    public class GetJobFunctionQueryHandler : IRequestHandler<GetJobFunctionQuery, Result>
////    {
////        private readonly IApplicationDbContext _context;
////        private readonly IIdentityService _identityService;
////        private readonly IMapper _mapper;

////        public GetJobFunctionQueryHandler(IApplicationDbContext context, IIdentityService identityService, IMapper mapper)
////        {
////            _context = context;
////            _mapper = mapper;
////            _identityService = identityService;
////        }

////        public async Task<Result> Handle(GetJobFunctionQuery request, CancellationToken cancellationToken)
////        {
////            try
////            {
////                var jobs = await _context.JobFunctions.ToListAsync();
////                return Result.Success(jobs);
////            }
////            catch (Exception ex)
////            {
////                return Result.Failure(new string[] { "Error retrieving staff by Id: ", ex?.Message ?? ex?.InnerException?.Message });
////            }
////        }
////    }

////}
