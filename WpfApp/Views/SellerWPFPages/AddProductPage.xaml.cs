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

namespace WpfApp.Views.SellerWPFPages
{
    /// <summary>
    /// Logika interakcji dla klasy AddProductPage.xaml
    /// </summary>
    public partial class AddProductPage : Page
    {
		private TaskCompletionSource<ProductModel?> _taskCompletionSource = new();
		public AddProductPage()
        {
            InitializeComponent();
        }
		internal async Task<ProductModel?> WaitForResultAsync()
		{
			return await _taskCompletionSource.Task;
		}

		private void exit_button_Click(object sender, RoutedEventArgs e)
		{
			_taskCompletionSource.SetResult(null);
		}

		private void add_product_button_Click(object sender, RoutedEventArgs e)
		{
			var result = new ProductModel
			{
				Name = product_name.Text,
				Description = product_desctription.Text,
				Price = decimal.TryParse(product_price.Text, out var price) ? price : throw new Exception("Invalid price"),
				Quantity = int.TryParse(product_quantity.Text, out var quantity) ? quantity : throw new Exception("Invalid quantity"),
				shelfRow = int.TryParse(product_row.Text, out var row) ? row : throw new Exception("Invalid row"),
				shelfColumn = int.TryParse(product_column.Text, out var column) ? column : throw new Exception("Invalid column")
			};
			_taskCompletionSource.SetResult(result);
		}
	}
}
