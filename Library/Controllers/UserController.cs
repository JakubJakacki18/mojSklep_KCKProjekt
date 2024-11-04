using Library.Data;
using Library.Interfaces;
using Library.Model;

namespace KCKProjekt.Controllers
{
    public class UserController
    {
        private IUserView _userView;
        public UserController(IUserView userView)
        {
            _userView = userView;
        }

        public async Task<UserModel> SignInOrUpSelectionAsync()
        {
            var isSignInDesire = await _userView.LandingPage();
            (string, string) authenticationData;
            UserModel? result;
            if (!isSignInDesire)
            {
                do
                {
                    authenticationData = await _userView.ShowSignUp();

                } while (!SignUpAsync(authenticationData.Item1, authenticationData.Item2));
            }

            do
            {
                authenticationData = await _userView.ShowSignIn();
                result = await SignInAsync(authenticationData.Item1, authenticationData.Item2);
            } while (result == null);
            return result;
        }

        public async Task<UserModel?> SignInAsync(string login, string password)
        {
            while (login != "admin" && password != "admin")
            {
                (login, password) = await _userView.ShowSignIn(false);
            }
            return new UserModel { };

        }

        public bool SignUpAsync(string login, string password)
        {
            //var newUserData = _userView.ShowSignUp();

            return true;
        }
        public bool SignOut()
        {
            // some logic
            return true;
        }
        public bool ChangePassword(string login, string password)
        {
            // some logic
            return true;
        }
        public bool DeleteAccount(string login)
        {
            // some logic
            return true;
        }

        public RolesEnum GetRole(UserModel loggedUser)
        {
            return loggedUser.Role;
        }
    }
}