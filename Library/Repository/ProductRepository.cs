using Library.Interfaces;
using Library.Models;

namespace Library.Repository
{
    public class ProductRepository(ApplicationDbContext context) : IProductRepository
    {
        private readonly ApplicationDbContext _context = context;
        public bool AddProduct(ProductModel product)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
