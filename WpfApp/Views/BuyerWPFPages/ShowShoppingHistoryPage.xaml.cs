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
using Library.Data;
using Library.Models;
using WpfApp.ViewModel;

namespace WpfApp.Views.BuyerWPFPages
{
	/// <summary>
	/// Logika interakcji dla klasy ShowShoppingHistoryPage.xaml
	/// </summary>
	public partial class ShowShoppingHistoryPage : Page
	{
        private TaskCompletionSource<object?> _taskCompletionSource = new();
		private List<ShoppingCartHistoryModel> shoppingCartHistories;
		private List<ShoppingCartHistoryViewModel> shoppingCartHistoryViewModels;
        public ShowShoppingHistoryPage(List<ShoppingCartHistoryModel> shoppingCartHistories)
		{
			InitializeComponent();
			this.shoppingCartHistories = shoppingCartHistories;
			this.shoppingCartHistoryViewModels = shoppingCartHistories
	   .Select(model => new ShoppingCartHistoryViewModel(model))
	   .ToList();
			HistoryDataGrid.ItemsSource = shoppingCartHistoryViewModels;

		}

        internal async Task WaitForResultAsync()
        {
			await _taskCompletionSource.Task;
            return;
        }

		private void HistoryDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (HistoryDataGrid.SelectedItem is ShoppingCartHistoryViewModel selectedHistoryEntry)
			{
				var detailsWindow = new HistoryDetailsWindow(selectedHistoryEntry);
				bool? result = detailsWindow.ShowDialog();

			}
		}

		private void exit_button_Click(object sender, RoutedEventArgs e)
		{
			_taskCompletionSource.SetResult(null);
		}
	}
}
