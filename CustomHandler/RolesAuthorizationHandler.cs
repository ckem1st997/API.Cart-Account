using API.Cart_Account.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Cart_Account.CustomHandler
{
    public class RolesAuthorizationHandler : AuthorizationHandler<RolesAuthorizationRequirement>
    {
        public readonly UserService _context;
        public RolesAuthorizationHandler(UserService context)
        {
            _context = context;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       RolesAuthorizationRequirement requirement)
        {

            if (context.User == null || !context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return Task.CompletedTask;
            }
            var validRole = false;
            if (requirement.AllowedRoles == null || !requirement.AllowedRoles.Any())
            {

                validRole = true;
            }
            else
            {
                var claims = context.User.Claims;
                var id = claims.FirstOrDefault(c => c.Type == "id").Value;
                // quyền nhập vào
                var roles = requirement.AllowedRoles;
              //  Console.WriteLine(_context);
                var user = _context.Get(id);
                validRole = roles.Contains(user.Role);

            }

            if (validRole)
            {
                context.Succeed(requirement);

            }
            else
            {
                context.Fail();
            }
            return Task.CompletedTask;
        }
    }
}
