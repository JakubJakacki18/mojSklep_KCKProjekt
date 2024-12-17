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
using System.Windows.Shapes;

namespace WpfApp.Views.SellerWPFPages
{
    /// <summary>
    /// Logika interakcji dla klasy EditProductWindow.xaml
    /// </summary>
    public partial class EditProductWindow : Window
    {
        public (ShowProductsSellerActionEnum, ProductModel?) Result { get; private set; }
		private ProductModel productModel;
        public EditProductWindow(ProductModel productModel)
        {
            InitializeComponent();
            this.productModel = productModel;
			product_name.Text = $"{productModel.Name}";
			product_desctription.Text = $"{productModel.Description}";
			product_price.Text = $"{productModel.Price}";
			product_quantity.Text = $"{productModel.Quantity}";
			product_row.Text = $"{productModel.shelfRow}";
			product_column.Text = $"{productModel.shelfColumn}";

		}

		private void add_product_button_Click(object sender, RoutedEventArgs e)
		{
			productModel.Name = product_name.Text;
			productModel.Description = product_desctription.Text;
			if (decimal.TryParse(product_price.Text, out decimal price))
			{
				productModel.Price = price;
			}
			else
			{

				MessageBox.Show("Niepoprawny format ceny. Wprowadź wartość dziesiętną.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			if (int.TryParse(product_quantity.Text, out int quantity))
			{
				productModel.Quantity = quantity;
			}
			else
			{

				MessageBox.Show("Niepoprawny format ilości. Wprowadź liczbę całkowitą.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			if (int.TryParse(product_row.Text, out int shelfRow))
			{
				productModel.shelfRow = shelfRow;
			}
			else
			{

				MessageBox.Show("Niepoprawny format wiersza półki. Wprowadź liczbę całkowitą.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (int.TryParse(product_column.Text, out int shelfColumn))
			{
				productModel.shelfColumn = shelfColumn;
			}
			else
			{

				MessageBox.Show("Niepoprawny format kolumny półki. Wprowadź liczbę całkowitą.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			Result = (ShowProductsSellerActionEnum.update,productModel);
			this.DialogResult = true;
		}

		private void remove_product_button_Click(object sender, RoutedEventArgs e)
		{
			Result = (ShowProductsSellerActionEnum.delete,productModel);
			this.DialogResult = true;
		}

		private void reject_changes_button_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
		}
	}
}
