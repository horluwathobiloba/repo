using OnyxDoc.AuthService.Application.Common.Exceptions;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using OnyxDoc.AuthService.Application.Common.Models;
using System;
using AutoMapper;

namespace OnyxDoc.AuthService.Application.Users.Commands.ChangeUserStatus
{
    public class ChangeUserStatusCommand :  IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
       
    }

    public class ChangeUserStatusCommandHandler : IRequestHandler<ChangeUserStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public ChangeUserStatusCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<Result> Handle(ChangeUserStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _identityService.GetUserByIdAndSubscriber(request.UserId, request.SubscriberId);
                if (user.user == null)
                {
                    return Result.Failure(new string[] { "Unable to change status.Invalid User ID and Subscriber credentials!" });
                }
                var userStatusForChange = await _identityService.GetUserById(request.UserId);
                if (userStatusForChange.user == null)
                {
                    return Result.Failure(new string[] { "Invalid User for status change" });
                }
                userStatusForChange.user.UserId = request.UserId;
               var result =  await _identityService.ChangeUserStatusAsync(userStatusForChange.user);
                await _context.SaveChangesAsync(cancellationToken);
                if (result.Succeeded)
                    return Result.Success(result.Messages[0]);
                else
                    return Result.Failure(result.Messages);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "User status change was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
