using Library.Data;
using Library.Interfaces;
using Library.Models;

namespace Library.Controllers
{
    public class SellerController
    {
        private static SellerController _instance;
        private ISellerView _sellerView;
        private UserModel currentLoggedInUser;
        private IProductRepository _productRepository;


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
            currentLoggedInUser = UserController.GetInstance().CurrentLoggedInUser;
            _sellerView = sellerView;
            _productRepository = productRepository;
        }


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
                        throw new ArgumentOutOfRangeException();
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
				if (result)
				{
					_sellerView.ShowMessage(ConstString.RemoveProductSuccess);
					return;
				}
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
				_productRepository.SaveChanges();
				//_sellerView.ShowMessage(ConstString.EditProductSuccess);
			}
			else
			{
				//_sellerView.ShowMessage(ConstString.EditProductFail);
			}
		}

	


    }
}
