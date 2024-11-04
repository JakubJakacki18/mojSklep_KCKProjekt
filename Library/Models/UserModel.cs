using Library.Data;

namespace Library.Model
{
    public class UserModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public RolesEnum Role { get; set; }
    }


}
