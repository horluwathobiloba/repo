using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.Notifications.Commands.CreateNotifications
{
    public class CreateNotificationsCommand:IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public List<NotificationRequest> Notifications { get; set; }
        public string UserId { get; set; }
    }

    public class CreateNotificationsCommandHandler : IRequestHandler<CreateNotificationsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreateNotificationsCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async  Task<Result> Handle(CreateNotificationsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                var list = new List<Notification>();

                await _context.BeginTransactionAsync();

                foreach (var notification in request.Notifications)
                {
                    var entity = new Notification
                    {
                        OrganisationId = request.OrganisationId,
                        NotificationStatus = NotificationStatus.Unread,
                        CustomerId = notification.CustomerId,
                        Message = notification.Message,
                        CreatedBy = request.UserId,
                        CreatedDate = DateTime.Now,
                        Status = Status.Active
                    };
                    list.Add(entity);
                }
                await _context.Notifications.AddRangeAsync(list);
                await _context.SaveChangesAsync(cancellationToken);

                await _context.CommitTransactionAsync();

                return Result.Success("Notifications created successfully!");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Notifications creation failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
    }

