using Library;
using System.Windows;
using WpfApp.Views;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var launcher = new Launcher(new Io(str => output.Content = str, () =>
                {

                    ++i;
                    return i % 2 == 0 ? input1.Text : input2.Text;
                }
            ), new UserWPFView());
            launcher.Run();
        }

        private int i = 0;
    }
}