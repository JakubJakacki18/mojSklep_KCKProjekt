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

namespace WpfApp.Views.UserWPFPages
{
	/// <summary>
	/// Logika interakcji dla klasy RoleSelectionPage.xaml
	/// </summary>
	public partial class RoleSelectionPage : Page
	{
		private TaskCompletionSource<int> _taskCompletionSource =new();
		public RoleSelectionPage()
		{
			InitializeComponent();
		}


		internal async Task<int> WaitForRoleSelectionAsync()
		{
			return await _taskCompletionSource.Task;
		}

		private void buyer_button_Click(object sender, RoutedEventArgs e)
		{
			_taskCompletionSource.SetResult(0);
		}

		private void seller_button_Click(object sender, RoutedEventArgs e)
		{
			_taskCompletionSource.SetResult(1);
		}
	}
}
