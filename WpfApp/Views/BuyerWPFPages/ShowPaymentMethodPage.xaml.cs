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

namespace WpfApp.Views.BuyerWPFPages
{
	/// <summary>
	/// Logika interakcji dla klasy ShowPaymentMethodPage.xaml
	/// </summary>
	public partial class ShowPaymentMethodPage : Page
	{
        private TaskCompletionSource<PaymentMethodEnum> _taskCompletionSource = new();
		private List<CartProductModel> cartProducts;
        public ShowPaymentMethodPage(List<CartProductModel> cartProducts)
		{
			InitializeComponent();
			this.cartProducts = cartProducts;
			CartProductsDataGrid.ItemsSource = cartProducts;
		}

        internal async Task<PaymentMethodEnum> WaitForResultAsync()
        {
			return await _taskCompletionSource.Task;
		}

		private void exit_button_Click(object sender, RoutedEventArgs e)
		{
			_taskCompletionSource.SetResult(PaymentMethodEnum.None);
		}

		private void buy_now_button_Click(object sender, RoutedEventArgs e)
		{
			var selectedRadioButton = LogicalTreeHelper
	   .GetChildren(this)
	   .OfType<RadioButton>()
	   .FirstOrDefault(rb => rb.GroupName == "Options" && rb.IsChecked == true);

			if (selectedRadioButton == null) 
			{
				return;
			}
			string tagValue = selectedRadioButton?.Tag.ToString()?.ToLower() ?? "";
			_taskCompletionSource.SetResult(tagValue switch
			{
				"cash" => PaymentMethodEnum.cash,
				"card" => PaymentMethodEnum.card,
				"blik" => PaymentMethodEnum.blik,
				_ => PaymentMethodEnum.None
			});
		}


	}
}

