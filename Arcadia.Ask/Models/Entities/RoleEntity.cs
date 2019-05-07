namespace Arcadia.Ask.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class RoleEntity
    {
        [Key]
        public Guid RoleId { get; set; }

        public string Name { get; set; }

        public IEnumerable<UserRoleEntity> UserRoles { get; set; }
    }
}