using Library.Data;
using Library.Models;
using System.Collections.ObjectModel;
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
        private ObservableCollection<ProductModel> filteredProducts;
        private TaskCompletionSource<(ShowProductsSellerActionEnum, ProductModel?)> _taskCompletionSource = new();
        private List<ProductModel> products;
        public ShowAllProductsAndEditPage(List<ProductModel> products)
        {
            InitializeComponent();
            this.products = products;
            filteredProducts = new ObservableCollection<ProductModel>(products);
            ProductsDataGrid.ItemsSource = filteredProducts;
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

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Text = "";
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchTextBox.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(searchText))
            {
                filteredProducts.Clear();
                foreach (var product in products)
                {
                    filteredProducts.Add(product);
                }
                return;
            }
            var filtered = products.Where(p =>
                (int.TryParse(searchText, out int searchId) && p.Id == searchId) ||
                p.Name.ToLower().Contains(searchText) ||
                (p.Description?.Contains(searchText, StringComparison.CurrentCultureIgnoreCase) ?? false)).ToList();

            // Aktualizacja listy
            filteredProducts.Clear();
            foreach (var product in filtered)
            {
                filteredProducts.Add(product);
            }
        }
    }
}

