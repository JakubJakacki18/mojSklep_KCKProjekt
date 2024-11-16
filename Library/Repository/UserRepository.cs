using Library.Interfaces;
using Library.Model;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Repository
{
    public class UserRepository : IUserRepository
    {
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        private readonly ApplicationDbContext _context;

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
            return _context.SaveChanges() > 0;
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


                var userWithCart = _context.Users
                    .Include(u => u.ShoppingCart)
                    .ThenInclude(sc => sc.ProductsInCart)
                    .FirstOrDefault(u => u.UserId == currentLoggedInUser.UserId);
                if (userWithCart == null)
                {
                    throw new Exception("User with cart is not found");
                }

                userWithCart.ShoppingCart.ProductsInCart.Add(cartProduct);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            //currentLoggedInUser.ShoppingCart.ProductsInCart.Add(cartProduct);
            return SaveChanges();
        }


        public IEnumerable<CartProductModel> GetCart(UserModel currentLoggedInUser)
            => currentLoggedInUser.ShoppingCart.ProductsInCart;
    }
}