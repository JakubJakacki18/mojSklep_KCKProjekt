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
        void ShowPaymentMethod();
        (CartActionEnum actionEnum, CartProductModel? cartProduct) ShowUserCart(List<CartProductModel> cartProducts);
        void ShowMessage(string addProductStatus);
    }
}
