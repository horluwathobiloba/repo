using MediatR;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.PaymentChannels.Commands.ChangePaymentChannel
{
    public class ChangePaymentChannelStatusCommand : IRequest<Result>
    {
        public int PaymentChannelId { get; set; }
        public string LoggedInUserId { get; set; }
    }

    public class ChangePaymentChannelStatusCommandHandler : IRequestHandler<ChangePaymentChannelStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public ChangePaymentChannelStatusCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<Result> Handle(ChangePaymentChannelStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var paymentChannel = await _context.PaymentChannels.FindAsync(request.PaymentChannelId);
                string message = "";
                switch (paymentChannel.Status)
                {
                    case Status.Active:
                        paymentChannel.Status = Status.Inactive;
                        message = "Payment Channel deactivation was successful";
                        break;
                    case Status.Inactive:
                        paymentChannel.Status = Status.Active;
                        message = "Payment Channel activation was successful";
                        break;
                    case Status.Deactivated:
                        paymentChannel.Status = Status.Active;
                        message = "Payment Channel activation was successful";
                        break;
                    default:
                        break;
                }
                _context.PaymentChannels.Update(paymentChannel);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("FAQ Category status updated successfully", paymentChannel);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Payment Channel status change was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
