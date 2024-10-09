using KCKProjekt.Views.ViewInterfaces;

namespace KCKProjekt.Controllers
{
    public class UserController
    {
        private IUserView _userView;
        public UserController(IUserView userView)
        {
            _userView = userView;
        }
        public void SignIn()
        {
            var authenticateDate = _userView.showSignIn();

            while (authenticateDate.Item1 != "admin" && authenticateDate.Item2 != "admin")
            {
                authenticateDate = _userView.showSignIn();
            }


        }
        public bool SignUp(string login, string password)
        {
            // some logic
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


    }
}
