using Library.Interfaces;

namespace Library.Controllers
{
    public class BuyerController
    {
        private IBuyerView _buyerView;

        public BuyerController(IBuyerView buyerView)
        {
            _buyerView = buyerView;
        }


    }
}
