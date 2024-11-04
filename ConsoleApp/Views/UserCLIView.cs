using Library.Interfaces;
using Terminal.Gui;


namespace ConsoleApp.Views
{
    public class UserCLIView : IUserView
    {
        private void ShowSignInErrorMessage(bool isValid)
        {
            if (isValid != true)
            {
                Application.Init();
                MessageBox.Query("Błąd", "Wprowadzone dane są niepoprawne, spróbuj jeszcze raz", "OK");
                Application.Shutdown();

            }
        }

        public Task<(string, string)> ShowSignIn(bool isValid)
        {
            string username = "";
            string password = "";
            ShowSignInErrorMessage(isValid);
            Application.Init();
            var top = Application.Top;

            var win = new Window("Logowanie")
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
            };
            top.Add(win);

            // Etykieta dla pola tekstowego
            var label = new Label("Wprowadź swój login:")
            {
                X = 1,
                Y = 1,
            };
            win.Add(label);

            // Pole tekstowe
            var inputField = new TextField("")
            {
                X = 1,
                Y = 2,
                Width = 30
            };
            win.Add(inputField);

            // Etykieta dla pola hasła
            var passwordLabel = new Label("Wprowadź swoje hasło:")
            {
                X = 1,
                Y = 4,
            };
            win.Add(passwordLabel);

            // Pole do wprowadzania ukrytego hasła
            var passwordField = new TextField("")
            {
                X = 1,
                Y = 5,
                Width = 30,
                Secret = true // Ustawienie ukrycia wprowadzanego tekstu
            };

            var passwordSecretButton = new Button("Wyświetl hasło")
            {
                X = Pos.Right(passwordField) + 1,
                Y = 5
            };
            passwordSecretButton.Clicked += () =>
            {
                passwordField.Secret = !passwordField.Secret;
                passwordSecretButton.Text = passwordField.Secret ? "Wyświetl hasło" : "Ukryj hasło";
            };

            win.Add(passwordField);

            // Przycisk do potwierdzenia
            var okButton = new Button("OK")
            {
                X = 1,
                Y = 7
            };
            okButton.Clicked += () =>
            {
                username = inputField.Text.ToString() ?? string.Empty;
                password = passwordField.Text.ToString() ?? string.Empty;
                Application.RequestStop(); // Zakończenie aplikacji
            };
            win.Add(okButton, passwordSecretButton);


            Application.Run();
            // Uruchomienie aplikacji
            Application.Shutdown();





            /*
            AnsiConsole.Markup("[underline bold green]Zaloguj się![/]\n");
            var username = AnsiConsole.Ask<string>("Enter your [green]username[/]:");
            var password = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter your [green]password[/]:")
                    .PromptStyle("yellow")
                    .Secret());
                    */

            return Task.FromResult((username, password));
        }

        public Task<(string, string)> ShowSignUp()
        {
            throw new NotImplementedException();
        }

        public Task<bool> LandingPage()
        {
            /*ConsoleKey consoleKey;
            do
            {
                AnsiConsole.Markup("[underline bold green]Wciśnij enter jeśli chcesz przejść do ekranu logowania lub spację jeśli do rejestracji[/]\n");
                consoleKey = Console.ReadKey().Key;
            } while (consoleKey != ConsoleKey.Enter && consoleKey != ConsoleKey.Spacebar);

            switch (consoleKey)
            {
                case ConsoleKey.Spacebar:
                    return Task.FromResult(false);
                case ConsoleKey.Enter:
                    return Task.FromResult(true);
                default:
                    throw new InvalidOperationException("Coś poszło nie tak");
            }*/
            bool? desire = null;
            Application.Init();

            var top = Application.Top;

            var win = new Window("Logowanie/Rejestracja - mójSklep")
            {
                X = 0,
                Y = 1,   // leave one row for menubar
                Width = Dim.Fill(),
                Height = Dim.Fill(),
            };
            top.Add(win);
            var label = new Label("Chcesz przejść do logowania czy rejestracji?")
            {
                X = Pos.Center(),
                Y = 1,
            };
            var signInButton = new Button()
            {
                Text = "Zaloguj się!",
                Y = Pos.Bottom(label) + 2,

                // center the login button horizontally
                X = Pos.Center(),
                IsDefault = false
            };
            var signUpButton = new Button()
            {
                Text = "Zarejestruj się!",
                Y = Pos.Bottom(label) + 3,

                // center the login button horizontally
                X = Pos.Center(),
                IsDefault = false,

            };

            signInButton.Clicked += () =>
            {
                desire = true;
                Application.RequestStop(); // Zakończ aplikację
            };

            // Obsługa kliknięcia przycisku "Zarejestruj się!"
            signUpButton.Clicked += () =>
            {
                desire = false;
                Application.RequestStop(); // Zakończ aplikację
            };
            win.Add(label);
            win.Add(signInButton);
            win.Add(signUpButton);

            Application.Run();
            Application.Shutdown();
            if (desire == null)
            {
                throw new InvalidOperationException("Coś poszło nie tak");
            }
            return Task.FromResult(desire.Value);
        }

        public void LogAs()
        {


        }
    }
}
