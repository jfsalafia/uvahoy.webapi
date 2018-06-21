using System.ComponentModel.DataAnnotations;

namespace uvahoy.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}