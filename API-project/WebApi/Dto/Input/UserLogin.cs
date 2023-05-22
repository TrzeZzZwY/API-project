namespace WebApi.Dto.Input
{
    public class UserLogin
    {
        public UserLogin(string login, string password)
        {
            Login = login;
            Password = password;
        }

        public string Login { get; set; }
        public string Password { get; set; }
    }
}
