using Onyx.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Onyx.AuthService.Application.Common.Interfaces
{
    public interface IEmailService
    {  
        
        Task<string> OrganizationSignUp(EmailVm emailVm);
        Task<string> EmailVerification(EmailVm emailVm);
        Task<string> SuccessfullVerification(EmailVm emailVm);
        Task<string> ContractRequestVerification(EmailVm emailVm);
        Task<string> SendForgotPasswordEmailAsync(EmailVm emailVm);
        Task<string> ResetPasswordEmailAsync(EmailVm emailVm);
        Task<string> ChangePasswordEmail(EmailVm emailVm);
        Task<string> AdminEmailVerification(EmailVm emailVm);
        Task<string> ContractRequestApproval(EmailVm emailVm);


        // Task<string> SendOrganizationCreationNotification(EmailVm emailVm);


    }
}
