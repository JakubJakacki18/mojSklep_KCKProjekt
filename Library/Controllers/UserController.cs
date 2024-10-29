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

        public UserModel SignInOrUpSelection()
        {
            var isSignInDesire = _userView.LandingPage();
            (string, string) authenticationData;
            UserModel? result;
            if (!isSignInDesire)
            {
                do
                {
                    authenticationData = _userView.showSignUp();

                } while (!SignUp(authenticationData.Item1, authenticationData.Item2));
            }

            do
            {
                authenticationData = _userView.showSignIn();
                result = SignIn(authenticationData.Item1, authenticationData.Item2);
            } while (result == null);
            return result;
        }

        public UserModel? SignIn(string login, string password)
        {
            while (login != "admin" && password != "admin")
            {
                (login, password) = _userView.showSignIn();
            }
            return new UserModel { };

        }

        public bool SignUp(string login, string password)
        {
            //var newUserData = _userView.showSignUp();

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