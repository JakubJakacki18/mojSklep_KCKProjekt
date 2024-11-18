using Library.Models;

namespace Library.Interfaces
{
    public interface IProductRepository
    {
        public IQueryable<ProductModel> GetProducts();
        public bool AddProduct(ProductModel product);
        public bool RemoveProduct(ProductModel product);
        public ProductModel? GetProduct(string name);
        public ProductModel? GetProduct(int id);
        public bool SaveChanges();
		bool UpdateProductsQuantity(List<CartProductModel> cartContent);
	}
}
