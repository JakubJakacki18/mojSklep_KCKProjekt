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

        public bool LandingPage()
        {
            return LandingPageAsync().Result;
        }

        public async Task<bool> LandingPageAsync()
        {
            var landingPage = new LandingPage();
            _mainFrame.Navigate(landingPage);
            return await landingPage.WaitForUserChoiceAsync();
        }

        public (string, string) showSignIn()
        {
            _mainFrame.Navigate(new SignInPage());
            return ("", "");

        }

        (string, string) showSignUp()
        {
            throw new NotImplementedException();
        }

        private void OnLoginCompleted(string username, string password)
        {

        }
        //      private void LandingPageReturn(object sender, NavigationEventArgs e)
        //      {
        //	if (e.Content is LandingPage landingPage)
        //          {
        //		_usersChoice = landingPage.usersChoice;
        //	}
        //}

        (string, string) IUserView.showSignUp()
        {
            throw new NotImplementedException();
        }
    }
}
