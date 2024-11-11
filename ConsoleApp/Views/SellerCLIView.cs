using Library.Data;
using Library.Interfaces;
using Terminal.Gui;

namespace ConsoleApp.Views
{
    public class SellerCLIView : RoleCLIView, ISellerView
    {
        private Window mainWindow;
        private Toplevel top;
        public int ShowMenu()
        {
            Application.Init();
            top = Application.Top;
            mainWindow = new Window("Sklep internetowy - " + ConstString.AppName)
            {
                X = 0, // Położenie okna w poziomie
                Y = 1, // Położenie okna w pionie
                Width = Dim.Fill(), // Rozciągnięcie okna na całą szerokość
                Height = Dim.Fill() // Rozciągnięcie okna na całą wysokość
            };
            // Stworzenie okna dialogowego, które będzie pełniło rolę menu
            var menuWindow = new Window("Menu")
            {
                X = Pos.Center(),
                Y = Pos.Center(),
                Width = 40,
                Height = 10,
                ColorScheme = ColorTheme.GrayThemePalette
            };
            int iter = 1;
            int selection = -1;
            var addNewProductButton = new Button($"_{iter++}. Dodaj nowy produkt")
            {
                X = Pos.Center(),
                Y = 1
            };
            addNewProductButton.Clicked += () =>
            {
                selection = 1;
                Application.RequestStop();

            };
            var showAllProducts = new Button($"_{iter++}. Przejrzyj produkty")
            {
                X = Pos.Center(),
                Y = 2
            };
            showAllProducts.Clicked += () =>
            {
                selection = 2;
                Application.RequestStop();

            };
            var exitShopButton = new Button($"_{iter++}. Wyjdź z {ConstString.AppName}")
            {
                X = Pos.Center(),
                Y = 3
            };
            exitShopButton.Clicked += () =>
            {
                selection = 3;
                Application.RequestStop();
            };
            menuWindow.Add(addNewProductButton, showAllProducts, exitShopButton);
            top.Add(mainWindow);
            //mainWindow.Add(GetMenuBar());
            mainWindow.Add(menuWindow);
            Application.Run();
            return selection;
        }


        public void AddProduct()
        {
            var win = new Window("Dodaj produkt")
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(1),
                Height = Dim.Fill(1),
                ColorScheme = ColorTheme.GrayThemePalette
            };
        }

        public void EditProduct()
        {
            throw new NotImplementedException();
        }
    }
}
