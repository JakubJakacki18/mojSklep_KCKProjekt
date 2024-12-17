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
using WpfApp.Views.BuyerWPFPages;

namespace WpfApp.Views.SellerWPFPages
{
    /// <summary>
    /// Logika interakcji dla klasy ShowAllProductsAndEditPage.xaml
    /// </summary>
    public partial class ShowAllProductsAndEditPage : Page
    {
		private TaskCompletionSource<(ShowProductsSellerActionEnum, ProductModel?)> _taskCompletionSource = new();
        private List<ProductModel> products;
		public ShowAllProductsAndEditPage(List<ProductModel> products)
        {
            InitializeComponent();
            this.products = products;
			ProductsDataGrid.ItemsSource = this.products;
		}
		internal async Task<(ShowProductsSellerActionEnum, ProductModel?)> WaitForResultAsync()
		{
			return await _taskCompletionSource.Task;
		}

		private void ProductsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (ProductsDataGrid.SelectedItem is ProductModel selectedProduct)
			{
				var editWindow = new EditProductWindow(selectedProduct);
				bool? result = editWindow.ShowDialog();

				if (result != true || editWindow == null)
				{
					return;
				}
				
			}
		}

		private void exit_button_Click(object sender, RoutedEventArgs e)
		{
			_taskCompletionSource.SetResult((ShowProductsSellerActionEnum.exit,null));
		}
	}
}
