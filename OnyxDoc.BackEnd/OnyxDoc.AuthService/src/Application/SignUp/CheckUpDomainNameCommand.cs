using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Application.Common.Models;
using OnyxDoc.AuthService.Application.Subscribers.Queries.GetSubscribers;
using OnyxDoc.AuthService.Domain.Entities;
using OnyxDoc.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.AuthService.Application.SignUp
{
    public class CheckUpDomainNameCommand: IRequest<Result>
    {
        public string Email { get; set; }
    }
    public class CheckUpDomainNameCommandHandler : IRequestHandler<CheckUpDomainNameCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        private readonly IDomainCheckService _domainCheckService;
        public CheckUpDomainNameCommandHandler(IApplicationDbContext context, IIdentityService identityService, IMapper mapper, IDomainCheckService domainCheckService)
        {
            _context = context;
            _identityService = identityService;
            _mapper = mapper;
            _domainCheckService = domainCheckService;
        }
        public async Task<Result> Handle(CheckUpDomainNameCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var domain = request.Email.Split("@", StringSplitOptions.None)[1];
                var existingDomains = await _domainCheckService.DomainExists(_context, SubscriberType.Corporate, domain);

                if (existingDomains.Count == 0 || existingDomains == null)
                {
                    return Result.Success("Domain name does not exist!!", null);
                }
                var subscriberDetails = _mapper.Map<List<SubscriberEntityVm>>(existingDomains);
                Dictionary<string, User> usersOnTheSystem = new Dictionary<string, User>();
                var userList = _identityService.GetAll(0, 0).Result.users;

                if (userList != null && userList.Count() > 0)
                {
                    usersOnTheSystem = userList.ToDictionary(a => a.UserId);
                }


                foreach (var data in subscriberDetails)
                {
                    data.Users = usersOnTheSystem.Values.Where(a => a.SubscriberId == data.Id)?.ToList();
                }
                
                return Result.Success("Domain name exist!", subscriberDetails);
            }

            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Check for domain name was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
