using ConsoleApp.Data;
using Library.Controllers;
using Library.Data;
using Terminal.Gui;

namespace ConsoleApp.Views
{
    public abstract class RoleCLIView
    {
        protected Window mainWindow;
        protected Toplevel top;

        protected void OpenWindowAndShutdown(Window win)
        {
            mainWindow.Add(win);
            top.Add(mainWindow);
            Application.Run(top);
            Application.Shutdown();
        }

        protected void OpenFrameAndShutdown(params FrameView[] frames)
        {
            if (mainWindow == null || top == null)
            {
                throw new InvalidOperationException("InitializeWindow() musi być wywołane przed OpenFrameAndShutdown().");
            }
            foreach (var f in frames)
            {
                mainWindow.Add(f);
            }
            top.Add(mainWindow);
            Application.Run(top);
            Application.Shutdown();
        }
        public Task<bool> ExitApp()
        {
            InitializeWindow();
            bool isExitWanted = false;
            var win = new Window("Wyjście")
            {
                X = Pos.Center(),
                Y = Pos.Center(),
                Width = 50,
                Height = 10,
                ColorScheme = ColorTheme.RedThemePalette

            };
            var label = new Label("Czy chcesz wyjść ze sklepu?")
            {
                X = Pos.Center(),
                Y = Pos.Center()
            };
            var yesButton = new Button("Tak")
            {
                X = Pos.Center() - 10,
                Y = Pos.Bottom(label) + 1
            };
            yesButton.Clicked += () =>
            {
                isExitWanted = true;
                Application.RequestStop();
            };
            var noButton = new Button("Nie")
            {
                X = Pos.Center() + 3,
                Y = Pos.Top(yesButton)
            };
            noButton.Clicked += () =>
            {
                Application.RequestStop();
            };


            win.Add(label, yesButton, noButton);
            OpenWindowAndShutdown(win);
            return Task.FromResult(isExitWanted);
        }
        public Task ShowMessage(string addProductStatus)
        {
            Application.Init();
            MessageBox.Query("", addProductStatus, "Ok");
            Application.Shutdown();
			return Task.CompletedTask;
		}

        protected void InitializeWindow()
        {
            Application.Init();
            top = Application.Top;
            mainWindow = new Window("Sklep internetowy - " + ConstString.AppName)
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            top.Add(GetMenuBar());
        }
        private MenuBar GetMenuBar()
        {
            return new MenuBar(new MenuBarItem[]
            {
                new MenuBarItem("Konto", new MenuItem[]
                {
                    new MenuItem("Informacje o użytkowniku", "",() => MessageBox.Query("Informacje o użytkowniku", "Nazwa użytkownika: "+UserController.GetInstance().CurrentLoggedInUser?.Login ?? "" , "Ok")),

                    new MenuItem("Wyloguj się!", "",
                        () => {
                            int result = MessageBox.Query("Wylogowywanie:", "Czy na pewno chcesz się wylogować?", "Tak","Nie");
                            if(result==0)
                            {
                                UserController.GetInstance().Logout();
                                Application.RequestStop();
                            }
                        }),


                }),
                new MenuBarItem("O programie", "", () => MessageBox.Query("O programie", ConstString.AppName+"\nAutor: Jakub Jakacki\nWersja: alpha 0.0.1", "Ok")),
            });
        }
        protected string DescriptionLimiter(string? description, int maxLength = ConstIntegers.MaxLengthOfDescription)
        {
            //p.OriginalProduct.Description?.Substring(0, Math.Min(p.OriginalProduct.Description.Length, 20)) ?? "";
            if (description == null)
            {
                return "";
            }
            if (description.Length > maxLength)
            {
                return $"{description.Substring(0, maxLength - 3)}...";
            }
            return description;
        }

    }
}