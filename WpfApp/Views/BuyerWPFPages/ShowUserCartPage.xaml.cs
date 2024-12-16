using Library.Data;
using Library.Models;
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
	/// Logika interakcji dla klasy ShowUserCartPage.xaml
	/// </summary>
	public partial class ShowUserCartPage : Page
	{
		private TaskCompletionSource<(CartActionEnum actionEnum, CartProductModel? cartProduct)> _taskCompletionSource = new();
		List<CartProductModel> cartProducts;
		public ShowUserCartPage(List<CartProductModel> cartProducts)
		{
			InitializeComponent();
			this.cartProducts = cartProducts;
			CartProductsSummaryDataGrid.ItemsSource = this.cartProducts;
			PriceToPayLabel.Content = $"Łącznie do zapłaty: {this.cartProducts.Sum(p=> p.Quantity*p.OriginalProduct.Price)}";
        }

        internal async Task<(CartActionEnum actionEnum, CartProductModel? cartProduct)> WaitForResultAsync()
		{
			return await _taskCompletionSource.Task;
		}

		private void CartProductsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (CartProductsSummaryDataGrid.SelectedItem is CartProductModel selectedProduct)
			{
				var detailsWindow = new CartProductDetailsWindow(selectedProduct);
				bool? result = detailsWindow.ShowDialog();

				if (result != true || detailsWindow == null)
				{
					return;
				}
				_taskCompletionSource.SetResult(detailsWindow.Result);

			}
		}

		private void exit_button_Click(object sender, RoutedEventArgs e)
		{
			_taskCompletionSource.SetResult((CartActionEnum.Exit,null));
		}

		private void pay_button_Click(object sender, RoutedEventArgs e)
		{
			_taskCompletionSource.SetResult((CartActionEnum.Buy, null));
		}

		private void remove_all_cart_button_Click(object sender, RoutedEventArgs e)
		{
			_taskCompletionSource.SetResult((CartActionEnum.RemoveAll, null));
		}
	}
}
