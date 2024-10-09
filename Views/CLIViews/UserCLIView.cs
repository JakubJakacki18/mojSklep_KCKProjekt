using KCKProjekt.Views.ViewInterfaces;
using Spectre.Console;

namespace KCKProjekt.Views.CLIViews
{
    public class UserCLIView : IUserView
    {
        public (string, string) showSignIn()
        {
            ConsoleKey consoleKey;
            do
            {
                AnsiConsole.Markup("[underline bold green]Wciśnij enter jeśli chcesz przejść do ekranu logowania lub spację jeśli do rejestracji[/]\n");
                consoleKey = Console.ReadKey().Key;
            } while (consoleKey != ConsoleKey.Enter && consoleKey != ConsoleKey.Spacebar);

            switch (consoleKey)
            {
                case ConsoleKey.Spacebar:
                    showSignUp();
                    break;
                case ConsoleKey.Enter:
                    break;
                default:
                    throw new InvalidOperationException("Coś poszło nie tak");
            }
            AnsiConsole.Markup("[underline bold green]Zaloguj się![/]\n");
            var username = AnsiConsole.Ask<string>("Enter your [green]username[/]:");
            var password = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter your [green]password[/]:")
                    .PromptStyle("yellow")
                    .Secret());

            return (username, password);
            //		var layout = new Layout("Root")
            //.SplitColumns(
            //	new Layout("Left"),
            //	new Layout("Right")
            //		.SplitRows(
            //			new Layout("Top"),
            //			new Layout("Bottom")));

            //		// Update the left column
            //		layout["Left"].Update(
            //			new Panel(
            //				Align.Center(
            //					new Markup("[underline bold green]Zaloguj się![/]\n"),
            //					VerticalAlignment.Middle))
            //				.Expand());

            //		// Render the layout
            //		AnsiConsole.Write(layout);
        }

        public void showSignUp()
        {
            throw new NotImplementedException();
        }
    }
}
