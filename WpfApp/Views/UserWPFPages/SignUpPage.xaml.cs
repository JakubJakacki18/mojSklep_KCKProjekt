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
    /// Logika interakcji dla klasy SignUpPage.xaml
    /// </summary>
    public partial class SignUpPage : Page
    {
		private TaskCompletionSource<(string, string)> _taskCompletionSource = new();
		public SignUpPage()
        {
            InitializeComponent();
        }



		internal async Task<(string, string)> WaitForSignUp()
		{
			return await _taskCompletionSource.Task;
		}

		private void sign_up_button_Click(object sender, RoutedEventArgs e)
		{
			if (login.Text.Length > 0 && password.Password.Length > 0)
			{
				var authenticationData = (login.Text, password.Password);
				_taskCompletionSource.SetResult(authenticationData);
			}
			else
			{
				MessageBox.Show("Login lub hasło jest puste");
			}
		}
	}
}
