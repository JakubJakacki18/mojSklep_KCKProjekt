using Library.Data;
using Library.Interfaces;
using Library.Models;

namespace Library.Controllers
{
    public class SellerController(ISellerView sellerView, IProductRepository productRepository)
    {
        private ISellerView _sellerView = sellerView;
        private IProductRepository _productRepository = productRepository;



        public void ShowMenu()
        {
            bool isExitWanted = false;
            do
            {
                switch (_sellerView.ShowMenu())
                {
                    case 1:
                        AddProduct(_sellerView.AddProduct());
                        break;
                    /*case 2:
                        _sellerView.ShowUserCart();
                        break;
                    case 3:
                        _sellerView.ShowPaymentMethod();
                        break;*/
                    case 3:
                        isExitWanted = _sellerView.ExitApp();
                        break;
                }
            } while (!isExitWanted);
        }

        public void ShowInterface()
        {
            //_sellerView.ShowInterface();
        }

        public void AddProduct(ProductModel? product)
        {
            if (product != null)
            {
				var result = _productRepository.AddProduct(product);
				if (result)
				{
					_sellerView.ShowMessage(ConstString.AddProductSuccess);
                    return;
				}
			}
                _sellerView.ShowMessage(ConstString.AddProductFail);
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
