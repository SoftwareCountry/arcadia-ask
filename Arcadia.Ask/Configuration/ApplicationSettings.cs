namespace Arcadia.Ask.Configuration
{
    using System.ComponentModel.DataAnnotations;

    public class ApplicationSettings
    {
        [Required]
        public AuthSettings AuthSettings { get; set; }
    }
}
