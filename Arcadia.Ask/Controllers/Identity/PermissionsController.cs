namespace Arcadia.Ask.Controllers.Identity
{
    using System.Linq;
    using System.Security.Claims;

    using Arcadia.Ask.Auth.Permissions;
    using Arcadia.Ask.Auth.Roles;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/permissions")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionsByRoleLoader permissionsLoader;

        public PermissionsController(IPermissionsByRoleLoader permissionsLoader)
        {
            this.permissionsLoader = permissionsLoader;
        }

        [HttpGet]
        [Route("")]
        [Authorize(Roles = RoleNames.User + ", " + RoleNames.Moderator)]
        public ActionResult<IPermissions> Get()
        {
            var role = this.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            var permissions = this.permissionsLoader.GetByRole(role);

            return this.Ok(permissions);
        }
    }
}