using KCKProjekt.Views.CLIViews;
using KCKProjekt.Views.ViewInterfaces;

namespace KCKProjekt.Controllers
{
    public class MainController
    {
        private IUserView _userView;
        private UserController _userController;
        public MainController()
        {
            switch (InterfaceView.ChoiceOfInterface())
            {
                default:
                    _userView = new UserCLIView();
                    break;
            }
            _userController = new UserController(_userView);
            _userController.SignIn();

        }
        public void Run()
        {

        }
    }
}
