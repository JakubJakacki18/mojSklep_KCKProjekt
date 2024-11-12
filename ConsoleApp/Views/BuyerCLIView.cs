using ConsoleApp.Data;
using Library.Data;
using Library.Interfaces;
using Library.Models;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Terminal.Gui;

namespace ConsoleApp.Views
{
	public class BuyerCLIView : RoleCLIView, IBuyerView
	{
		public void ShowAllProducts(List<ProductModel> products)
		{
			var win = new Window("Produkty")
			{
				X = 0,
				Y = 1,
				Width = Dim.Fill(1),
				Height = Dim.Fill(1),
				ColorScheme = ColorTheme.GrayThemePalette
			};



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
				columnWidths[col] = Math.Max(columnWidths[col],productNames.Max(row => row[col].Length));
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
			
			var Label = new Label("Lista produktów")
			{
				X = Pos.Center(),
				Y = 0
			};
			win.Add(Label);
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
				MessageBox.Query("Szczegóły produktu", products[args.Item].Description, "Ok");
			};
			var label = new Label("Lista produktów")
			{
				X = Pos.Center(),
				Y = 0
			};
			win.Add(listView);
			OpenWindowAndShutdown(win);

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
