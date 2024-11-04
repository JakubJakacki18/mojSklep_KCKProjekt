using KCKProjekt.Controllers;
using Library.Interfaces;
using Library.Repository;

namespace Library;

public class Launcher(/*Io io,*/ IUserView _userView)
{
    private UserController _userController;
    public async Task RunAsync()
    {
        ApplicationDbContext context = new ApplicationDbContext();
        IUserRepository userRepository = new UserRepository(context);
        _userController = new UserController(_userView, userRepository);
        var result = await _userController.SignInOrUpSelectionAsync();





        //int.TryParse(io.Input(), out int f);
        //int.TryParse(io.Input(), out int s);

        //var calc = Calculator.Add(f, s);
        //io.Output(calc.ToString());
    }
}