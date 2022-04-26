using System.ComponentModel.DataAnnotations;

namespace CollectionWebApp.ViewModels
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Compare("Password")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
