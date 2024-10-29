using Library.Interfaces;
using System.Windows.Controls;

namespace WpfApp.Views
{
    /// <summary>
    /// Logika interakcji dla klasy UserWPFView.xaml
    /// </summary>
    public partial class UserWPFView : Page, IUserView
    {
        private readonly Frame _mainFrame;
        public UserWPFView(Frame mainFrame)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
        }

        public (string, string) showSignIn()
        {
            _mainFrame.Navigate(new SignInPage());
        }

        public void showSignUp()
        {
            throw new NotImplementedException();
        }
    }
}
