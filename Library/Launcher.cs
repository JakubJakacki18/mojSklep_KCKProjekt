
using Library.Controllers;
using Library.Interfaces;
using Library.Repository;

namespace Library;

public class Launcher(IUserView _userView, IBuyerView _buyerView, ISellerView _sellerView)
{
    public async Task RunAsync()
    {
        ApplicationDbContext context = new();
        IUserRepository userRepository = new UserRepository(context);
        IProductRepository productRepository = new ProductRepository(context);
        var userController = UserController.Initialize(_userView, userRepository);
        do
        {
            var loggedUser = await userController.SignInOrUpSelectionAsync();
            int choosedInterface = await userController.RoleSelecion(loggedUser);
            switch (choosedInterface)
            {
                case 0:
                    var buyerController = BuyerController.Initialize(_buyerView, userRepository, productRepository);
                    await buyerController.ShowMenu();
                    break;
                case 1:
                    var sellerController = SellerController.Initialize(_sellerView, productRepository);
                    await sellerController.ShowMenu();
                    break;
                default:
                    break;
            }
        } while (userController.CurrentLoggedInUser == null);
    }
}