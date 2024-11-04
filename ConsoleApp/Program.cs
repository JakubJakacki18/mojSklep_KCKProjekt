using ConsoleApp.Views;
using Library;

var launcher = new Launcher(/*new Io(Console.WriteLine, Console.ReadLine), */new UserCLIView());

launcher.RunAsync();


