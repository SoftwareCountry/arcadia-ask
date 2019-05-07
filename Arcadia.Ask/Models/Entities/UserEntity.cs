namespace Arcadia.Ask.Models.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class UserEntity
    {
        [Key]
        public string Login { get; set; }

        public string Hash { get; set; }
    }
}