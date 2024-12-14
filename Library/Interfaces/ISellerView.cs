using Library.Data;
using Library.Models;

namespace Library.Interfaces
{
    public interface ISellerView
    {
        public Task<ProductModel?> AddProduct();
        public Task EditProduct();
        Task<bool> ExitApp();
        public Task<int> ShowMenu();
        Task ShowMessage(string addProductStatus);
        Task<(ShowProductsSellerActionEnum, ProductModel?)> ShowAllProductsAndEdit(List<ProductModel> product);
    }
}
