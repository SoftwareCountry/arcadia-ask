namespace Arcadia.Ask.Models.Requests
{
    using System.ComponentModel.DataAnnotations;

    public class ModeratorSignInRequestModel
    {
        [Required]
        public string Login { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(20)]
        public string Password { get; set; }
    }
}