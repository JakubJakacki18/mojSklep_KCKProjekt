
using Library.Controllers;
using Library.Interfaces;
using Library.Repository;

namespace Library;

public class Launcher(/*Io io,*/ IUserView _userView, IBuyerView _buyerView, ISellerView _sellerView, IAdminView _adminView)
{
    private UserController _userController;
    public async Task RunAsync()
    {
        ApplicationDbContext context = new ApplicationDbContext();
        IUserRepository userRepository = new UserRepository(context);
        IProductRepository productRepository = new ProductRepository(context);
        _userController = new UserController(_userView, userRepository);
        var loggedUser = await _userController.SignInOrUpSelectionAsync();
        int choosedInterface = _userController.RoleSelecion(loggedUser);
        switch (choosedInterface)
        {
            case 0:
                var buyerController = new BuyerController(_buyerView, userRepository, productRepository);
                buyerController.ShowMenu();
                break;
            case 1:
                //var sellerController = new SellerController(_sellerView, userRepository);
                //await sellerController.RunAsync(loggedUser);
                break;
            case 2:
                //var adminController = new AdminController(_adminView, userRepository);
                //await adminController.RunAsync(loggedUser);
                break;
        }






        //int.TryParse(io.Input(), out int f);
        //int.TryParse(io.Input(), out int s);

        //var calc = Calculator.Add(f, s);
        //io.Output(calc.ToString());
    }
}