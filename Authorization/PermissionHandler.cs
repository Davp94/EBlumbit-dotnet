using System;
using Microsoft.AspNetCore.Authorization;

namespace EBlumbit.Authorization;

public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        var hasPermission = context.User.HasClaim(c=>c.Type == "Permission" && c.Value == requirement.Permission);

        if(hasPermission)
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}
