namespace Arcadia.Ask.Models.Requests
{
    using System.ComponentModel.DataAnnotations;

    public class ModeratorSignInRequestModel
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }
}