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
using System.Xml;
using WpfApp.ViewModel;
using WpfApp.Views.BuyerWPFPages;

namespace WpfApp.Views.BuyerWPFPages
{
	/// <summary>
	/// Logika interakcji dla klasy HistoryDetailsWindow.xaml
	/// </summary>
	public partial class HistoryDetailsWindow : Window
	{
		ShoppingCartHistoryViewModel shoppingCartHistoryViewModel;
		public HistoryDetailsWindow(ShoppingCartHistoryViewModel shoppingCartHistoryViewModel)
		{
			InitializeComponent();
			this.shoppingCartHistoryViewModel = shoppingCartHistoryViewModel;
			DateLabel.Content = $"Zakupiono: {shoppingCartHistoryViewModel.Date}";
			TotalPriceLabel.Content = $"Za kwotę: {shoppingCartHistoryViewModel.TotalPrice}";
			QuantityLabel.Content = $"Ilość produktów: {shoppingCartHistoryViewModel.ProductCount}";
			PaymentMethodLabel.Content = $"Metoda płatności: {shoppingCartHistoryViewModel.PaymentMethod}";


			exit_button.Click += ExitButton_Click;

			ShoppingCartHistoryDataGrid.ItemsSource = shoppingCartHistoryViewModel.PurchasedProducts;
		}

		private void ExitButton_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
		}
	}
}


//public (CartActionEnum, CartProductModel?) Result { get; private set; }
