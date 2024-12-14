using Library.Data;

namespace Library.Interfaces
{
    public interface IUserView
    {
        public Task<(string, string)> ShowSignIn(bool isValid = true);
        public Task<(string, string)> ShowSignUp(bool isValid = true);
        public Task<bool> LandingPage();
        public Task<int> RoleSelection(RolesEnum roles);
    }
}
