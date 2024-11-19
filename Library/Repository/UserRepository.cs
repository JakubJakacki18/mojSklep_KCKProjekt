using Library.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Repository
{
	public class UserRepository(ApplicationDbContext context) : IUserRepository
    {
        private readonly ApplicationDbContext _context = context;

        public IQueryable<UserModel> GetUsers()
        {
            return _context.Users;
        }

        public bool AddUser(UserModel user)
        {
            _context.Users.Add(user);
            return SaveChanges();
        }

        public bool RemoveUser(UserModel user)
        {
            _context.Users.Remove(user);
            return SaveChanges();
        }

        public UserModel? GetUser(string login)
        {
            return _context.Users.FirstOrDefault(u => u.Login == login);
        }

        public bool SaveChanges()
        {
            bool result = false;
            try
            {
                result = _context.SaveChanges() > 0;
            }
            
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return result;
        }

        public UserModel? GetUser(string login, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Login == login);
            if (user != null)
            {
                if (user.Password == password)
                {
                    return user;
                }
            }
            return null;
        }

        public bool AddProductToCart(CartProductModel cartProduct, UserModel currentLoggedInUser)
        {
            try
            {
                UserModelWithCart(currentLoggedInUser)?.ProductsInCart.Add(cartProduct);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return SaveChanges();
        }


        public IEnumerable<CartProductModel> GetCart(UserModel currentLoggedInUser)
            => UserModelWithCartAndOriginalProduct(currentLoggedInUser)?.ProductsInCart ?? new List<CartProductModel>();

        public bool IsProductInCart(CartProductModel cartProduct, UserModel currentLoggedInUser)
        {
            UserModel? user = UserModelWithCartAndOriginalProduct(currentLoggedInUser);
            return user?.ProductsInCart.Any(p => p.OriginalProduct.Id == cartProduct.OriginalProduct.Id) ?? false;
        }

        public bool UpdateProductInCart(CartProductModel cartProduct, UserModel currentLoggedInUser)
        {
            UserModel? user = UserModelWithCartAndOriginalProduct(currentLoggedInUser);
            CartProductModel? product = user?.ProductsInCart.FirstOrDefault(p => p.OriginalProduct.Id == cartProduct.OriginalProduct.Id);
            if (product != null)
            {
                product.Quantity = cartProduct.Quantity;

            }
            return SaveChanges();
        }

        private UserModel? UserModelWithCart(UserModel currentLoggedInUser)
            => _context.Users.Include(p => p.ProductsInCart)
                .FirstOrDefault(u => u.UserId == currentLoggedInUser.UserId);

        private UserModel? UserModelWithCartAndOriginalProduct(UserModel currentLoggedInUser)
            => _context.Users.Include(p => p.ProductsInCart).ThenInclude(c => c.OriginalProduct)
                .FirstOrDefault(u => u.UserId == currentLoggedInUser.UserId);

		private UserModel? UserModelWithCartHistoryAndOriginalProduct(UserModel currentLoggedInUser)
			=> _context.Users.Include(s => s.ShoppingCartHistories).ThenInclude(c => c.CartProducts).ThenInclude(o => o.OriginalProduct)
				.FirstOrDefault(u => u.UserId == currentLoggedInUser.UserId);


		public bool RemoveProductFromCart(CartProductModel cartProduct, UserModel currentLoggedInUser)
		{
			UserModel? user = UserModelWithCartAndOriginalProduct(currentLoggedInUser);
			CartProductModel? product = user?.ProductsInCart.FirstOrDefault(p => p.OriginalProduct.Id == cartProduct.OriginalProduct.Id);
			if (product != null)
			{
				user?.ProductsInCart.Remove(product);
			}
            return SaveChanges();

		}

		public bool RemoveAllProductsFromCart(UserModel currentLoggedInUser)
		{
			UserModel? user = UserModelWithCartAndOriginalProduct(currentLoggedInUser);
            user?.ProductsInCart.Clear();
            return SaveChanges();
		}

		public bool BuyProducts(UserModel currentLoggedInUser, ShoppingCartHistoryModel shoppingCartHistory)
		{
			currentLoggedInUser.ShoppingCartHistories.Add(shoppingCartHistory);
			return SaveChanges() ? RemoveAllProductsFromCart(currentLoggedInUser) : false;
		}

        public IEnumerable<ShoppingCartHistoryModel> GetShoppingCartHistory(UserModel currentLoggedInUser) 
            =>UserModelWithCartHistoryAndOriginalProduct(currentLoggedInUser)?.ShoppingCartHistories ?? new List<ShoppingCartHistoryModel>();
	}

}