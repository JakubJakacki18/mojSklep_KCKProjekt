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

		public Task<bool> WaitForUserChoiceAsync()
		{

			return _taskCompletionSource.Task;
		}
	}
}
