using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.ContractTypeInitiators.Commands.CreateContractTypeInitiator
{
    public class CreateContractTypeInitiatorCommandValidator:AbstractValidator<CreateContractTypeInitiatorCommand>
    {
        public CreateContractTypeInitiatorCommandValidator()
        {
            RuleFor(v => v.OrganisationId).GreaterThan(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.ContractTypeId).GreaterThan(0).WithMessage("A valid Contract type Initiator must be specified!");           
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!"); 
           // RuleFor(v => v.W).Must(AccountNo_Length).WithMessage("Account number Length must be equal or greater than 8");   
        }

        //private bool AccountNo_Length(string accountno)
        //{
        //    if (accountno.Length < 8)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}   
    }
}
