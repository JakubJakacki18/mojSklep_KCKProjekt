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
                        object? result;
                        do
                        {
                            result = _buyerView.ShowAllProducts(_productRepository.GetProducts().ToList(),_userRepository.GetCart(_currentLoggedInUser).ToList());
                            if (result is CartProductModel cartProduct)
                            {
                                AddProductToCart(cartProduct);
                            }
                        } while (result is CartProductModel);
                        break;
                    case 2:
                        _buyerView.ShowUserCart(_userRepository.GetCart(_currentLoggedInUser).ToList());

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
            if (!result)
            {
                _buyerView.ShowMessage(ConstString.AddToCartFail);
            }
            else
            {
                _buyerView.ShowMessage(ConstString.AddToCartSuccess);
            }


        }
    }
}
