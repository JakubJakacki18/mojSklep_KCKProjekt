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
        private readonly UserController _userController;
		private UserModel? currentLoggedInUser;
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
			_userController = UserController.GetInstance();
			currentLoggedInUser = _userController.CurrentLoggedInUser;
            _buyerView = buyerView;
            _userRepository = userRepository;
            _productRepository = productRepository;
        }

        

        public void ShowMenu()
        {
            bool isExitWanted = false;
            do
            {
				currentLoggedInUser = _userController.CurrentLoggedInUser;
				if (currentLoggedInUser == null)
                {
                    return;
                }
                switch (_buyerView.ShowMenu())
                {
                    case 1:
                        object? resultShowAllProducts;
                        do
                        {
                            resultShowAllProducts = _buyerView.ShowAllProducts(_productRepository.GetProducts().ToList(),_userRepository.GetCart(currentLoggedInUser).ToList());
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
							resultShowUserCart = _buyerView.ShowUserCart(_userRepository.GetCart(currentLoggedInUser).ToList());
                            isExitWanted =UserCartAction(resultShowUserCart);
						}
						while (resultShowUserCart.Item1 != CartActionEnum.Exit && isExitWanted==false);
						break;
                    case 3:
                        _ =BuyProducts();
						break;
                    case 4:
                        _buyerView.ShowShoppingHistory(_userRepository.GetShoppingCartHistory(currentLoggedInUser).ToList());
                        break;
                    case 5:
						isExitWanted = _buyerView.ExitApp();
						break;
					default:
                        break;
				}
            } while (!isExitWanted);
        }

        private void AddProductToCart(CartProductModel cartProduct)
        {
            if(currentLoggedInUser==null)
                throw new Exception("User is not logged in");
			bool result = _userRepository.IsProductInCart(cartProduct, currentLoggedInUser)
                ? _userRepository.UpdateProductInCart(cartProduct, currentLoggedInUser)
                : _userRepository.AddProductToCart(cartProduct, currentLoggedInUser);
            _buyerView.ShowMessage((result) ? ConstString.AddToCartSuccess : ConstString.AddToCartFail);
        }

        private bool UserCartAction((CartActionEnum, CartProductModel?) resultShowUserCart)
		{
			if (currentLoggedInUser == null)
				throw new Exception("User is not logged in");
			var action = resultShowUserCart.Item1;
			var product = resultShowUserCart.Item2;
            bool isSuccess, isLogout =false ;
			switch (action)
			{
				case CartActionEnum.Buy:
                    isLogout = !BuyProducts();
					break;

				case CartActionEnum.RemoveAll:
					isSuccess = _userRepository.RemoveAllProductsFromCart(currentLoggedInUser);
					_buyerView.ShowMessage(isSuccess ? ConstString.RemoveAllProductsFromCartSuccess : ConstString.RemoveAllProductsFromCartFail);
					break;

				case CartActionEnum.Remove when product != null:
					isSuccess = _userRepository.RemoveProductFromCart(product, currentLoggedInUser);
					_buyerView.ShowMessage(isSuccess ? ConstString.RemoveFromCartSuccess : ConstString.RemoveFromCartFail);
					break;

				case CartActionEnum.Update when product != null:
					isSuccess = _userRepository.UpdateProductInCart(product, currentLoggedInUser);
					_buyerView.ShowMessage(isSuccess ? ConstString.UpdateInCartSuccess : ConstString.UpdateInCartFail);
					break;

				case CartActionEnum.Exit:
					break;

				default:
					throw new ArgumentOutOfRangeException();
			}
			return isLogout;
		}

        private bool BuyProducts()
		{
			if (currentLoggedInUser == null)
				throw new Exception("User is not logged in");
			var cartContent = _userRepository.GetCart(currentLoggedInUser).ToList();
            if (cartContent.Count == 0)
			{
				_buyerView.ShowMessage("Koszyk jest pusty");
				return true; 
			}
			var result = _buyerView.ShowPaymentMethod(cartContent);
           
            if (result == PaymentMethodEnum.None)
            {
				return true;
			}
            if (result == PaymentMethodEnum.Exit)
			{
                return false;
			}
			var shoppingCarthHistoryModel = new ShoppingCartHistoryModel()
            {
                PaymentMethod = result,
                PurchasedProducts = cartContent.Select(cartProduct => new PurchasedProductModel()
				{
                    ProductId = cartProduct.OriginalProduct.Id,
					Price = cartProduct.OriginalProduct.Price,
					Description = cartProduct.OriginalProduct.Description,
					Name = cartProduct.OriginalProduct.Name,
                    Quantity = cartProduct.Quantity,
                    UserId = cartProduct.UserId,
                    User = cartProduct.User
                }).ToList(),
				User = currentLoggedInUser,
                Date = DateTime.Now,
                TotalPrice = cartContent.Sum(p => p.Quantity * p.OriginalProduct.Price),
            };
			bool isSuccessChangeQuantity = _productRepository.UpdateProductsQuantity(cartContent);
			bool isSuccessBuyProducts = _userRepository.BuyProducts(currentLoggedInUser, shoppingCarthHistoryModel);
			_buyerView.ShowMessage((isSuccessChangeQuantity&& isSuccessBuyProducts) ? ConstString.BuyProductsSuccess : ConstString.BuyProductsFail);
            return true;
            
		}


	}
}
