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


        public async void ShowMenu()
        {

            bool isExitWanted = false;
            do
            {
                currentLoggedInUser = _userController.CurrentLoggedInUser;
                if (currentLoggedInUser == null)
                {
                    return;
                }
                switch (await _sellerView.ShowMenu())
                {
                    case 1:
                        await AddProduct(await _sellerView.AddProduct());
                        break;
                    case 2:
                        await ShowAllProducts();
                        break;
                    /*case 2:
                        _sellerView.ShowUserCart();
                        break;
                    case 3:
                        _sellerView.ShowPaymentMethod();
                        break;*/
                    case 3:
                        isExitWanted = await _sellerView.ExitApp();
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

        public async Task AddProduct(ProductModel? product)
        {
            if (product != null)
            {
                var result = _productRepository.AddProduct(product);
                if (result)
                {
                    await _sellerView.ShowMessage(ConstString.AddProductSuccess);
                    return;
                }
            }
            await _sellerView.ShowMessage(ConstString.AddProductFail);
        }

        public void ChangeProduct()
        {
        }

        public async Task RemoveProduct(ProductModel? productModel)
        {
            if (productModel != null)
            {
                var result = _productRepository.RemoveProduct(productModel);
                await _sellerView.ShowMessage((result) ? ConstString.RemoveProductSuccess : ConstString.RemoveProductFail);
            }
            else
            {
                await _sellerView.ShowMessage(ConstString.RemoveProductFail);
            }
        }
        public void ShowDetailsOfProduct() { }

        public async Task ShowAllProducts()
        {
            (ShowProductsSellerActionEnum, ProductModel?) result;
            do
            {
                result = await _sellerView.ShowAllProductsAndEdit(_productRepository.GetProducts().ToList());
                switch (result.Item1)
                {
                    case ShowProductsSellerActionEnum.exit:
                        return;
                    case ShowProductsSellerActionEnum.update:
                        await EditProduct(result.Item2);
                        break;
                    case ShowProductsSellerActionEnum.delete:
                        await RemoveProduct(result.Item2);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            } while (result.Item1 != ShowProductsSellerActionEnum.exit);
        }

        public async Task EditProduct(ProductModel? product)
        {
            if (product != null)
            {
                await _sellerView.ShowMessage(_productRepository.SaveChanges() ? ConstString.EditProductSuccess : ConstString.EditProductFail);
            }
            else
            {
                await _sellerView.ShowMessage(ConstString.EditProductFail);
            }
        }




    }
}
