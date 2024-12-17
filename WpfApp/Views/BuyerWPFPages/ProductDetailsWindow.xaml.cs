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

namespace WpfApp.Views.BuyerWPFPages
{
	/// <summary>
	/// Logika interakcji dla klasy ProductDetailsWindow.xaml
	/// </summary>
	public partial class ProductDetailsWindow : Window
	{
		ProductModel product;
		public int QuantityToAdd { get; private set; }
		public ProductDetailsWindow(ProductModel selectedProduct)
		{
			InitializeComponent();
			product = selectedProduct;

			IdLabel.Content = $"Kod kreskowy: {product.Id}";
			NameLabel.Content = $"Nazwa: {product.Name}";
			DescriptionLabel.Content = $"Opis: {product.Description}";
			PriceLabel.Content = $"Cena: {product.Price}";
			QuantityLabel.Content = $"Ilość dostępnych sztuk: {product.Quantity}";

			AddToCartButton.Click += AddToCartButton_Click;
			RejectButton.Click += RejectButton_Click;

			QuantityTextBox.TextChanged += QuantityTextBox_TextChanged;
		}

		private void QuantityTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (int.TryParse(QuantityTextBox.Text, out int quantity) && quantity > 0 && quantity <= product.Quantity)
			{
				PriceToPayLabel.Content = $"Cena do zapłaty: {quantity * product.Price} {ConstString.Currency}";
			}
			else if (QuantityTextBox.Text.Length > 0)
			{
				string text = QuantityTextBox.Text;
				int caretIndex = QuantityTextBox.CaretIndex;
				QuantityTextBox.Text = text.Substring(0, text.Length - 1);
				QuantityTextBox.CaretIndex = Math.Max(0, caretIndex - 1);
			}

		}

		// Kliknięcie przycisku "Dodaj do koszyka"
		private void AddToCartButton_Click(object sender, RoutedEventArgs e)
		{
			if (int.TryParse(QuantityTextBox.Text, out int quantity) &&
				(quantity > 0 && quantity <= product.Quantity))
			{
				QuantityToAdd = quantity;
				this.DialogResult = true; 
			}
			else
			{
				MessageBox.Show("Niepoprawna ilość produktów, popraw wartość", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		// Kliknięcie przycisku "Zrezygnuj"
		private void RejectButton_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
		}
	}
}

