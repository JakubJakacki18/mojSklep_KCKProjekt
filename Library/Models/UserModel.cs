using Library.Data;

namespace Library.Model
{
    public class UserModel
    {
        string Login { get; set; }
        string Password { get; set; }
        public RolesEnum Role { get; private set; }
    }


}
