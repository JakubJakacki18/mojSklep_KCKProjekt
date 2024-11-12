using ConsoleApp.Data;
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
            Application.Run(win);
            Application.Shutdown();
        }

		protected void OpenWindow(Window win)
		{
			mainWindow.Add(win);
			top.Add(mainWindow);
			Application.Run(win);
		}
		public bool ExitApp()
        {
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
            return isExitWanted;
        }
		public void ShowMessage(string addProductStatus)
		{
			MessageBox.Query("Produkt", addProductStatus, "Ok");
            
            Application.Shutdown();   
		}

	}
}