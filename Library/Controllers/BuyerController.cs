using Library.Data;
using Library.Interfaces;
using Library.Models;

namespace Library.Controllers
{
    public class BuyerController
    {
        private readonly IBuyerView _buyerView;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly UserModel _currentLoggedInUser;

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

        private static BuyerController _instance;

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
                            bool isSuccess = false;
							resultShowUserCart= _buyerView.ShowUserCart(_userRepository.GetCart(_currentLoggedInUser).ToList());
                            if (resultShowUserCart.Item2 == null)
                                continue;
							if (resultShowUserCart.Item1 == CartActionEnum.Remove)
							{
								isSuccess =_userRepository.RemoveProductFromCart(resultShowUserCart.Item2, _currentLoggedInUser);
								_buyerView.ShowMessage((isSuccess) ? ConstString.RemoveFromCartSuccess : ConstString.RemoveFromCartFail);
							}
                            if(resultShowUserCart.Item1 == CartActionEnum.Update)
                            {
                                isSuccess =_userRepository.UpdateProductInCart(resultShowUserCart.Item2, _currentLoggedInUser);
								_buyerView.ShowMessage((isSuccess) ? ConstString.UpdateInCartSuccess : ConstString.UpdateInCartFail);

							}
						} while (resultShowUserCart.Item1 != CartActionEnum.Exit);
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
    }
}
