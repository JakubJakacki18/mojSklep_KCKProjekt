using Library.Data;
using Library.Interfaces;
using System.Windows.Controls;
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
            //_mainFrame.Navigate(new LandingPage());
            //_mainFrame.Navigated += LandingPageReturn;


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
            return await signInPage.WaitForSignIn();

        }

        public async Task<(string, string)> ShowSignUp()
        {
            //_mainFrame.Navigate(new SignUpPage());
            return ("", "");
        }

        private void OnLoginCompleted(string username, string password)
        {

        }

        public Task<(string, string)> ShowSignUp(bool isValid = true)
        {
            throw new NotImplementedException();
        }

        public int RoleSelection(RolesEnum roles)
        {
            throw new NotImplementedException();
        }
        //      private void LandingPageReturn(object sender, NavigationEventArgs e)
        //      {
        //	if (e.Content is LandingPage landingPage)
        //          {
        //		_usersChoice = landingPage.usersChoice;
        //	}
        //}

    }
}
