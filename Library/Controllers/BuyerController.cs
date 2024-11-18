using Library.Data;
using Library.Interfaces;
using Library.Models;
using System;
namespace Library.Controllers
{
    public class BuyerController
    {
        private readonly IBuyerView _buyerView;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly UserModel _currentLoggedInUser;
		private static BuyerController? _instance;

		public static BuyerController Initialize(IBuyerView buyerView, IUserRepository userRepository, IProductRepository productRepository)
        {
            return _instance = new BuyerController(buyerView, userRepository, productRepository);
        }
        public static BuyerController getInstance()
        {
            if (_instance == null)
            {
                throw new Exception("BuyerController not initialized");
            }

            return _instance;
        }
        private BuyerController(IBuyerView buyerView, IUserRepository userRepository, IProductRepository productRepository)
        {
            _currentLoggedInUser = UserController.GetInstance().CurrentLoggedInUser;
            _buyerView = buyerView;
            _userRepository = userRepository;
            _productRepository = productRepository;
        }

        

        public void ShowMenu()
        {
            bool isExitWanted = false;
            do
            {
                switch (_buyerView.ShowMenu())
                {
                    case 1:
                        object? resultShowAllProducts;
                        do
                        {
                            resultShowAllProducts = _buyerView.ShowAllProducts(_productRepository.GetProducts().ToList(),_userRepository.GetCart(_currentLoggedInUser).ToList());
                            if (resultShowAllProducts is CartProductModel cartProduct)
                            {
                                AddProductToCart(cartProduct);
                            }
                        } while (resultShowAllProducts is CartProductModel);
                        break;
                    case 2:
                        (CartActionEnum, CartProductModel?) resultShowUserCart;
						do
						{
							resultShowUserCart = _buyerView.ShowUserCart(_userRepository.GetCart(_currentLoggedInUser).ToList());
                            UserCartAction(resultShowUserCart);
						}
						while (resultShowUserCart.Item1 != CartActionEnum.Exit);
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
            bool result = _userRepository.IsProductInCart(cartProduct, _currentLoggedInUser)
                ? _userRepository.UpdateProductInCart(cartProduct, _currentLoggedInUser)
                : _userRepository.AddProductToCart(cartProduct, _currentLoggedInUser);
            _buyerView.ShowMessage((result) ? ConstString.AddToCartSuccess : ConstString.AddToCartFail);
        }

        private void UserCartAction((CartActionEnum, CartProductModel?) resultShowUserCart)
		{
			var action = resultShowUserCart.Item1;
			var product = resultShowUserCart.Item2;
			bool isSuccess = false;
			switch (action)
			{
				case CartActionEnum.Buy:
					isSuccess = _userRepository.BuyProducts(_currentLoggedInUser);
					break;

				case CartActionEnum.RemoveAll:
					isSuccess = _userRepository.RemoveAllProductsFromCart(_currentLoggedInUser);
					_buyerView.ShowMessage(isSuccess ? ConstString.RemoveAllProductsFromCartSuccess : ConstString.RemoveAllProductsFromCartFail);
					break;

				case CartActionEnum.Remove when product != null:
					isSuccess = _userRepository.RemoveProductFromCart(product, _currentLoggedInUser);
					_buyerView.ShowMessage(isSuccess ? ConstString.RemoveFromCartSuccess : ConstString.RemoveFromCartFail);
					break;

				case CartActionEnum.Update when product != null:
					isSuccess = _userRepository.UpdateProductInCart(product, _currentLoggedInUser);
					_buyerView.ShowMessage(isSuccess ? ConstString.UpdateInCartSuccess : ConstString.UpdateInCartFail);
					break;

				case CartActionEnum.Exit:
					break;

				default:
					throw new ArgumentOutOfRangeException();
			}
		}

        private void BuyProducts()
		{
            var result = _buyerView.ShowPaymentMethod();
            var ShoppingCartModel = new ShoppingCartModel() 
            {
                PaymentMethod = result.Item1,
				CartProducts = _userRepository.GetCart(_currentLoggedInUser).ToList()
			}

		}
	}
}
