using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.Contracts.Commands.ContractEmailNotification
{
    public class ContractEmailNotificationCommandValidator : AbstractValidator<ContractEmailNotificationCommand>
    {
        public ContractEmailNotificationCommandValidator()
        {
          
        }
    }
}
