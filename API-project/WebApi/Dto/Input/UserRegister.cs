namespace WebApi.Dto.Input
{
    public class UserRegister
    {
        public UserRegister(string login, string password, string name, string email)
        {
            Login = login;
            Password = password;
            Name = name;
            Email = email;
        }

        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
