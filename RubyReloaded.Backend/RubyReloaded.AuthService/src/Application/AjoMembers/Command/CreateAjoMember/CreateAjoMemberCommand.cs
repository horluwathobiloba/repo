using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Ajos.Command.VerifyAjoInvitationCode;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Entities;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.AjoMembers.Command.CreateAjoMember
{
    public class CreateAjoMemberCommand:IRequest<Result>
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public int AjoId { get; set; }
        public int RoleId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }

    public class CreateAjoMemberCommandHandler : IRequestHandler<CreateAjoMemberCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        public CreateAjoMemberCommandHandler(IApplicationDbContext context,IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }
        public async Task<Result> Handle(CreateAjoMemberCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //Find the user
                var user = await _identityService.GetUserById(request.UserId);
                if (user.user is null)
                {
                    return Result.Failure("User does not exist");
                }
                //check if the user already exist in this Ajo Account
                var exist = await _context.AjoMembers.AnyAsync(a => a.UserId == request.UserId && a.AjoId == request.AjoId);
                if (exist)
                {
                    return Result.Success("User Alredy Exist In This Ajo");
                }
                var ajo = await _context.Ajos.FirstOrDefaultAsync(x => x.Id == request.AjoId);
                var verificationRequest = new VerifyAjoInvitationCodeCommand
                {
                    AjoId = request.AjoId,
                    Code = request.Code,
                    Email = request.Email
                };
                var verifyCodeHandler = await new VerifyAjoInvitationCodeCommandHandler(_context).Handle(verificationRequest, cancellationToken);
                if (!verifyCodeHandler.Succeeded)
                {
                    return Result.Failure(verifyCodeHandler.Message);
                }

                var ajoMember = new AjoMember
                {
                    Name = request.Name,
                    Status = Domain.Enums.Status.Active,
                    StatusDesc = Status.Active.ToString(),
                    Ajo = ajo,
                    AjoId = request.AjoId,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    Email = user.user.Email,
                    UserId = request.UserId,
                    RoleId = request.RoleId,
                };
                await _context.AjoMembers.AddAsync(ajoMember);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success(ajoMember);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Ajo Member adding was not successful", ex?.Message ?? ex?.InnerException.Message });
            }

        }
    }
}
