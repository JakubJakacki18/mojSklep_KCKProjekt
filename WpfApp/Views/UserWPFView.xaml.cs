using Library.Data;
using Library.Interfaces;
using System.Windows.Controls;
using System.Windows.Forms;
using WpfApp.Views.UserWPFPages;

namespace WpfApp.Views
{
    /// <summary>
    /// Logika interakcji dla klasy UserWPFView.xaml
    /// </summary>
    public partial class UserWPFView : Page, IUserView
    {
        private TaskCompletionSource<bool> isSignInDesireTask;
        private readonly Frame _mainFrame;
        public UserWPFView(Frame mainFrame)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
        }

        public async Task<bool> LandingPage()
        {
            var landingPage = new LandingPage();
            _mainFrame.Navigate(landingPage);
            return await landingPage.WaitForUserChoiceAsync();
        }

        public async Task<(string, string)> ShowSignIn(bool isValid)
        {
            var signInPage = new SignInPage();
            _mainFrame.Navigate(signInPage);
            if (!isValid)
                MessageBox.Show("Logowanie nieudane, spróbuj ponownie");
            return await signInPage.WaitForSignIn();

        }

        public async Task<(string, string)> ShowSignUp(bool isValid = true)
        {
            var signUpPage = new SignUpPage();
            _mainFrame.Navigate(signUpPage);
            if (!isValid)
                MessageBox.Show("Rejestracja nieudana, spróbuj ponownie");
            return await signUpPage.WaitForSignUp();
        }

        async Task<int> IUserView.RoleSelection(RolesEnum roles)
        {
            if (RolesEnum.PermissionBuyer == roles)
            {
                return 0;
            }
            var roleSelectionPage = new RoleSelectionPage();
            _mainFrame.Navigate(roleSelectionPage);
            return await roleSelectionPage.WaitForRoleSelectionAsync();
        }


    }
}
