using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.AuthService.Application.Common.Exceptions;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.AuthService.Application.SystemSetting.Queries.GetSystemSetting
{
    public class GetSystemSettingByIdQuery : IRequest<Result>
    {
        public int Id { get; set; }
        public string UserId { get; set; }
    }


    public class GetSystemSettingByIdQueryHandler : IRequestHandler<GetSystemSettingByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;

        public GetSystemSettingByIdQueryHandler(IApplicationDbContext context, IMapper mapper, IIdentityService identityService)
        {
            _context = context;
            _mapper = mapper;
            _identityService = identityService;
        }

        public async Task<Result> Handle(GetSystemSettingByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var userCheck = await _identityService.GetUserById(request.UserId);

                if (userCheck.user == null)
                {
                    return Result.Failure("Invalid Subscriber and User Specified");
                }
                var entity = await _context.SystemSettings.Where(a =>  a.Id == request.Id && a.Status == Domain.Enums.Status.Active)
                    .FirstOrDefaultAsync();
                if (entity == null)
                {
                    throw new NotFoundException(nameof(SystemSetting), request.Id);
                }
                var result = _mapper.Map<SystemSettingDto>(entity);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error retrieving system setting by id {ex?.Message ?? ex?.InnerException?.Message}");
            }

        }
    }
}
