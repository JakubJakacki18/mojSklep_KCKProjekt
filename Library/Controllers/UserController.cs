using Library.Data;
using Library.Interfaces;
using Library.Model;
using Microsoft.Data.SqlClient;

namespace Library.Controllers
{
    public class UserController
    {
        private IUserView _userView;
        private IUserRepository _userRepository;
        public UserModel CurrentLoggedInUser { get; private set; }
        public static UserController Initialize(IUserView userView, IUserRepository userRepository)
        {
            _instance = new UserController(userView, userRepository);
            return _instance;
        }

        private static UserController _instance;
        public static UserController GetInstance()
        {
            if (_instance == null)
            {
                throw new Exception("UserController not initialized");
            }

            return _instance;
        }
        private UserController(IUserView userView, IUserRepository userRepository)
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
                    try
                    {
                        isSignUpSuccess = SignUpAsync(authenticationData.Item1, authenticationData.Item2);
                    }
                    catch (SqlException exception)
                    {
                        Console.WriteLine($"Błąd połączenia z bazą danych {exception.Message} {exception.ErrorCode}");
                        throw;
                    }
                } while (!isSignUpSuccess);
            }

            do
            {
                authenticationData = await _userView.ShowSignIn();
                try
                {
                    
                    result = await SignInAsync(authenticationData.Item1, authenticationData.Item2);
                }
                catch (SqlException exception)
                {
                    Console.WriteLine($"Błąd połączenia z bazą danych {exception.Message} {exception.ErrorCode}");
                    throw;
                }
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

            CurrentLoggedInUser = user;
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