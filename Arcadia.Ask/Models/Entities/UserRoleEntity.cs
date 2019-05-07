namespace Arcadia.Ask.Models.Entities
{
    using System;

    public class UserRoleEntity
    {
        public string UserLogin { get; set; }

        public UserEntity User { get; set; }

        public Guid RoleId { get; set; }

        public RoleEntity Role { get; set; }
    }
}