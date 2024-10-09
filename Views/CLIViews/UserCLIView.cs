using KCKProjekt.Views.ViewInterfaces;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCKProjekt.Views.CLIViews
{
    public class UserCLIView : IUserView
	{
		public void showSignIn()
		{
			//AnsiConsole.Markup("[underline bold green]Zaloguj się![/]\n");

			//var username = AnsiConsole.Ask<string>("Enter your [green]username[/]:");
			//var password = AnsiConsole.Prompt(
			//	new TextPrompt<string>("Enter your [green]password[/]:")
			//		.PromptStyle("yellow")
			//		.Secret());

			var layout = new Layout("Root")
	.SplitColumns(
		new Layout("Left"),
		new Layout("Right")
			.SplitRows(
				new Layout("Top"),
				new Layout("Bottom")));

			// Update the left column
			layout["Left"].Update(
				new Panel(
					Align.Center(
						new Markup("Hello [blue]World![/]"),
						VerticalAlignment.Middle))
					.Expand());

			// Render the layout
			AnsiConsole.Write(layout);
		}

		public void showSignUp()
		{
			throw new NotImplementedException();
		}
	}
}
