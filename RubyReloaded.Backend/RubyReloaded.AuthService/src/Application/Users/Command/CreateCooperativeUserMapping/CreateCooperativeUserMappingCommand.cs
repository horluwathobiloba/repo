using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Application.Users.Command.VerifyInvitationCode;
using RubyReloaded.AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Users.Command.CreateCooperativeUserMapping
{
    //The user already Exist on the system Before 
    public class CreateCooperativeUserMappingCommand:IRequest<Result>
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public int CooperativeId { get; set; }
        public string Code { get; set; }
        //public CooperativeAccessStatus CooperativeAccessStatus { get; set; }
        public int RoleId { get; set; }
    }

    public class CreateCooperativeUserMappingCommandHandler : IRequestHandler<CreateCooperativeUserMappingCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IEmailService _emailService;

        public CreateCooperativeUserMappingCommandHandler(IApplicationDbContext context,IIdentityService identityService, IEmailService emailService)
        {
            _context = context;
            _identityService = identityService;
            _emailService = emailService;
        }
        public async Task<Result> Handle(CreateCooperativeUserMappingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var cooperative = await _context.Cooperatives.Include(x => x.CooperativeSetting).FirstOrDefaultAsync(x => x.Id == request.CooperativeId);
                if (cooperative is null)
                {
                    return Result.Failure("Cooperative Cannot be found");
                }

                var cooperativeSettings = cooperative.CooperativeSetting;
                var user = await _identityService.GetUserById(request.UserId);
                var verificationRequest = new VerifyInvitationCodeCommand
                {
                    Code = request.Code,
                    CooperativeId = request.CooperativeId,
                    Email = request.Email
                };
                var verifyCodeHandler = await new VerifyInvitationCodeCommandHandler(_context).Handle(verificationRequest, cancellationToken);
                if (!verifyCodeHandler.Succeeded)
                {
                    return Result.Failure(verifyCodeHandler.Message);
                }

                if (cooperativeSettings.RequestToJoin)
                {
                    await _context.BeginTransactionAsync();
                    var cooperativeUser = new CooperativeUserMapping
                    {
                        UserId = request.UserId,
                        Cooperative = cooperative,
                        CooperativeAccessStatus = CooperativeAccessStatus.Processing,
                        CooperativeId = cooperative.Id,
                        Email = user.user.Email,
                        CreatedDate = DateTime.Now,
                        Name = user.user.Name,
                        RoleId = request.RoleId,
                        Status = Domain.Enums.Status.Active,
                        StatusDesc = Domain.Enums.Status.Active.ToString()
                    };
                    await _context.CooperativeMembers.AddAsync(cooperativeUser);
                    await _context.SaveChangesAsync(cancellationToken);
                    var requestToJoin = new RequestToJoinTracker
                    {
                        AdminEmail = cooperativeSettings.AdminEmail,
                        Status = Domain.Enums.Status.Inactive,
                        StatusDesc = Domain.Enums.Status.Inactive.ToString(),
                        UserEmail = cooperativeUser.Email,
                        CooperativeAccessStatus = CooperativeAccessStatus.Processing,
                        CooperativeId = cooperative.Id,
                        Name = cooperativeUser.Email,
                        CreatedDate = DateTime.Now,
                    };
                    await _context.RequestToJoinTrackers.AddAsync(requestToJoin);
                    await _context.SaveChangesAsync(cancellationToken);
                    var email = new EmailVm
                    {
                        Application = "Ruby",
                        Subject = "Create User",
                        Text = "Request Access",
                        RecipientEmail = cooperativeSettings.AdminEmail,
                        RecipientName = user.user.FirstName,
                        DisplayButton = "User Created successfully",
                        Body1 = $"{user.user.Email} wants to join your cooporative",
                        Body2 = ""
                    };
                    var emailtouser = new EmailVm
                    {
                        Application = "Ruby",
                        Subject = "Request Access",
                        Text = "Request Access",
                        RecipientEmail = request.Email,
                        RecipientName = user.user.FirstName,
                        DisplayButton = "User Created successfully",
                        Body1 = $"Your request to join {cooperative.Name} has been sent",
                        Body2 = "",
                        Body="",
                        Body3=""
                    };
                    await _emailService.CooperativeSignUp(email);
                    await _emailService.SuccessfullVerification(emailtouser);
                    await _context.CommitTransactionAsync();
                    return Result.Success("User creation was successful and email for requesting access sent successfully", user);
                }

                var cooperativeUser2 = new CooperativeUserMapping
                {
                    UserId = request.UserId,
                    Cooperative = cooperative,
                    CooperativeAccessStatus = CooperativeAccessStatus.Approved,
                    CooperativeId = cooperative.Id,
                    Email = user.user.Email,
                    CreatedDate = DateTime.Now,
                    Name = user.user.Name,
                    RoleId = request.RoleId,
                    Status = Domain.Enums.Status.Active,
                    StatusDesc = Domain.Enums.Status.Active.ToString(),

                };
                await _context.CooperativeMembers.AddAsync(cooperativeUser2);
                await _context.SaveChangesAsync(cancellationToken);
                var email2 = new EmailVm
                {
                    Application = "Ruby",
                    Subject = "Cooperative Access Granted",
                    Text = "",
                    RecipientEmail = request.Email,
                    RecipientName = user.user.FirstName,
                    DisplayButton = "User Created successfully",
                    Body1 = $"You have successfully joined {cooperative.Name}",
                    Body2 = "",
                    Body = "",
                    Body3 = ""
                };
                await _emailService.CooperativeSignUp(email2);
                return Result.Success("User creation was successful", user);
            }
            catch (Exception ex)
            {
                _context.RollbackTransaction();
                return Result.Failure("User creation failed: "+ex.Message??ex.InnerException.Message);
            }
        }
    }
}
