using System.Windows;
using System.Windows.Controls;

namespace WpfApp.Views.UserWPFPages
{
    /// <summary>
    /// Logika interakcji dla klasy SignInPage.xaml
    /// </summary>
    public partial class SignInPage : Page
    {
		//private Action<(string,string)> _loginCallback;
		public SignInPage()
        {
            InitializeComponent();
			//_loginCallback = loginCallback;
		}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var authenticationData = (input1.Text, input2.Text);
			//_loginCallback?.Invoke(authenticationData);
            NavigationService.GoBack();
        }
    }
}
