
using Library.Controllers;
using Library.Interfaces;
using Library.Repository;

namespace Library;

public class Launcher(/*Io io,*/ IUserView _userView, IBuyerView _buyerView, ISellerView _sellerView/*, IAdminView _adminView*/)
{

    public async Task RunAsync()
    {
        ApplicationDbContext context = new ApplicationDbContext();
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
                    //await sellerController.RunAsync(loggedUser);
                    break;
                //case 2:
                //    //var adminController = new AdminController(_adminView, userRepository);
                //    //await adminController.RunAsync(loggedUser);
                //    break;
                default:
                    break;
            }
        } while (userController.CurrentLoggedInUser == null);





        //int.TryParse(io.Input(), out int f);
        //int.TryParse(io.Input(), out int s);

        //var calc = Calculator.Add(f, s);
        //io.Output(calc.ToString());
    }
}