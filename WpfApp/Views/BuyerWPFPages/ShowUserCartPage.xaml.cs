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
		}

		internal async Task<(CartActionEnum actionEnum, CartProductModel? cartProduct)> WaitForResultAsync()
		{
			return await _taskCompletionSource.Task;
		}
	}
}
