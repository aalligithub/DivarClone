using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace DivarClone.Attributes
{
    public class RoleOrPermissionAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public string Role { get; set; }
        public string Permission { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            // Check if the user is authenticated
            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var claimsIdentity = user.Identity as ClaimsIdentity;

            if (claimsIdentity != null)
            {
                // Check if the user is in the specified role
                if (!string.IsNullOrEmpty(Role) && user.IsInRole(Role))
                {
                    return; // Authorized by role
                }

                // Check if the user has the required permission claim
                if (!string.IsNullOrEmpty(Permission))
                {
                    var hasPermission = claimsIdentity.HasClaim(claim =>
                        claim.Type == CustomClaims.Permission && claim.Value == Permission);

                    if (hasPermission)
                    {
                        return; // Authorized by permission claim
                    }
                }
            }

            // If neither role nor permission is satisfied, deny access
            context.Result = new ForbidResult();
        }
    }
}
