namespace KCKProjekt.Controllers
{
    public class ShopController
    {
        private IShopView _shopView;
        public ShopController(IShopView shopView)
        {
            _shopView = shopView;
        }
        public void ShowProducts()
        {
            _shopView.ShowProducts();
        }
        public void ShowProductDetails()
        {
            _shopView.ShowProductDetails();
        }
        public void AddProductToCart()
        {
            _shopView.AddProductToCart();
        }
        public void ShowCart()
        {
            _shopView.ShowCart();
        }
        public void RemoveProductFromCart()
        {
            _shopView.RemoveProductFromCart();
        }
        public void BuyProducts()
        {
            _shopView.BuyProducts();
        }
    }

    public interface IShopView
    {
        public void ShowProducts();
        public void ShowProductDetails();
        public void AddProductToCart();
        public void ShowCart();
        public void RemoveProductFromCart();
        public void BuyProducts();
    }
}
