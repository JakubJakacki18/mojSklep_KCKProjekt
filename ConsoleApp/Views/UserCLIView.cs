using Colorful;
using ConsoleApp.Data;
using Library.Data;
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

        private void ShowSignUpErrorMessage(bool isValid)
        {
            if (isValid != true)
            {
                Application.Init();
                MessageBox.Query("Błąd", "Użytkownik o tym loginie już istnieje", "OK");
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

            var win = new Window("Logowanie - " + ConstString.AppName)
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                CanFocus = false
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
                if (username == string.Empty || password == string.Empty)
                {
                    MessageBox.Query("Bład", "Login albo hasło nie zostało uzupełnione", "Ok");
                }
                else
                {
                    Application.RequestStop(); // Zakończenie aplikacji
                }
            };
            win.Add(okButton, passwordSecretButton);
            Application.Run();
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

        public Task<(string, string)> ShowSignUp(bool isValid)
        {
            string username = "";
            string password = "";
            string confirmPassword = "";

            ShowSignUpErrorMessage(isValid);
            Application.Init();
            var top = Application.Top;
            var win = new Window("Rejestracja - " + ConstString.AppName)
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
                Y = Pos.Top(label) + 1,
                Width = 30
            };
            win.Add(inputField);

            // Etykieta dla pola hasła
            var passwordLabel = new Label("Wprowadź swoje hasło:")
            {
                X = 1,
                Y = Pos.Top(inputField) + 2,
            };
            win.Add(passwordLabel);

            // Pole do wprowadzania ukrytego hasła
            var passwordField = new TextField("")
            {
                X = 1,
                Y = Pos.Top(passwordLabel) + 1,
                Width = 30,
                Secret = true
            };
            var confirmPasswordLabel = new Label("Potwierdź swoje hasło:")
            {
                X = 1,
                Y = Pos.Top(passwordField) + 2,
            };
            var confirmPasswordField = new TextField("")
            {
                X = 1,
                Y = Pos.Top(confirmPasswordLabel) + 1,
                Width = 30,
                Secret = true
            };
            var passwordSecretButton = new Button("Wyświetl hasło")
            {
                X = Pos.Right(passwordField) + 1,
                Y = Pos.Top(passwordField)
            };
            passwordSecretButton.Clicked += () =>
            {
                passwordField.Secret = !passwordField.Secret;
                passwordSecretButton.Text = passwordField.Secret ? "Wyświetl hasło" : "Ukryj hasło";
            };

            win.Add(passwordField, confirmPasswordLabel, confirmPasswordField);

            // Przycisk do potwierdzenia
            var okButton = new Button("OK")
            {
                X = 1,
                Y = Pos.Top(confirmPasswordField) + 2
            };

            void HandleSignUp()
            {
                {
                    username = inputField.Text.ToString() ?? string.Empty;
                    password = passwordField.Text.ToString() ?? string.Empty;
                    confirmPassword = confirmPasswordField.Text.ToString() ?? string.Empty;
                    if (username == string.Empty || password == string.Empty)
                    {
                        MessageBox.Query("Bład", "Login albo hasło nie zostało uzupełnione", "Ok");
                        passwordField.Text = "";
                    }
                    else if (!password.Equals(confirmPassword))
                    {
                        MessageBox.Query("Bład", "Hasła nie są takie same", "Ok");
                        confirmPasswordField.Text = "";
                        passwordField.Text = "";
                    }
                    else

                    {
                        Application.RequestStop(); // Zakończenie aplikacji
                    }
                }
            }

            okButton.Clicked += HandleSignUp;
            passwordField.KeyPress += (e) =>
            {
                if (e.KeyEvent.Key == Key.Enter)
                {
                    HandleSignUp();
                }
            };
            win.Add(okButton, passwordSecretButton);
            Application.Run();
            Application.Shutdown();
            return Task.FromResult((username, password));
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

            var win = new Window("Logowanie/Rejestracja - " + ConstString.AppName)
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
            };
            top.Add(win);
            string asciiArtText = String.Empty;
            try
            {
                Figlet figlet = new Figlet();
                asciiArtText = figlet.ToAscii(ConstString.AppName).ToString();
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }

            var titleLabel = new Label(asciiArtText)
            {
                X = Pos.Center(),
                Y = 1,
            };
            win.Add(titleLabel);
            var signFrame = new FrameView()
            {
                X = Pos.Center(),
                Y = Pos.Center(),
                Width = 50,
                Height = 9,
                ColorScheme = ColorTheme.GrayThemePalette
            };
            var label = new Label("Chcesz przejść do logowania czy rejestracji?")
            {
                X = Pos.Center(),
                Y = 1
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
                Text = "Za_rejestruj się!",
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
            win.Add(signFrame);
            signFrame.Add(label);
            signFrame.Add(signInButton);
            signFrame.Add(signUpButton);



            Application.Run();
            Application.Shutdown();
            if (desire == null)
            {
                throw new InvalidOperationException("Coś poszło nie tak");
            }
            return Task.FromResult(desire.Value);
        }

        private void ShowProggresBar()
        {
            var progresBar = new ProgressBar()
            {

            };


        }

        public int RoleSelection(RolesEnum roles)
        {
            if (roles == RolesEnum.PermissionBuyer)
            {
                return 0;
            }

            int val = -1;
            Application.Init();
            var top = Application.Top;
            var bigWin = new Window("Wybór roli - " + ConstString.AppName)
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
            };
            var win = new FrameView()
            {
                X = Pos.Center(),
                Y = Pos.Center(),
                Width = 30,
                Height = 10,
                ColorScheme = ColorTheme.GrayThemePalette,
            };
            top.Add(bigWin);
            bigWin.Add(win);
            var label = new Label("Zaloguj się jako:")
            {
                X = Pos.Center(),
                Y = Pos.Center() - 3,
            };
            win.Add(label);
			//if (roles.HasFlag(RolesEnum.Admin))
			//{
			//    var adminButton = new Button()
			//    {
			//        Text = "Administrator",
			//        Y = Pos.Bottom(label) + 5,

			//        // center the login button horizontally
			//        X = Pos.Center(),
			//        IsDefault = false,
			//    };
			//    adminButton.Clicked += () =>
			//    {
			//        val = 2;
			//        Application.RequestStop();
			//    };
			//    win.Add(adminButton);
			//}
			if (roles.HasFlag(RolesEnum.Seller))
            {
                var sellerButton = new Button()
                {
                    Text = "Sprzedający",
                    Y = Pos.Bottom(label) + 3,
                    X = Pos.Center(),
                    IsDefault = false,
                };
                sellerButton.Clicked += () =>
                {
                    val = 1;
                    Application.RequestStop();
                };
                win.Add(sellerButton);
            }
			if (roles.HasFlag(RolesEnum.Buyer))
			{
				var buyerButton = new Button()
				{
					Text = "Klient sklepu",
					Y = Pos.Bottom(label)+4,
					X = Pos.Center(),
					IsDefault = false,
				};
				buyerButton.Clicked += () =>
				{
					val = 0;
					Application.RequestStop();
				};
				win.Add(buyerButton);
			}

			Application.Run();
            Application.Shutdown();
            return val;
        }
    }
}
