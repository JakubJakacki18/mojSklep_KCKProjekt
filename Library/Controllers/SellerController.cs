using Library.Data;
using Library.Interfaces;
using Library.Models;

namespace Library.Controllers
{
    public class SellerController
    {
        private static SellerController _instance;
        private readonly ISellerView _sellerView;
        private UserModel? currentLoggedInUser;
        private readonly IProductRepository _productRepository;
        private readonly UserController _userController;


		public static SellerController Initialize(ISellerView sellerView, IProductRepository productRepository)
        {
            return _instance = new SellerController(sellerView, productRepository);
        }
        public static SellerController getInstance()
        {
            if (_instance == null)
            {
                throw new Exception("SellerController not initialized");
            }

            return _instance;
        }
        private SellerController(ISellerView sellerView, IProductRepository productRepository)
        {
			_userController = UserController.GetInstance();
			currentLoggedInUser = _userController.CurrentLoggedInUser;
            _sellerView = sellerView;
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
				switch (_sellerView.ShowMenu())
                {
                    case 1:
                        AddProduct(_sellerView.AddProduct());
                        break;
                    case 2:
                        ShowAllProducts();
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
                    default:
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

        public void RemoveProduct(ProductModel? productModel)
        {
            if (productModel != null)
            {
				var result = _productRepository.RemoveProduct(productModel);
				_sellerView.ShowMessage((result) ? ConstString.RemoveProductSuccess : ConstString.RemoveProductFail);
			}
			else
			{
				_sellerView.ShowMessage(ConstString.RemoveProductFail);
			}
		}
            public void ShowDetailsOfProduct() { }

        public void ShowAllProducts()
        {
            (ShowProductsSellerActionEnum, ProductModel?) result;
            do
            {
                result = _sellerView.ShowAllProductsAndEdit(_productRepository.GetProducts().ToList());
                switch(result.Item1)
                {
					case ShowProductsSellerActionEnum.exit:
						return;
					case ShowProductsSellerActionEnum.update:
						EditProduct(result.Item2);
						break;
					case ShowProductsSellerActionEnum.delete:
						RemoveProduct(result.Item2);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			} while (result.Item1!=ShowProductsSellerActionEnum.exit);
		}

		public void EditProduct(ProductModel? product)
		{
			if (product != null)
			{
                _sellerView.ShowMessage(_productRepository.SaveChanges() ? ConstString.EditProductSuccess : ConstString.EditProductFail);
			}
			else
			{
				_sellerView.ShowMessage(ConstString.EditProductFail);
			}
		}

	


    }
}
