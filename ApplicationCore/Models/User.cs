using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Dotnet.ApplicationCore.Models
{
    public class User : IdentityUser
    {
        [Display(Name = "First Name")]
        [StringLength(50, MinimumLength = 3)]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(50, MinimumLength = 3)]
        [Required]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required]
        public override string Email { get; set; }
    }
}
