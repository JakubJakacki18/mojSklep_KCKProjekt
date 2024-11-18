using Library.Data;
using Library.Models;

namespace Library.Interfaces
{
    public interface IBuyerView
    {
        bool ExitApp();
        Object? ShowAllProducts(List<ProductModel> products,List<CartProductModel> productsFromCart);
        public void ShowInterface();
        public int ShowMenu();
        PaymentMethodEnum ShowPaymentMethod(List<CartProductModel> productsFromCart);
        (CartActionEnum actionEnum, CartProductModel? cartProduct) ShowUserCart(List<CartProductModel> cartProducts);
        void ShowShoppingHistory(List<ShoppingCartHistoryModel> shoppingCartHistories);
		void ShowMessage(string addProductStatus);
    }
}
