using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.LicenseTypes.Queries.GetLicenseTypes;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using ReventInject;
using System;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.LicenseTypes.Commands.CreateLicenseType
{
    public class CreateLicenseTypeCommand : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; } 
    }

    public class CreateLicenseTypeCommandHandler : IRequestHandler<CreateLicenseTypeCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly INotificationService _notificationService;

        public CreateLicenseTypeCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService,INotificationService notificationService)
        {
            _authService = authService;            
            _context = context;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        public async Task<Result> Handle(CreateLicenseTypeCommand request, CancellationToken cancellationToken)
        {
            try
            { 
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);              
                
                var exists = await _context.LicenseTypes.AnyAsync(x => x.OrganisationId == request.OrganisationId && x.Name.ToLower().Trim() == request.Name.ToLower().Trim());

                if (exists)
                {
                    return Result.Failure($"License type name already exists!");
                }

                var entity = new Domain.Entities.LicenseType
                {
                    Name = request.Name,
                    OrganisationId = request.OrganisationId,
                    OrganisationName = _authService.Organisation?.Name,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
                };

                await _context.LicenseTypes.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);
                var notification = new Notification
                {
                   
                    Message = "",
                    NotificationStatus = NotificationStatus.Unread,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now
                };
                await _context.Notifications.AddAsync(notification);
                await _notificationService.SendNotification(request.UserId, notification.Message);
                var result = _mapper.Map<LicenseTypeDto>(entity);
                return Result.Success("LicenseType created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"LicenseType creation failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
