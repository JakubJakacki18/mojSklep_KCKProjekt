using Library.Interfaces;

namespace Library.Controllers
{
    public class SellerController
    {
        private ISellerView _sellerView;

        public SellerController(ISellerView sellerView)
        {
            _sellerView = sellerView;
        }

        public void ShowInterface()
        {
            //_sellerView.ShowInterface();
        }

        public void AddProduct()
        {

        }

        public void ChangeProduct()
        {
        }

        public void RemoveProduct()
        {

        }
        public void ShowDetailsOfProduct() { }
        public void ShowAllProducts() { }


    }
}
