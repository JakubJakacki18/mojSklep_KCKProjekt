using KCKProjekt.Controllers;
using Library.Interfaces;

namespace Library;

public class Launcher(Io io, IUserView _userView)
{
    private UserController _userController;
    public void Run()
    {
        _userController = new UserController(_userView);
        var result = _userController.SignInOrUpSelection();





        int.TryParse(io.Input(), out int f);
        int.TryParse(io.Input(), out int s);

        var calc = Calculator.Add(f, s);

        io.Output(calc.ToString());
    }
}