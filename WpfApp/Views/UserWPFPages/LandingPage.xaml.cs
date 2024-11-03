using System.Windows;
using System.Windows.Controls;

namespace WpfApp.Views.UserWPFPages
{
    /// <summary>
    /// Logika interakcji dla klasy LandingPage.xaml
    /// </summary>
    public partial class LandingPage : Page
    {
        private TaskCompletionSource<bool> _taskCompletionSource;
        public LandingPage()
        {
            InitializeComponent();

            _taskCompletionSource = new TaskCompletionSource<bool>();
        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            _taskCompletionSource.SetResult(true);
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            _taskCompletionSource.SetResult(false);

        }

        public async Task<bool> WaitForUserChoiceAsync()
        {
            return await _taskCompletionSource.Task;
        }

    }
}
