using System.ComponentModel.DataAnnotations;

namespace WebApi.Dto.Input
{
    public class UserRegister
    {
        public UserRegister(string login, string password, string email)
        {
            Login = login;
            Password = password;
            Email = email;
        }
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "User login can only contain letters, digits, and underscores.")]
        public string Login { get; set; }
        public string Password { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [StringLength(16, ErrorMessage = "Email must be between 5 and 50 characters", MinimumLength = 5)]
        [RegularExpression("^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$", ErrorMessage = "Email must be a valid email")]
        public string Email { get; set; }
    }
}
