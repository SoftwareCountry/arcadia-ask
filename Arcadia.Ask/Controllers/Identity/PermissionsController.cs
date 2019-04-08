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
        private readonly IPermissionsByRoleCreator permissionsCreator;

        public PermissionsController(IPermissionsByRoleCreator permissionsCreator)
        {
            this.permissionsCreator = permissionsCreator;
        }

        [HttpGet]
        [Route("")]
        [Authorize]
        public ActionResult<IPermissions> Get()
        {
            var role = this.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            var permissions = this.permissionsCreator.Create(role);

            return this.Ok(permissions);
        }
    }
}