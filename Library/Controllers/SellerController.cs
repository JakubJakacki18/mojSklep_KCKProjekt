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
                    case 4:
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
            var result = _productRepository.AddProduct(product);
            if (result)
            {
                _sellerView.ShowMessage("Product added successfully");
            }
            else
            {
                _sellerView.ShowMessage("Product could not be added");
            }

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
