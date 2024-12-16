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
using WpfApp.Views.SellerWPFPages;

namespace WpfApp.Views
{
	/// <summary>
	/// Logika interakcji dla klasy SellerWPFView.xaml
	/// </summary>
	public partial class SellerWPFView : Page, ISellerView
	{
		private readonly Frame _mainFrame;
		public SellerWPFView(Frame mainFrame)
		{
			InitializeComponent();
			_mainFrame = mainFrame;
		}

		public Task<ProductModel?> AddProduct()
		{
			throw new NotImplementedException();
		}

		public Task EditProduct()
		{
			throw new NotImplementedException();
		}

		public Task<bool> ExitApp()
		{
			var result = (MessageBox.Show("Czy chcesz zamknąć aplikacje?", ConstString.AppName, MessageBoxButton.YesNo) == MessageBoxResult.Yes);
			if (result)
				Application.Current.Shutdown();
			return Task.FromResult(result);
		}

		public Task<(ShowProductsSellerActionEnum, ProductModel?)> ShowAllProductsAndEdit(List<ProductModel> product)
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
			throw new NotImplementedException();
		}
	}
}
