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
	/// Logika interakcji dla klasy ShowShoppingHistoryPage.xaml
	/// </summary>
	public partial class ShowShoppingHistoryPage : Page
	{
        private TaskCompletionSource _taskCompletionSource = new();
		private List<ShoppingCartHistoryModel> shoppingCartHistories;
        public ShowShoppingHistoryPage(List<ShoppingCartHistoryModel> shoppingCartHistories)
		{
			InitializeComponent();
			this.shoppingCartHistories = shoppingCartHistories;
			_taskCompletionSource.SetResult();
		}

        internal async Task WaitForResultAsync()
        {
			await _taskCompletionSource.Task;
            return;
        }
    }
}
