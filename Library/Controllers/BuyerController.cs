using Library.Data;
using Library.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Library.Controllers
{
    public class BuyerController(IBuyerView buyerView, IUserRepository userRepository, IProductRepository productRepository)
    {
        private IBuyerView _buyerView = buyerView;
        private IUserRepository _userRepository = userRepository;
        private IProductRepository _productRepository = productRepository;

        public void ShowMenu()
        {
            bool isExitWanted = false;
            do
            {
                switch (_buyerView.ShowMenu())
                {
                    case 1:
                        object? result;
                        do
                        {
                            result = _buyerView.ShowAllProducts(_productRepository.GetProducts().ToList());
                            if (result is CartProductModel cartProduct)
                            {
                                AddProductToCart(cartProduct);
                                _buyerView.ShowMessage(ConstString.AddToCartSuccess);
                            }
						} while (result is CartProductModel);
						break;
                    case 2:
                        

                        _buyerView.ShowUserCart();
                        break;
                    case 3:
                        _buyerView.ShowPaymentMethod();
                        break;
                    case 4:
                        isExitWanted = _buyerView.ExitApp();
                        break;
                }
            } while (!isExitWanted);
        }
        private void AddProductToCart(CartProductModel cartProduct)
		{
			_userRepository.AddProductToCart(cartProduct,currentLoggedInUser);

		}
	}
}
