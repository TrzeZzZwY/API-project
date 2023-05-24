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

        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
