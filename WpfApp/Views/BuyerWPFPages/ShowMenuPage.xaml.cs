using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
	}
}
