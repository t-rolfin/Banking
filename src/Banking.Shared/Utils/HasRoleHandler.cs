using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Shared.Utils
{
    public class HasRoleHandler : AuthorizationHandler<HasRoleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasRoleRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.Role))
                return Task.CompletedTask;

            var role = context.User.FindFirst(c => c.Type == ClaimTypes.Role)?.Value;

            if (role == requirement.Role)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
