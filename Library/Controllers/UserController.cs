using Library.Data;
using Library.Interfaces;
using Library.Model;

namespace Library.Controllers
{
    public class UserController
    {
        private IUserView _userView;
        private IBuyerView _buyerView;
        private ISellerView _sellerView;
        private IAdminView _adminView;
        private IUserRepository _userRepository;
        public UserController(IUserView userView, IUserRepository userRepository)
        {
            _userView = userView;
            _userRepository = userRepository;
        }

        public async Task<UserModel> SignInOrUpSelectionAsync()
        {
            var isSignInDesire = await _userView.LandingPage();
            (string, string) authenticationData;
            UserModel? result;
            if (!isSignInDesire)
            {
                bool isSignUpSuccess = true;
                do
                {
                    authenticationData = await _userView.ShowSignUp(isSignUpSuccess);
                    isSignUpSuccess = SignUpAsync(authenticationData.Item1, authenticationData.Item2);
                } while (!isSignUpSuccess);
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
            UserModel? user = _userRepository.GetUser(login, password);
            while (user == null)
            {
                (login, password) = await _userView.ShowSignIn(false);
                user = _userRepository.GetUser(login, password);
            }
            return user;

        }

        public bool SignUpAsync(string login, string password)
        {
            UserModel? user = _userRepository.GetUser(login);
            if (user != null)
            {
                return false;
            }
            user = new UserModel()
            {

                Login = login,
                Password = password,
                Role = RolesEnum.Buyer
            };
            return _userRepository.AddUser(user) ? true : throw new Exception("Użytkownik nie został dodany!");
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

        public int RoleSelecion(UserModel loggedUser)
        {
            var Roles = loggedUser.Role;
            return _userView.RoleSelection(Roles);

        }
    }
}