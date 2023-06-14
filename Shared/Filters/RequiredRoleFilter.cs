using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Shared.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RequiredRolesFilter : Attribute, IAuthorizationFilter
    {
        private readonly string[] _allowedRoles;
        public RequiredRolesFilter(params string[] allowedRoles)
        {
            _allowedRoles = allowedRoles;
        }

        /// <summary>
        /// Simply put, this works with the Authorise Filter to check that users
        /// have the required role to access the endpoint.
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {

            if (context.HttpContext.User?.Identity?.IsAuthenticated != true)
            {
                // User is not authenticated, return unauthorized result
                context.Result = new UnauthorizedResult();
                return;
            }

            // Check if the user has any of the allowed roles
            var hasAllowedRole = _allowedRoles.Any(role => context.HttpContext.User.IsInRole(role));

            if (!hasAllowedRole)
            {
                // User does not have any of the allowed roles, return forbidden result
                context.Result = new ForbidResult();
            }
        }
    }
}
