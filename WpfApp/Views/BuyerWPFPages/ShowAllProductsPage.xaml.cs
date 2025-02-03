using Library.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

		private ObservableCollection<ProductModel> filteredProducts;
		private TaskCompletionSource<object?> _taskCompletionSource = new();
		List<ProductModel> products;
		List<CartProductModel> productsFromCart;
		public ShowAllProductsPage(List<ProductModel> products, List<CartProductModel> productsFromCart)
		{
			InitializeComponent();
			this.products = products;
			this.productsFromCart = productsFromCart;

			filteredProducts = new ObservableCollection<ProductModel>(products);
			ProductsDataGrid.ItemsSource = filteredProducts;
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

		private void SearchButton_Click(object sender, RoutedEventArgs e)
		{
			SearchTextBox.Text = "";
		}

		private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			string searchText = SearchTextBox.Text.Trim().ToLower();
			if (string.IsNullOrEmpty(searchText))
			{
				filteredProducts.Clear();
				foreach (var product in products)
				{
					filteredProducts.Add(product);
				}
				return;
			}
			var filtered = products.Where(p =>
			(int.TryParse(searchText, out int searchId) && p.Id == searchId) ||
			p.Name.ToLower().Contains(searchText) ||
			(p.Description?.Contains(searchText, StringComparison.CurrentCultureIgnoreCase) ?? false)).ToList();

			// Aktualizacja listy
			filteredProducts.Clear();
			foreach (var product in filtered)
			{
				filteredProducts.Add(product);
			}
		}
	}
}
