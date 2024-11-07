using Library.Interfaces;

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
                        _buyerView.ShowAllProducts(_productRepository.GetProducts().ToList());
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
    }
}
