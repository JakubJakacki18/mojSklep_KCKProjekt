using Library.Models;

namespace Library.Interfaces
{
    public interface ISellerView
    {
        public ProductModel? AddProduct();
        public void EditProduct();
        bool ExitApp();
        public int ShowMenu();
		void ShowMessage(string addProductStatus);
	}
}
