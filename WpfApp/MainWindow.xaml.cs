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
            //var launcher = new Launcher(new Io(str => output.Content = str, () =>
            //{

            //	++i;
            //	return i % 2 == 0 ? input1.Text : input2.Text;
            //}
            //), new UserWPFView(MainFrame));
            //launcher.Run();
            //new UserWPFView(MainFrame);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private int i = 0;

        private void MainFrame_Loaded(object sender, RoutedEventArgs e)
        {
            var launcher = new Launcher(new UserWPFView(MainFrame));
            launcher.Run();
        }
    }
}