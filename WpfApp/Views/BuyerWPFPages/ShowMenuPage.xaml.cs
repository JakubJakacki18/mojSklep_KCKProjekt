using Library.Controllers;
using Library.Data;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp.Views.BuyerWPFPages
{
    /// <summary>
    /// Logika interakcji dla klasy ShowMenuPage.xaml
    /// </summary>
    public partial class ShowMenuPage : Page
    {
        private TaskCompletionSource<int> _taskCompletionSource = new();
        public ShowMenuPage()
        {
            InitializeComponent();
        }

        internal async Task<int> WaitForPageSelectionAsync()
        {
            return await _taskCompletionSource.Task;
        }

        private void menu_button_click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string tagValue)
            {
                int action = int.Parse(tagValue);
                _taskCompletionSource.SetResult(action);
            }
        }

        private void logout_button_Click(object sender, RoutedEventArgs e)
        {
            var result = (MessageBox.Show("Czy chcesz się wylogować?", ConstString.AppName, MessageBoxButton.YesNo) == MessageBoxResult.Yes);
            if (result)
            {
                UserController.GetInstance().Logout();
                _taskCompletionSource.SetResult(0);
            }
        }
    }
}
