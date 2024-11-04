using Library.Data;

namespace Library.Model
{
    public class UserModel
    {
        public required string Login { get; set; }
        public required string Password { get; set; }
        public RolesEnum Role { get; set; }
    }


}
