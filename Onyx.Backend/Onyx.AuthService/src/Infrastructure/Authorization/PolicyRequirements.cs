
using Microsoft.AspNetCore.Authorization;
using Onyx.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Onyx.AuthService.Infrastructure.Authorization
{
   public class PolicyRequirements
    {
        public class ShouldHaveSuperAdminAccessRequirement : IAuthorizationRequirement
        {
        }
        public class ShouldHaveAdminAccessRequirement : IAuthorizationRequirement
        {
        }
        public class ShouldHaveSalesOfficerAccessRequirement : IAuthorizationRequirement
        {
        }
        public class ShouldHavePowerUserAccessRequirement : IAuthorizationRequirement
        {
        }
        public class ShouldHaveSupportAccessRequirement : IAuthorizationRequirement
        {
        }
        public class ShouldHaveSuperAdminAccessRequirementHandler : AuthorizationHandler<ShouldHaveSuperAdminAccessRequirement>
        {
            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ShouldHaveSuperAdminAccessRequirement requirement)
            {

                if (!context.User.HasClaim(x => x.Type == "Access Level"))
                    return Task.CompletedTask;

                var claim = context.User.Claims.FirstOrDefault(x => x.Type == "Access Level");
                var acessLevel = claim.Value;

                if (acessLevel == AccessLevel.SuperAdmin.ToString())
                {
                    context.Succeed(requirement);
                }

                return Task.CompletedTask;
            }
        }

        public class ShouldBeAnAdminRequirementHandler : AuthorizationHandler<ShouldHaveAdminAccessRequirement>
        {
            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ShouldHaveAdminAccessRequirement requirement)
            {

                if (!context.User.HasClaim(x => x.Type == "Access Level"))
                    return Task.CompletedTask;

                var claim = context.User.Claims.FirstOrDefault(x => x.Type == "Access Level");
                var acessLevel = claim.Value;

                if (acessLevel == AccessLevel.Admin.ToString())
                {
                    context.Succeed(requirement);
                }

                return Task.CompletedTask;
            }
        }

        public class ShouldHaveSalesOfficerAccessRequirementHandler : AuthorizationHandler<ShouldHaveSalesOfficerAccessRequirement>
        {
            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ShouldHaveSalesOfficerAccessRequirement requirement)
            {

                if (!context.User.HasClaim(x => x.Type == "Access Level"))
                    return Task.CompletedTask;

                var claim = context.User.Claims.FirstOrDefault(x => x.Type == "Access Level");
                var acessLevel = claim.Value;

                //if (acessLevel == AccessLevel.Sales.ToString())
                //{
                //    context.Succeed(requirement);
                //}

                return Task.CompletedTask;
            }
        }

        public class ShouldHavePowerUserAccessRequirementHandler : AuthorizationHandler<ShouldHavePowerUserAccessRequirement>
        {
            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ShouldHavePowerUserAccessRequirement requirement)
            {

                if (!context.User.HasClaim(x => x.Type == "Access Level"))
                    return Task.CompletedTask;

                var claim = context.User.Claims.FirstOrDefault(x => x.Type == "Access Level");
                var acessLevel = claim.Value;

                //if (acessLevel == AccessLevel.PowerUsers.ToString())
                //{
                //    context.Succeed(requirement);
                //}

                return Task.CompletedTask;
            }
        }

        public class ShouldHaveSupportAccessRequirementHandler : AuthorizationHandler<ShouldHaveSupportAccessRequirement>
        {
            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ShouldHaveSupportAccessRequirement requirement)
            {
                if (!context.User.HasClaim(x => x.Type == "Access Level"))
                    return Task.CompletedTask;

                var claim = context.User.Claims.FirstOrDefault(x => x.Type == "Access Level");
                var acessLevel = claim.Value;

                if (acessLevel == AccessLevel.Support.ToString())
                {
                    context.Succeed(requirement);
                }

                return Task.CompletedTask;
            }
        }
    }
}

