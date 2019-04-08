namespace Arcadia.Ask.Controllers.Identity
{
    using System.Linq;
    using System.Security.Claims;

    using Arcadia.Ask.Auth.Permissions;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/permissions")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        [Authorize]
        public ActionResult<IPermissions> Get()
        {
            var role = this.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            var permissions = new PermissionsByRoleCreator(role).Permissions;

            return this.Ok(permissions);
        }
    }
}