using System.ComponentModel.DataAnnotations;

namespace RoleBasedAuthSample.Models
{
    public class User
    {
        [Key]
        public string UserName { get; set; }
        public string Name { get; set; }
        public List<string>? Roles { get; set; }
        public int AccessId { get; set; }
        public bool IsActive { get; set; }
        public string? Token { get; set; }
        public string Password { get; set; }
        public User()
        {
            Roles = new List<string>();
        }
        public User(string userName, string name, string password, List<string>? roles,int accessid)
        {
            UserName = userName;
            Name = name;
            Password = password;
            Roles = roles;
            AccessId = accessid;
        }
    }

    public class LoginUser
    {
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
    }

    public class RegisterUser
    {
        public string Name { get; set; } = "";
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        public List<string>? Roles { get; set; }
        public  int  AccessId { get; set; }
    }
}
