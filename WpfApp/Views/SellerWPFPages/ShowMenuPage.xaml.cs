using Library.Controllers;
using Library.Data;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp.Views.SellerWPFPages
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

		private void logout_menu_item_click(object sender, RoutedEventArgs e)
		{
			var result = (MessageBox.Show("Czy chcesz się wylogować?", ConstString.AppName, MessageBoxButton.YesNo) == MessageBoxResult.Yes);
			if (result)
			{
				UserController.GetInstance().Logout();
				_taskCompletionSource.SetResult(0);
			}
		}

		private void AccountButton_Click(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;
			if (button != null && button.ContextMenu != null)
			{
				button.ContextMenu.PlacementTarget = button;
				button.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
				button.ContextMenu.IsOpen = true;
			}
		}
	}
}
