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
using static System.Net.Mime.MediaTypeNames;

namespace WpfApp.Views.BuyerWPFPages
{
    /// <summary>
    /// Logika interakcji dla klasy CartProductDetailsWindow.xaml
    /// </summary>
    public partial class CartProductDetailsWindow : Window
    {
		CartProductModel cartProduct;
		public (CartActionEnum, CartProductModel?) Result { get; private set; }
		public CartProductDetailsWindow(CartProductModel cartProduct)
        {
            InitializeComponent();
            this.cartProduct = cartProduct;
			IdLabel.Content = $"Kod kreskowy: {cartProduct.OriginalProduct.Id}";
			NameLabel.Content = $"Nazwa: {cartProduct.OriginalProduct.Name}";
			DescriptionLabel.Content = $"Opis: {cartProduct.OriginalProduct.Description}";
			PriceLabel.Content = $"Cena: {cartProduct.OriginalProduct.Price}";
			QuantityLabel.Content = $"Ilość dostępnych sztuk: {cartProduct.OriginalProduct.Quantity}";
            QuantityInCartLabel.Content = $"Ilość w koszyku obecnie: {cartProduct.Quantity}";

			QuantityTextBox.TextChanged += QuantityTextBox_TextChanged;
            change_quantity_button.Click += ChangeQuantityButton_Click;
			remove_cartproduct_button.Click += RemoveButton_Click;
            reject_changes_button.Click += RejectButton_Click;
		}

		private void RejectButton_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
		}

		private void RemoveButton_Click(object sender, RoutedEventArgs e)
		{
			Result = (CartActionEnum.Remove, cartProduct);
			DialogResult = true;
		}

		private void ChangeQuantityButton_Click(object sender, RoutedEventArgs e)
		{
			cartProduct.Quantity = int.Parse(QuantityTextBox.Text);
			Result = (CartActionEnum.Update, cartProduct);
			DialogResult = true;
		}

		private void QuantityTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (int.TryParse(QuantityTextBox.Text, out int quantity) && quantity > 0 && quantity <= cartProduct.OriginalProduct.Quantity)
			{
				PriceToPayLabel.Content = $"Cena do zapłaty: {quantity * cartProduct.OriginalProduct.Price} {ConstString.Currency}";
			}
			else if (QuantityTextBox.Text.Length>0) 
			{
				string text = QuantityTextBox.Text;
				int caretIndex = QuantityTextBox.CaretIndex;
				QuantityTextBox.Text = text.Substring(0, text.Length - 1);
				QuantityTextBox.CaretIndex = Math.Max(0, caretIndex - 1);
			}

		}
	}
}


