using Library.Interfaces;
using Library.Models;

namespace Library.Repository
{
    public class ProductRepository(ApplicationDbContext context) : IProductRepository
    {
        private readonly ApplicationDbContext _context = context;
        public bool AddProduct(ProductModel product)
        {
			_context.Products.Add(product);
			return SaveChanges();
		}

        public ProductModel? GetProduct(string name)
        {
            throw new NotImplementedException();
        }

        public ProductModel? GetProduct(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<ProductModel> GetProducts()
        {
            return _context.Products;
        }

        public bool RemoveProduct(ProductModel product)
        {
            throw new NotImplementedException();
        }

        public bool SaveChanges()
        {
			return _context.SaveChanges() > 0;
		}

		public bool UpdateProductsQuantity(List<CartProductModel> cartContent)
		{
			foreach (var cartProduct in cartContent)
			{
				var product = _context.Products.FirstOrDefault(p => p.Id == cartProduct.ProductId);
				if (product != null)
				{
                    if(product.Quantity- cartProduct.Quantity>0)
					    product.Quantity -= cartProduct.Quantity;
				}
			}
			return SaveChanges();
		}
	}
}
