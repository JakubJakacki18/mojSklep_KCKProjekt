using Library.Data;
using Library.Models;

namespace Library.Interfaces
{
    public interface IBuyerView
    {
        Task<bool> ExitApp();
        Task<Object?> ShowAllProducts(List<ProductModel> products, List<CartProductModel> productsFromCart);
        public Task ShowInterface();
        public Task<int> ShowMenu();
        Task<PaymentMethodEnum> ShowPaymentMethod(List<CartProductModel> productsFromCart);
        Task<(CartActionEnum actionEnum, CartProductModel? cartProduct)> ShowUserCart(List<CartProductModel> cartProducts);
        Task ShowShoppingHistory(List<ShoppingCartHistoryModel> shoppingCartHistories);
        Task ShowMessage(string addProductStatus);
    }
}
