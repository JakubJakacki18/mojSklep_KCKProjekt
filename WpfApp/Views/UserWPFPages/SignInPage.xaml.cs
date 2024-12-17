using System.Windows;
using System.Windows.Controls;

namespace WpfApp.Views.UserWPFPages
{
    /// <summary>
    /// Logika interakcji dla klasy SignInPage.xaml
    /// </summary>
    public partial class SignInPage : Page
    {
        private TaskCompletionSource<(string, string)> _taskCompletionSource =
            new();
        public SignInPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var authenticationData = (login.Text, password.Password);
            _taskCompletionSource.SetResult(authenticationData);
        }
        public async Task<(string, string)> WaitForSignIn()
        {
            return await _taskCompletionSource.Task;
        }

    }
}
