using ConsoleApp.Data;
using ConsoleApp.Services;
using Library.Data;
using Library.Interfaces;
using Library.Models;
using System.Text;
using Terminal.Gui;

namespace ConsoleApp.Views
{
    public class BuyerCLIView : RoleCLIView, IBuyerView
    {
		public Object? ShowAllProducts(List<ProductModel> products)
        {
            Object? result = null;
            var win = new Window("Produkty")
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(1),
                Height = Dim.Fill(1),
                ColorScheme = ColorTheme.GrayThemePalette
            };

			if (products.Count == 0)
			{
				var nullLabel = new Label("Brak produktów w bazie")
				{
					X = Pos.Center(),
					Y = Pos.Center()
				};
                var exitNullButton = new Button("Zamknij")
				{
					X = Pos.Center(),
					Y = Pos.Bottom(nullLabel) + 1
				};
				exitNullButton.Clicked += () =>
				{
					Application.RequestStop();
				};
				win.Add(nullLabel, exitNullButton);
				OpenWindowAndShutdown(win);
				return result;
			}
			var productNames = products.Select(p => new string[]
            {
                p.Id.ToString(),
                p.Name,
                p.Description ?? "",
                p.Price.ToString("0.00")
            }).ToList();
            string[] header = { "Kod kreskowy", "Nazwa", "Opis", "Cena" };
            int[] columnWidths = header.Select(h => h.Length).ToArray();
            for (int col = 0; col < productNames[0].Length; col++)
            {
                columnWidths[col] = Math.Max(columnWidths[col], productNames.Max(row => row[col].Length));
            }

            string[] table = productNames.Select(p =>
            {
                StringBuilder s = new();
                for (int i = 0; i < p.Length; i++)
                {
                    s.Append(" | ");
                    s.Append(p[i].PadRight(columnWidths[i]));
                }
                s.Append(" | ");
                return s.ToString();

            }).ToArray();

            var label = new Label("Lista produktów")
            {
                X = Pos.Center(),
                Y = 0
            };
            win.Add(label);
			
			string tableHeader = ((Func<string>)(() =>
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" | ");
                for (int j = 0; j < header.Length; j++)
                {
                    sb.Append(header[j].PadRight(columnWidths[j]));
                    sb.Append(" | ");
                }
                return sb.ToString();
            }))();
            var tableHeaderLabel = new Label(tableHeader)
            {
                X = Pos.Center(),
                Y = 1
            };
            win.Add(tableHeaderLabel);


			var listView = new ListView(table)
            {
                X = Pos.Center(),
                Y = 2,
                Width = table[0].Length,
                Height = Dim.Fill() - 2,
                AllowsMarking = false
            };


			listView.OpenSelectedItem += (args) =>
            {
				var windowDetails = new Window("Szczegóły produktu")
				{
					X = 0,
					Y = 1,
					Width = Dim.Fill(1),
					Height = Dim.Fill(1),
					ColorScheme = ColorTheme.GrayThemePalette
				};
				var idLabel = new Label("Kod kreskowy: " + products[args.Item].Id)
				{
					X = Pos.Center(),
					Y = 1
				};
				var nameLabel = new Label("Nazwa: " + products[args.Item].Name)
				{
					X = Pos.Center(),
					Y = Pos.Bottom(idLabel)
				};
				var descriptionLabel = new Label("Opis: " + products[args.Item].Description)
				{
					X = Pos.Center(),
					Y = Pos.Bottom(nameLabel)
				};
				var priceLabel = new Label("Cena: " + products[args.Item].Price)
				{
					X = Pos.Center(),
					Y = Pos.Bottom(descriptionLabel)
				};
				var quantityLabel = new Label("Ilość dostęnych sztuk: " + products[args.Item].Quantity)
				{
					X = Pos.Center(),
					Y = Pos.Bottom(priceLabel)
				};
				var quantityQuestionLabel = new Label("Ile sztuk chcesz dodać do koszyka?")
				{
					X = Pos.Center(),
					Y = Pos.Bottom(quantityLabel) + 2
				};
				var quantityQuestionTextField = new TextField("")
				{
					X = Pos.Center(),
					Y = Pos.Bottom(quantityQuestionLabel),
					Width = 30,

				};
				var priceToPayLabel = new Label("")
				{
					X = Pos.Center(),
					Y = Pos.Bottom(quantityQuestionTextField)
				};
				var addToCartButton = new Button("Dodaj do koszyka")
				{
					X = Pos.Center(),
					Y = Pos.Bottom(priceToPayLabel) + 1
				};
				addToCartButton.Clicked += () =>
				{
					if (int.TryParse(quantityQuestionTextField.Text.ToString(), out int quantity) && (quantity > 0 && quantity <= products[args.Item].Quantity))
					{
						MessageBox.Query("Dodano do koszyka", "Dodano produkt do koszyka", "Ok");
						result = new CartProductModel()
						{
							OriginalProduct = products[args.Item],
							Quantity = quantity
						};
						Application.RequestStop();
					}
					else
					{
						MessageBox.ErrorQuery("Błąd", "Niepoprawna ilość produktów, popraw wartość", "Ok");
					}
				};
				var rejectAddingToCartButton = new Button("Zrezygnuj z dodawania do koszyka")
				{
					X = Pos.Right(addToCartButton),
					Y = Pos.Top(addToCartButton)
				};
				rejectAddingToCartButton.Clicked += () =>
				{
					win.Remove(windowDetails);
				};

				TextFieldValidator.AllowOnlyIntegers(quantityQuestionTextField);
				quantityQuestionTextField.TextChanged += (_) =>
				{
					if (int.TryParse(quantityQuestionTextField.Text.ToString(), out int quantity))
					{
						priceToPayLabel.Text = $"Cena do zapłaty: {quantity * products[args.Item].Price} {ConstString.Currency}";

					}
				};

				windowDetails.Add(idLabel,
					nameLabel,
					descriptionLabel,
					priceLabel,
					quantityLabel,
					quantityQuestionLabel,
					quantityQuestionTextField,
					priceToPayLabel,
					addToCartButton,
					rejectAddingToCartButton);
				win.Add(windowDetails);



			};
            win.Add(listView);
			var exitButton = new Button("Zamknij")
			{
				Y = Pos.Top(listView),
				X = Pos.Right(listView) + 1
			};
			exitButton.Clicked += () =>
			{
				Application.RequestStop();
			};
			win.Add(exitButton);
			OpenWindowAndShutdown(win);
            return result;

        }

        public void ShowInterface()
        {

        }
		public int ShowMenu()
        {
            //Console.WriteLine("1. View Books");
            //Console.WriteLine("2. View Authors");
            //Console.WriteLine("3. View Genres");
            //Console.WriteLine("4. View Publishers");
            //Console.WriteLine("5. View Orders");
            //Console.WriteLine("6. View Cart");
            //Console.WriteLine("7. Add to Cart");
            //Console.WriteLine("8. Remove from Cart");
            //Console.WriteLine("9. Checkout");
            //Console.WriteLine("10. Logout");
            // Inicjalizacja aplikacji
            InitializeWindow();
			// Stworzenie okna dialogowego, które będzie pełniło rolę menu
			var menuWindow = new FrameView("Menu")
            {
                X = Pos.Center(),
                Y = Pos.Center(),
                Width = 40,
                Height = 10,
                ColorScheme = ColorTheme.GrayThemePalette
            };
            int iter = 1;
            int selection = -1;
            var showProductListButton = new Button($"_{iter++}. Zobacz listę naszych produktów")
            {
                X = Pos.Center(),
                Y = 1
            };
            showProductListButton.Clicked += () =>
            {
                selection = 1;
                Application.RequestStop();

            };
            var showCartButton = new Button($"_{iter++}. Zobacz swój koszyk")
            {
                X = Pos.Center(),
                Y = 2
            };
            showCartButton.Clicked += () =>
            {
                selection = 2;
                Application.RequestStop();

            };
            var finalizeShoppingButton = new Button($"_{iter++}. Zapłać za zakupy")
            {
                X = Pos.Center(),
                Y = 3
            };
            finalizeShoppingButton.Clicked += () =>
            {
                selection = 3;
                Application.RequestStop();

            };
            var exitShopButton = new Button($"_{iter++}. Wyjdź z {ConstString.AppName}")
            {
                X = Pos.Center(),
                Y = 4
            };
            exitShopButton.Clicked += () =>
            {
                selection = 4;
                Application.RequestStop();
            };
            menuWindow.Add(showProductListButton, showCartButton, finalizeShoppingButton, exitShopButton);
            top.Add(mainWindow);
            mainWindow.Add(GetMenuBar());
            mainWindow.Add(menuWindow);
            Application.Run();
            return selection;
        }

        public void ShowPaymentMethod()
        {
            throw new NotImplementedException();
        }

        public void ShowUserCart()
        {
            throw new NotImplementedException();
        }

        private MenuBar GetMenuBar()
        {
            return new MenuBar(new MenuBarItem[]
            {
                new MenuBarItem("_Plik", new MenuItem[]
                {
                    new MenuItem("_Nowy", "", () => MessageBox.Query("Nowy", "Utworzono nowy plik.", "Ok")),
                    new MenuItem("_Otwórz", "", () => MessageBox.Query("Otwórz", "Otwieranie pliku...", "Ok")),
                    new MenuItem("_Zamknij", "", () => Application.RequestStop())
                }),
                new MenuBarItem("Konto", new MenuItem[]
                {
                    new MenuItem("Informacje o użytkowniku", "", () => MessageBox.Query("O programie", "To jest przykładowa aplikacja.", "Ok")),
                    new MenuItem("Wyloguj się!", "", () =>   MessageBox.Query("Wylogowywanie:", "Czy na pewno chcesz się wylogować?", "Ok"))

                })
            });
        }
    }
}
