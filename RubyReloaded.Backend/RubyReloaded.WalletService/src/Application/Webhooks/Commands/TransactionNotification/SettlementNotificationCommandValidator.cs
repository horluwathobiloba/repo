using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.Webhooks.Commands.SettlementNotificationCommand
{
    public class SettlementNotificationCommandValidator : AbstractValidator<SettlementNotificationCommand>
    {
        public SettlementNotificationCommandValidator()
        {
           
        }
    }
}
