using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Shared.Filters
{
    /// <summary>
    /// Specifies that the decorated method requires certain roles for access.
    /// This attribute works in conjunction with the Authorize Filter to check
    /// if the user has the required role to access the endpoint.
    /// </summary>
    /// <remarks>
    /// When applied to a method, the RequiredRolesFilter checks if the user is
    /// authenticated. If not authenticated, an UnauthorizedResult is returned.
    /// If the user is authenticated, it verifies if the user has any of the allowed roles.
    /// If the user does not have any of the allowed roles, a NotFoundResult is returned.
    /// </remarks>
    /// <seealso cref="IAuthorizationFilter" />
    /// <example>
    /// <code>
    /// [RequiredRolesFilter("Admin", "Moderator")]
    /// public IActionResult SomeSecuredEndpoint()
    /// {
    /// // Endpoint logic here
    /// }
    /// </code>
    /// </example>
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
                // User does not have any of the allowed roles, return NotFound result
                context.Result = new NotFoundResult();
            }
        }
    }
}
