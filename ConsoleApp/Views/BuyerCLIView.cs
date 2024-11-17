using ConsoleApp.Data;
using ConsoleApp.Services;
using Library.Data;
using Library.Interfaces;
using Library.Models;
using System.Data;
using System.Diagnostics;
using System.Text;
using Terminal.Gui;

namespace ConsoleApp.Views
{
    public class BuyerCLIView : RoleCLIView, IBuyerView
    {
        public Object? ShowAllProducts(List<ProductModel> products, List<CartProductModel> productsFromCart)
        {
            InitializeWindow();
			Object? result = null;
            var win = new FrameView("Produkty")
            {
                X = 0,
                Y = 1,
                Width=Dim.Percent(70),
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
                exitNullButton.Clicked += () => { Application.RequestStop(); };
                win.Add(nullLabel, exitNullButton);
				OpenFrameAndShutdown(win);
				return result;
            }

            var productNames = products.Select(p => new string[]
            {
                p.Id.ToString(),
                p.Name,
                DescriptionLimiter(p.Description),
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
                    if (int.TryParse(quantityQuestionTextField.Text.ToString(), out int quantity) &&
                        (quantity > 0 && quantity <= products[args.Item].Quantity))
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
                rejectAddingToCartButton.Clicked += () => { win.Remove(windowDetails); };

                TextFieldValidator.AllowOnlyIntegers(quantityQuestionTextField);
                quantityQuestionTextField.TextChanged += (_) =>
                {
                    if (int.TryParse(quantityQuestionTextField.Text.ToString(), out int quantity))
                    {
                        priceToPayLabel.Text =
                            $"Cena do zapłaty: {quantity * products[args.Item].Price} {ConstString.Currency}";

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
			var cartFrame = new FrameView("Podgląd koszyka")
			{
				X = Pos.Right(win),
				Y = Pos.Top(win),
				Width = Dim.Fill(1),
				Height = Dim.Fill(1)
			};

			var cartTable = new TableView()
			{
				X = 0,
				Y = 0,
				Width = Dim.Fill(1),
				Height = Dim.Fill(1),
				CanFocus = false
			};
			var columnNames = new string[] { "Kod kreskowy", "Nazwa", "Ilość" };
			var dt = new DataTable();
			foreach (var columnName in columnNames)
			{
				dt.Columns.Add(columnName);
			}
			foreach (var product in productsFromCart)
			{
				dt.Rows.Add(product.OriginalProduct.Id, product.OriginalProduct.Name, product.Quantity);
			}
			cartTable.Table = dt;
			cartFrame.Add(cartTable);
			win.Add(listView);
            var exitButton = new Button("Zamknij")
            {
                Y = Pos.Top(listView),
                X = Pos.Right(listView) + 1
            };
            exitButton.Clicked += () => { Application.RequestStop(); };
            win.Add(exitButton);
            OpenFrameAndShutdown(win,cartFrame);
            return result;

        }

        public void ShowInterface()
        {

        }

        public int ShowMenu()
        
        {
            InitializeWindow();
            // Stworzenie okna dialogowego, które będzie pełniło rolę menu
            var menuFrame = new FrameView("Menu")
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
            menuFrame.Add(showProductListButton, showCartButton, finalizeShoppingButton, exitShopButton);
            mainWindow.Add(GetMenuBar());
            OpenFrameAndShutdown(menuFrame);
            return selection;
        }

        public void ShowPaymentMethod()
        {
            throw new NotImplementedException();
        }


        public (CartActionEnum, CartProductModel?) ShowUserCart(List<CartProductModel> cartProducts)
        {
            InitializeWindow();
			(CartActionEnum, CartProductModel?) result = (CartActionEnum.Exit, null);
            var frame = new FrameView("Koszyk")
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(1),
                Height = Dim.Fill(1),
                ColorScheme = ColorTheme.GrayThemePalette
            };
            var headerLabel = new Label()
            {
                X = Pos.Center(),
                Y = 1
            };
            if (cartProducts.Count == 0)
            {
                headerLabel.Text = "Twój koszyk jest pusty";
                frame.Add(headerLabel);
                var exitNullButton = new Button("Zamknij")
                {
                    X = Pos.Center(),
                    Y = Pos.Bottom(headerLabel) + 1
                };
                exitNullButton.Clicked += () =>
                {
                    Application.RequestStop();
                };
                frame.Add(exitNullButton);
                OpenFrameAndShutdown(frame);
                return result;
            }
            var productNames = cartProducts.Select(p => new string[]
        {
                p.OriginalProduct.Id.ToString(),
                p.OriginalProduct.Name,
                /*p.OriginalProduct.Description?.Substring(0,Math.Min(p.OriginalProduct.Description.Length,20)) ?? "",*/
                DescriptionLimiter(p.OriginalProduct.Description),
                p.OriginalProduct.Price.ToString("0.00"),
                p.Quantity.ToString(),
                (p.Quantity * p.OriginalProduct.Price).ToString("0.00")
        }).ToList();
            string[] header = { "Kod kreskowy", "Nazwa", "Opis", "Cena", "Ilość w koszyku", "Łącznie" };
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
            frame.Add(label);

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
            frame.Add(tableHeaderLabel);


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
                var idLabel = new Label("Kod kreskowy: " + cartProducts[args.Item].OriginalProduct.Id)
                {
                    X = Pos.Center(),
                    Y = 1
                };
                var nameLabel = new Label("Nazwa: " + cartProducts[args.Item].OriginalProduct.Name)
                {
                    X = Pos.Center(),
                    Y = Pos.Bottom(idLabel)
                };
                var descriptionLabel = new Label("Opis: " + cartProducts[args.Item].OriginalProduct.Description)
                {
                    X = Pos.Center(),
                    Y = Pos.Bottom(nameLabel)
                };
                var priceLabel = new Label("Cena: " + cartProducts[args.Item].OriginalProduct.Price)
                {
                    X = Pos.Center(),
                    Y = Pos.Bottom(descriptionLabel)
                };
                var quantityLabel = new Label("Ilość dostęnych sztuk: " + cartProducts[args.Item].OriginalProduct.Quantity)
                {
                    X = Pos.Center(),
                    Y = Pos.Bottom(priceLabel)
                };
                var quantityInCartLabel = new Label("Ilość w koszyku: " + cartProducts[args.Item].Quantity)
                {
                    X = Pos.Center(),
                    Y = Pos.Bottom(quantityLabel)
                };
                var quantityQuestionLabel = new Label("Ile chcesz kupić naszego produktu?")
                {
                    X = Pos.Center(),
                    Y = Pos.Bottom(quantityInCartLabel) + 2
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
                var changeQuantityInCartButton = new Button("Zmień ilość")
                {
                    X = Pos.Center(),
                    Y = Pos.Bottom(priceToPayLabel) + 1
                };
                changeQuantityInCartButton.Clicked += () =>
                {
                    if (int.TryParse(quantityQuestionTextField.Text.ToString(), out int quantity) && (quantity > 0 && quantity <= cartProducts[args.Item].OriginalProduct.Quantity))
                    {
                        //MessageBox.Query("Dodano do koszyka", "Dodano produkt do koszyka", "Ok");
                        Application.RequestStop();
                    }
                    else
                    {
                        MessageBox.ErrorQuery("Błąd", "Niepoprawna ilość produktów, popraw wartość", "Ok");
                    }
                };
                var rejectChangesButton = new Button("Zrezygnuj z zmieniania ilości")
                {
                    X = Pos.Right(changeQuantityInCartButton),
                    Y = Pos.Top(changeQuantityInCartButton)
                };
                rejectChangesButton.Clicked += () =>
                {
                    frame.Remove(windowDetails);
                };

                TextFieldValidator.AllowOnlyIntegers(quantityQuestionTextField);
                quantityQuestionTextField.TextChanged += (_) =>
                {
                    if (int.TryParse(quantityQuestionTextField.Text.ToString(), out int quantity))
                    {
                        priceToPayLabel.Text = $"Cena do zapłaty: {quantity * cartProducts[args.Item].OriginalProduct.Price} {ConstString.Currency}";

                    }
                };

                windowDetails.Add(idLabel,
                    nameLabel,
                    descriptionLabel,
                    priceLabel,
                    quantityLabel,
                    quantityInCartLabel,
                    quantityQuestionLabel,
                    quantityQuestionTextField,
                    priceToPayLabel,
                    changeQuantityInCartButton,
                    rejectChangesButton);
                frame.Add(windowDetails);



            };
            frame.Add(listView);
            var exitButton = new Button("Zamknij")
            {
                Y = Pos.Top(listView),
                X = Pos.Right(listView) + 1
            };
            exitButton.Clicked += () =>
            {
                Application.RequestStop();
            };
            frame.Add(exitButton);
            OpenFrameAndShutdown(frame);
            return result;

        }

        private string DescriptionLimiter(string? description)
        {
            //p.OriginalProduct.Description?.Substring(0, Math.Min(p.OriginalProduct.Description.Length, 20)) ?? "";
            if (description == null)
            {
                return "";
            }
            if (description.Length > ConstIntegers.MaxLengthOfDescription)
            {

                return $"{description.Substring(0, ConstIntegers.MaxLengthOfDescription - 3)}...";
            }
            return description;
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
                    new MenuItem("Informacje o użytkowniku", "",
                        () => MessageBox.Query("O programie", "To jest przykładowa aplikacja.", "Ok")),
                    new MenuItem("Wyloguj się!", "",
                        () => MessageBox.Query("Wylogowywanie:", "Czy na pewno chcesz się wylogować?", "Ok"))

                })
            });
        }
    }
}
