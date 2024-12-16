using Library.Data;
using Library.Interfaces;
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
using WpfApp.Views.UserWPFPages;

namespace WpfApp.Views
{
	/// <summary>
	/// Logika interakcji dla klasy BuyerWPFView.xaml
	/// </summary>
	public partial class BuyerWPFView : Page, IBuyerView
	{
		private readonly Frame _mainFrame;
		public BuyerWPFView(Frame mainFrame)
		{
			InitializeComponent();
			_mainFrame = mainFrame;
		}

		public Task<bool> ExitApp()
		{
			var result = (MessageBox.Show("Czy chcesz zamknąć aplikacje?", ConstString.AppName, MessageBoxButton.YesNo)==MessageBoxResult.Yes);
			if(result)
				Application.Current.Shutdown();
			return Task.FromResult(result);
		}

		public async Task<object?> ShowAllProducts(List<ProductModel> products, List<CartProductModel> productsFromCart)
		{
			var showAllProductsPage = new ShowAllProductsPage(products, productsFromCart);
			_mainFrame.Navigate(showAllProductsPage);
			return await showAllProductsPage.WaitForObject();

		}

		public Task ShowInterface()
		{
			throw new NotImplementedException();
		}

		public async Task<int> ShowMenu()
		{
			var showMenuPage = new ShowMenuPage();
			_mainFrame.Navigate(showMenuPage);
			return await showMenuPage.WaitForPageSelectionAsync();
		}

		public Task ShowMessage(string addProductStatus)
		{
			MessageBox.Show(addProductStatus);
			return Task.CompletedTask;
		}

		public Task<PaymentMethodEnum> ShowPaymentMethod(List<CartProductModel> productsFromCart)
		{
			throw new NotImplementedException();
		}

		public Task ShowShoppingHistory(List<ShoppingCartHistoryModel> shoppingCartHistories)
		{
			throw new NotImplementedException();
		}

		public Task<(CartActionEnum actionEnum, CartProductModel? cartProduct)> ShowUserCart(List<CartProductModel> cartProducts)
		{
			throw new NotImplementedException();
		}
	}
}
