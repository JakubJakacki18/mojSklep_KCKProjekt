using Library.Models;

namespace Library.Interfaces
{
    public interface IUserRepository
    {
        public IQueryable<UserModel> GetUsers();
        public bool AddUser(UserModel user);
        public bool RemoveUser(UserModel user);
        public UserModel? GetUser(string login);
        public UserModel? GetUser(string login, string password);
        public bool SaveChanges();
        public bool AddProductToCart(CartProductModel cartProduct, UserModel currentLoggedInUser);
        IEnumerable<CartProductModel> GetCart(UserModel currentLoggedInUser);
        bool IsProductInCart(CartProductModel cartProduct, UserModel currentLoggedInUser);
        bool UpdateProductInCart(CartProductModel cartProduct, UserModel currentLoggedInUser);
		bool RemoveProductFromCart(CartProductModel cartProduct, UserModel currentLoggedInUser);
		bool RemoveAllProductsFromCart(UserModel currentLoggedInUser);
		bool BuyProducts(UserModel currentLoggedInUser, ShoppingCartHistoryModel shoppingCarthHistoryModel);
        IEnumerable<ShoppingCartHistoryModel> GetShoppingCartHistory(UserModel currentLoggedInUser);
	}
}
