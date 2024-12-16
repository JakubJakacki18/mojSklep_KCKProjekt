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
	/// Logika interakcji dla klasy ShowAllProductsPage.xaml
	/// </summary>
	public partial class ShowAllProductsPage : Page
	{
		private TaskCompletionSource<object?> _taskCompletionSource = new();
		List<ProductModel> products;
		List<CartProductModel> productsFromCart;
		public ShowAllProductsPage(List<ProductModel> products, List<CartProductModel> productsFromCart)
		{
			InitializeComponent();
			this.products = products;
			this.productsFromCart = productsFromCart;
			ProductsDataGrid.ItemsSource = this.products;
			CartProductsDataGrid.ItemsSource = this.productsFromCart;
		}

		private void ProductsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (ProductsDataGrid.SelectedItem is ProductModel selectedProduct)
			{
				var detailsWindow = new ProductDetailsWindow(selectedProduct);
				bool? result = detailsWindow.ShowDialog();

				if (result != true || detailsWindow == null)
				{
					return;
				}
				var cartProduct = new CartProductModel()
				{
					OriginalProduct = selectedProduct,
					Quantity = detailsWindow.QuantityToAdd
				};
				_taskCompletionSource.SetResult(cartProduct);

			}
		}

		public async Task<object?> WaitForObjectAsync()
		{
			return await _taskCompletionSource.Task;
		}

		private void exit_button_Click(object sender, RoutedEventArgs e)
		{
			_taskCompletionSource.SetResult(null);
		}
	}
}
