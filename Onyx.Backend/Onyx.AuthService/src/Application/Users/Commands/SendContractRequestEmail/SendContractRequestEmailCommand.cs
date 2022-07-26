
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using Onyx.AuthService.Domain.Entities;
using Onyx.AuthService.Application.Common.Models;
using Onyx.AuthService.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Onyx.AuthService.Application.Users.Commands.SendContractRequestEmail
{
    public class SendContractRequestEmailCommand : IRequest<Result>
    {
        public string[] Email { get; set; }
    }

    public class SendContractRequestEmailCommandHandler : IRequestHandler<SendContractRequestEmailCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        public SendContractRequestEmailCommandHandler(IApplicationDbContext context, IIdentityService identityService,
                                          IMapper mapper, IConfiguration configuration, IEmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _identityService = identityService;
            _configuration = configuration;
            _emailService = emailService;
        }
        public async Task<Result> Handle(SendContractRequestEmailCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Email != null)
                {
                    foreach (var item in request.Email)
                    {
                        var resultEmail = await _identityService.GetUserByEmail(item);
                        if (resultEmail.staff == null)
                        {
                            continue;
                        }
                        //get role by rolename
                        var role = await _context.Roles.Where(a => a.Id == resultEmail.staff.Id).FirstOrDefaultAsync();

                        //if (role == null || role.Name.ToUpper() != "Legal".ToUpper())
                        //{
                        //    continue;
                        //}

                        string webDomain = _configuration["WebDomain"];
                        var email = new EmailVm
                        {
                            Application = "Onyx",
                            Subject = "Contract Request",
                            BCC = "",
                            CC = "",
                            Text = "",
                            RecipientEmail = item,
                            Body = "A contract Request has been initiated!",
                            Body1 = "Click the button below to login to view the contract",
                            ButtonText = "Click Here",
                            ButtonLink = webDomain + $"login"
                        };
                        await _emailService.ContractRequestApproval(email);
                    }
                }
              
               
                return Result.Success("Contract Request email sent successfully");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Error sending email to legal", ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
  
