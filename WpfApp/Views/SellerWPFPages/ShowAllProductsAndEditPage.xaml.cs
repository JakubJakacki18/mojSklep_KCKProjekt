using Library.Data;
using Library.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApp.Views.SellerWPFPages
{
    /// <summary>
    /// Logika interakcji dla klasy ShowAllProductsAndEditPage.xaml
    /// </summary>
    public partial class ShowAllProductsAndEditPage : Page
    {
        private TaskCompletionSource<(ShowProductsSellerActionEnum, ProductModel?)> _taskCompletionSource = new();
        private List<ProductModel> products;
        public ShowAllProductsAndEditPage(List<ProductModel> products)
        {
            InitializeComponent();
            this.products = products;
            ProductsDataGrid.ItemsSource = this.products;
        }
        internal async Task<(ShowProductsSellerActionEnum, ProductModel?)> WaitForResultAsync()
        {
            return await _taskCompletionSource.Task;
        }

        private void ProductsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ProductsDataGrid.SelectedItem is ProductModel selectedProduct)
            {
                var editWindow = new EditProductWindow(selectedProduct);
                bool? result = editWindow.ShowDialog();

                if (result != true || editWindow == null)
                {
                    return;
                }
                _taskCompletionSource.SetResult(editWindow.Result);

            }
        }

        private void exit_button_Click(object sender, RoutedEventArgs e)
        {
            _taskCompletionSource.SetResult((ShowProductsSellerActionEnum.exit, null));
        }
    }
}
