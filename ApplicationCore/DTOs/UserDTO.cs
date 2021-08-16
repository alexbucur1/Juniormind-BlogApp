using BlogApp.Dotnet.ApplicationCore.Models;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Dotnet.ApplicationCore.DTOs
{
    public class UserDTO
    {
        public UserDTO() { }

        public UserDTO (User user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
        }

        public string Id { get; set; }

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
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool IsAdmin { get; set; }
    }
}
