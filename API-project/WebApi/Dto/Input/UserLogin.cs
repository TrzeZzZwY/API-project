using System.ComponentModel.DataAnnotations;

namespace WebApi.Dto.Input
{
    public class UserLogin
    {
        public UserLogin(string login, string password)
        {
            Login = login;
            Password = password;
        }
        [Required(ErrorMessage = "Login is required.")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
