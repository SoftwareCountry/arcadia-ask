namespace Arcadia.Ask.Configuration
{
    using System.ComponentModel.DataAnnotations;

    public class AuthSettings : IAuthSettings
    {
        [Required]
        public string UserCookieName { get; set; }
    }
}