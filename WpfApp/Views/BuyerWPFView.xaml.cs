using Library.Data;
using Library.Interfaces;
using Library.Models;
using System.Windows;
using System.Windows.Controls;
using WpfApp.Views.BuyerWPFPages;

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
            var result = (MessageBox.Show("Czy chcesz zamknąć aplikacje?", ConstString.AppName, MessageBoxButton.YesNo) == MessageBoxResult.Yes);
            if (result)
                Application.Current.Shutdown();
            return Task.FromResult(result);
        }

        public async Task<object?> ShowAllProducts(List<ProductModel> products, List<CartProductModel> productsFromCart)
        {
            var showAllProductsPage = new ShowAllProductsPage(products, productsFromCart);
            _mainFrame.Navigate(showAllProductsPage);
            return await showAllProductsPage.WaitForObjectAsync();

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

        public async Task<PaymentMethodEnum> ShowPaymentMethod(List<CartProductModel> productsFromCart)
        {
            var showPaymentMethodPage = new ShowPaymentMethodPage(productsFromCart);
            _mainFrame.Navigate(showPaymentMethodPage);
            return await showPaymentMethodPage.WaitForResultAsync();
        }

        public async Task ShowShoppingHistory(List<ShoppingCartHistoryModel> shoppingCartHistories)
        {
            var showShoppingHistoryPage = new ShowShoppingHistoryPage(shoppingCartHistories);
            MessageBox.Show("test2");
            _mainFrame.Navigate(showShoppingHistoryPage);
            MessageBox.Show("test");
            await showShoppingHistoryPage.WaitForResultAsync();
        }

        public async Task<(CartActionEnum actionEnum, CartProductModel? cartProduct)> ShowUserCart(List<CartProductModel> cartProducts)
        {
            var showUserCartPage = new ShowUserCartPage(cartProducts);
            _mainFrame.Navigate(showUserCartPage);
            return await showUserCartPage.WaitForResultAsync();
        }
    }
}
