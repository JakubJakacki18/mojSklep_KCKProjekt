using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Views
{
    public static class InterfaceView
    {
        public static string? ChoiceOfInterface()
            => AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Jaki styl [green]interfejsu[/] wybierasz?")
                .PageSize(3)
                .AddChoices([
                    "Tekstowy", "Graficzny"
                ]));




    }
}
