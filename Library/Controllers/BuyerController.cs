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
                        BuyProducts();
						break;
                    case 4:
                        _buyerView.ShowShoppingHistory(_userRepository.GetShoppingCartHistory(_currentLoggedInUser).ToList());
                        break;
                    case 5:
						isExitWanted = _buyerView.ExitApp();
						break;
					default:
						throw new ArgumentOutOfRangeException();
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
                    BuyProducts();
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
			var cartContent = _userRepository.GetCart(_currentLoggedInUser).ToList();
            if (cartContent.Count == 0)
			{
				_buyerView.ShowMessage("Koszyk jest pusty");
				return;
			}
			var result = _buyerView.ShowPaymentMethod(cartContent);
           
            if (result == PaymentMethodEnum.None)
            {
                return;
            }
            var shoppingCarthHistoryModel = new ShoppingCartHistoryModel()
            {
                PaymentMethod = result,
                CartProducts = cartContent.Select(cartProduct => new CartProductModel
                {
                    ProductId = cartProduct.ProductId,
                    OriginalProduct = cartProduct.OriginalProduct,
                    Quantity = cartProduct.Quantity,
                    UserId = cartProduct.UserId,
                    User = cartProduct.User
                }).ToList(),
				User = _currentLoggedInUser,
                Date = DateTime.Now,
                TotalPrice = cartContent.Sum(p => p.Quantity * p.OriginalProduct.Price),
            };
			bool isSuccessChangeQuantity = _productRepository.UpdateProductsQuantity(cartContent);
			bool isSuccessBuyProducts = _userRepository.BuyProducts(_currentLoggedInUser, shoppingCarthHistoryModel);
			_buyerView.ShowMessage((isSuccessChangeQuantity&& isSuccessBuyProducts) ? ConstString.BuyProductsSuccess : ConstString.BuyProductsFail);
		}


	}
}
