using ConsoleApp.Data;
using ConsoleApp.Services;
using Library.Data;
using Library.Interfaces;
using Library.Models;
using NStack;
using System.ComponentModel;
using System.Data;
using System.Text;
using Terminal.Gui;

namespace ConsoleApp.Views
{
    public class BuyerCLIView : RoleCLIView, IBuyerView
    {
        public Task<Object?> ShowAllProducts(List<ProductModel> products, List<CartProductModel> productsFromCart)
        {
            InitializeWindow();
            Object? result = null;
            var win = new FrameView("Produkty")
            {
                X = 0,
                Y = 1,
                Width = Dim.Percent(70),
                Height = Dim.Fill(1),
                ColorScheme = ColorTheme.GrayThemePalette
            };

            if (products.Count == 0)
            {
                win.Width = Dim.Fill(1);
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
                win.KeyPress += (e) =>
                {
                    if (e.KeyEvent.Key == Key.Esc)
                    {
                        Application.RequestStop();
                    }
                };
                win.Add(nullLabel, exitNullButton);
                OpenFrameAndShutdown(win);
                return result;
            }

            var productNames = products.Select(p => new string[]
            {
                p.Id.ToString(),
                DescriptionLimiter(p.Name,ConstIntegers.MaxLengthOfName),
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
                Height = Dim.Percent(80),
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
                    Y = Pos.Bottom(priceToPayLabel) + 2
                };
                addToCartButton.Clicked += () =>
                {
                    if (int.TryParse(quantityQuestionTextField.Text.ToString(), out int quantity) &&
                        (quantity > 0 && quantity <= products[args.Item].Quantity))
                    {
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
                    X = Pos.Center(),
                    Y = Pos.Bottom(addToCartButton)
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
                windowDetails.SetFocus();
                quantityQuestionTextField.SetFocus();



            };
            var cartFrame = new FrameView("Podgląd koszyka")
            {
                X = Pos.Right(win),
                Y = Pos.Top(win),
                Width = Dim.Fill(1),
                Height = Dim.Fill(1),

                ColorScheme = ColorTheme.GrayThemePalette
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
                dt.Rows.Add(product.OriginalProduct.Id, DescriptionLimiter(product.OriginalProduct.Name, ConstIntegers.MaxLengthOfName), product.Quantity);
            }
            cartTable.Table = dt;
            cartFrame.Add(cartTable);
            win.Add(listView);
            var exitButton = new Button("Zamknij")
            {
                Y = Pos.Bottom(listView) + 1,
                X = Pos.Left(listView)
            };
            exitButton.Clicked += () => { Application.RequestStop(); };
            win.KeyPress += (e) =>
            {
                if (e.KeyEvent.Key == Key.Esc)
                {
                    Application.RequestStop();
                }
            };
            win.Add(exitButton);
            OpenFrameAndShutdown(win, cartFrame);
            return result;

        }

        public Task ShowInterface()
        {

        }

        public Task<int> ShowMenu()

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
            var showHistoryButton = new Button($"_{iter++}. Pokaż historie zakupów")
            {
                X = Pos.Center(),
                Y = 4
            };
            showHistoryButton.Clicked += () =>
            {
                selection = 4;
                Application.RequestStop();
            };
            var exitShopButton = new Button($"_{iter++}. Wyjdź z {ConstString.AppName}")
            {
                X = Pos.Center(),
                Y = 5
            };

            exitShopButton.Clicked += () =>
            {
                selection = 5;
                Application.RequestStop();
            };
            menuFrame.Add(showProductListButton, showCartButton, finalizeShoppingButton, showHistoryButton, exitShopButton);
            OpenFrameAndShutdown(menuFrame);
            return selection;
        }




        public Task<(CartActionEnum, CartProductModel?)> ShowUserCart(List<CartProductModel> cartProducts)
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

            if (cartProducts.Count == 0)
            {
                var nullHeaderLabel = new Label()
                {
                    X = Pos.Center(),
                    Y = Pos.Center(),
                };
                nullHeaderLabel.Text = "Twój koszyk jest pusty";
                frame.Add(nullHeaderLabel);
                var exitNullButton = new Button("Zamknij")
                {
                    X = Pos.Center(),
                    Y = Pos.Bottom(nullHeaderLabel) + 1
                };
                exitNullButton.Clicked += () =>
                {
                    Application.RequestStop();
                };
                frame.KeyPress += (e) =>
                {
                    if (e.KeyEvent.Key == Key.Esc)
                    {
                        Application.RequestStop();
                    }
                };

                frame.Add(exitNullButton);
                OpenFrameAndShutdown(frame);
                return result;
            }
            var productNames = cartProducts.Select(p => new string[]
        {
                p.OriginalProduct.Id.ToString(),
                DescriptionLimiter(p.OriginalProduct.Name,ConstIntegers.MaxLengthOfName),
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
                Height = Dim.Percent(80),
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
                    ColorScheme = ColorTheme.GrayThemePalette,

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
                var buttonContainer = new View()
                {
                    X = Pos.Center(),
                    Y = Pos.Bottom(priceToPayLabel) + 1,
                    Width = Dim.Sized(54 + 12),
                    Height = 1
                };
                var changeQuantityInCartButton = new Button("Zmień ilość")
                {
                    X = 0,
                    Y = 0,
                };
                changeQuantityInCartButton.Clicked += () =>
                {
                    if (int.TryParse(quantityQuestionTextField.Text.ToString(), out int quantity) && (quantity > 0 && quantity <= cartProducts[args.Item].OriginalProduct.Quantity))
                    {
                        var cartProduct = cartProducts[args.Item];
                        cartProduct.Quantity = quantity;
                        result = (CartActionEnum.Update, cartProduct);
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
                var removeProductFromCartButton = new Button("Usuń z koszyka")
                {
                    X = Pos.Right(rejectChangesButton),
                    Y = Pos.Top(rejectChangesButton)
                };
                removeProductFromCartButton.Clicked += () =>
                {
                    var answer = MessageBox.Query("Usuwanie produktu z koszyka", "Czy jesteś pewien, że chcesz usunąć produkt z koszyka?", "Tak", "Nie");
                    if (answer == 0)
                    {
                        result = (CartActionEnum.Remove, cartProducts[args.Item]);
                        Application.RequestStop();
                    }
                };
                TextFieldValidator.AllowOnlyIntegers(quantityQuestionTextField);
                quantityQuestionTextField.TextChanged += (_) =>
                {
                    if (int.TryParse(quantityQuestionTextField.Text.ToString(), out int quantity))
                    {
                        priceToPayLabel.Text = $"Cena do zapłaty: {quantity * cartProducts[args.Item].OriginalProduct.Price} {ConstString.Currency}";
                    }
                };
                buttonContainer.Add(changeQuantityInCartButton, rejectChangesButton, removeProductFromCartButton);
                windowDetails.Add(idLabel,
                    nameLabel,
                    descriptionLabel,
                    priceLabel,
                    quantityLabel,
                    quantityInCartLabel,
                    quantityQuestionLabel,
                    quantityQuestionTextField,
                    priceToPayLabel,
                    buttonContainer);
                frame.Add(windowDetails);
                windowDetails.SetFocus();
                quantityQuestionTextField.SetFocus();

            };
            frame.Add(listView);
            string summaryText = ((Func<string>)(() =>
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Łączna kwota: ");
                decimal sum = 0m;
                sum = cartProducts.Sum(p => p.Quantity * p.OriginalProduct.Price);
                sb.Append(sum.ToString());
                sb.Append(ConstString.Currency);
                return sb.ToString();
            }))();
            var summary = new Label(summaryText)
            {
                Y = Pos.Bottom(listView) + 1,
                X = Pos.Left(listView)
            };
            var finalizeShoppingButton = new Button("Zapłać za zakupy")
            {
                Y = Pos.Bottom(summary),
                X = Pos.Left(summary)
            };
            finalizeShoppingButton.Clicked += () =>
            {
                result = (CartActionEnum.Buy, null);
                Application.RequestStop();
            };
            var removeAllProductsFromCart = new Button("Wyczyść koszyk")
            {
                Y = Pos.Top(finalizeShoppingButton),
                X = Pos.Right(finalizeShoppingButton) + 1
            };
            removeAllProductsFromCart.Clicked += () =>
            {
                var answer = MessageBox.Query("Koszyk", "Czy chcesz usunąć wszystkie produkty z koszyka?", "Tak", "Nie");
                if (answer == 0)
                {
                    result = (CartActionEnum.RemoveAll, null);
                    Application.RequestStop();
                }
            };
            var exitButton = new Button("Zamknij")
            {
                Y = Pos.Top(removeAllProductsFromCart),
                X = Pos.Right(removeAllProductsFromCart) + 1
            };
            exitButton.Clicked += () =>
            {
                Application.RequestStop();
            };
            frame.KeyPress += (e) =>
            {
                if (e.KeyEvent.Key == Key.Esc)
                {
                    Application.RequestStop();
                }
            };

            frame.Add(summary);
            frame.Add(finalizeShoppingButton, removeAllProductsFromCart, exitButton);
            OpenFrameAndShutdown(frame);
            return result;
        }

        public Task<PaymentMethodEnum> ShowPaymentMethod(List<CartProductModel> productsFromCart)
        {
            PaymentMethodEnum paymentMethod = PaymentMethodEnum.Exit;
            InitializeWindow();
            if (productsFromCart.Count == 0)
            {
                var nullFrame = new FrameView("Koszyk")
                {
                    X = 0,
                    Y = 0,
                    Width = Dim.Fill(1),
                    Height = Dim.Fill(1),
                    ColorScheme = ColorTheme.GrayThemePalette
                };

                var nullLabel = new Label("Twój koszyk jest pusty")
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
                nullFrame.KeyPress += (e) =>
                {
                    if (e.KeyEvent.Key == Key.Esc)
                    {
                        Application.RequestStop();
                    }
                };
                nullFrame.Add(nullLabel, exitNullButton);
                OpenFrameAndShutdown(nullFrame);
                return paymentMethod;
            }
            var paymentMethodFrame = new FrameView("Dokonaj zakupu")
            {
                X = 0,
                Y = 1,
                Width = Dim.Percent(50),
                Height = Dim.Fill(1),
                ColorScheme = ColorTheme.GrayThemePalette
            };
            var paymentMethodLabel = new Label("Wybierz sposób płatności: ")
            {
                X = 1,
                Y = 1
            };
            ustring[] options =
            {
                ustring.Make("Gotówka"),
                ustring.Make("Kartą"),
                ustring.Make("Blikiem")
            };
            var radioPayementMethodButton = new RadioGroup()
            {
                X = Pos.Left(paymentMethodLabel),
                Y = Pos.Bottom(paymentMethodLabel),
                RadioLabels = options,
                SelectedItem = 0
            };
            var confirmPaymentMethodButton = new Button("Kup teraz")
            {
                X = Pos.Center(),
                Y = Pos.Bottom(radioPayementMethodButton) + 1
            };
            var continueShoppingButton = new Button("Kontynuuj zakupy")
            {
                X = Pos.Center(),
                Y = Pos.Bottom(confirmPaymentMethodButton)
            };

            confirmPaymentMethodButton.Clicked += () =>
            {

                var paymentMethodWindow = new Window("Potwierdzenie zakupu")
                {
                    X = Pos.Center(),
                    Y = Pos.Center(),
                    Width = 50,
                    Height = 9,
                    ColorScheme = ColorTheme.RedThemePalette
                };

                var isUserSureLabel = new Label("Czy na pewno chcesz dokonać zakupu?")
                {
                    X = Pos.Center(),
                    Y = 1
                };
                var yesButton = new Button("Tak")
                {
                    X = Pos.Center(),
                    Y = Pos.Bottom(isUserSureLabel) + 1
                };
                yesButton.Clicked += () =>
                {
                    paymentMethod = (PaymentMethodEnum)radioPayementMethodButton.SelectedItem;
                    Application.RequestStop();
                };
                var noButton = new Button("Nie")
                {
                    X = Pos.Center(),
                    Y = Pos.Bottom(yesButton)
                };
                noButton.Clicked += () =>
                {
                    paymentMethodFrame.Remove(paymentMethodWindow);
                };
                paymentMethodFrame.Add(paymentMethodWindow);
                paymentMethodWindow.Add(isUserSureLabel, yesButton, noButton);
                paymentMethodWindow.SetFocus();
            };
            continueShoppingButton.Clicked += () =>
            {
                paymentMethod = PaymentMethodEnum.None;
                Application.RequestStop();
            };
            paymentMethodFrame.KeyPress += (e) =>
            {
                if (e.KeyEvent.Key == Key.Esc)
                {
                    paymentMethod = PaymentMethodEnum.None;
                    Application.RequestStop();
                }
            };


            var summaryCartFrame = new FrameView("Zawartość koszyka")
            {
                X = Pos.Right(paymentMethodFrame),
                Y = Pos.Top(paymentMethodFrame),
                Width = Dim.Percent(50),
                Height = Dim.Fill(1),
                ColorScheme = ColorTheme.GrayThemePalette
            };

            var cartTable = new TableView()
            {
                X = Pos.Center(),
                Y = 1,
                Width = Dim.Fill(1),
                Height = Dim.Percent(80),
                CanFocus = false
            };
            var columnNames = new string[] { "Kod kreskowy", "Nazwa", "Ilość", "Cena", "Suma" };
            var dt = new DataTable();
            foreach (var columnName in columnNames)
            {
                dt.Columns.Add(columnName);
            }
            foreach (var product in productsFromCart)
            {
                dt.Rows.Add(product.OriginalProduct.Id, product.OriginalProduct.Name, product.Quantity, product.OriginalProduct.Price, product.OriginalProduct.Price * product.Quantity);
            }
            cartTable.Table = dt;

            string summaryText = ((Func<string>)(() =>
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Łączna kwota: ");
                decimal sum = 0m;
                sum = productsFromCart.Sum(p => p.Quantity * p.OriginalProduct.Price);
                sb.Append(sum.ToString());
                sb.Append(ConstString.Currency);
                return sb.ToString();
            }))();


            var priceToPayLabel = new Label(summaryText)
            {
                X = Pos.Center(),
                Y = Pos.Bottom(cartTable),
            };

            summaryCartFrame.Add(cartTable, priceToPayLabel);






            paymentMethodFrame.Add(paymentMethodLabel, radioPayementMethodButton, confirmPaymentMethodButton, continueShoppingButton);
            OpenFrameAndShutdown(paymentMethodFrame, summaryCartFrame);
            return paymentMethod;
        }

        public Task ShowShoppingHistory(List<ShoppingCartHistoryModel> shoppingCartHistories)
        {
            InitializeWindow();
            var historyFrame = new FrameView("Historia zakupów")
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(1),
                Height = Dim.Fill(1),
                ColorScheme = ColorTheme.GrayThemePalette
            };
            if (shoppingCartHistories.Count == 0)
            {
                var nullLabel = new Label("Brak historii zakupów w bazie")
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
                historyFrame.KeyPress += (e) =>
                {
                    if (e.KeyEvent.Key == Key.Esc)
                    {
                        Application.RequestStop();
                    }
                };
                historyFrame.Add(nullLabel, exitNullButton);
                OpenFrameAndShutdown(historyFrame);
                return;
            }

            var productNames = shoppingCartHistories.Select(p => new string[]
            {
                p.Date.ToString(),
                p.TotalPrice.ToString("0.00"),
                (p.PaymentMethod != null) ? GetEnumDescription((PaymentMethodEnum)p.PaymentMethod) : "",
                p.PurchasedProducts.Count.ToString()

            }).ToList();
            string[] header = { "Data", "Suma", "Płatność", "Ilość zakupionych produktów" };
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

            var label = new Label("Wpisy historii")
            {
                X = Pos.Center(),
                Y = 0
            };
            historyFrame.Add(label);

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
            historyFrame.Add(tableHeaderLabel);


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
                var windowDetails = new Window("Szczegóły wpisu")
                {
                    X = 0,
                    Y = 1,
                    Width = Dim.Fill(1),
                    Height = Dim.Fill(1),
                    ColorScheme = ColorTheme.GrayThemePalette
                };
                var dateLabel = new Label("Zakupiono: " + shoppingCartHistories[args.Item].Date.ToString())
                {
                    X = Pos.Center(),
                    Y = 1,
                };
                var totalPriceLabel = new Label("Za kwotę: " + shoppingCartHistories[args.Item].TotalPrice.ToString())
                {
                    X = Pos.Center(),
                    Y = Pos.Bottom(dateLabel)
                };
                PaymentMethodEnum? paymentEnum = shoppingCartHistories[args.Item].PaymentMethod;
                string paymentMethodString = (paymentEnum != null) ? GetEnumDescription((PaymentMethodEnum)paymentEnum) : "";
                var paymentMethodLabel = new Label("Metoda płatności: " + paymentMethodString)
                {
                    X = Pos.Center(),
                    Y = Pos.Bottom(totalPriceLabel)
                };
                var quantityOfProductsLabel = new Label("Ilość produktów: " + shoppingCartHistories[args.Item].PurchasedProducts.Count.ToString())
                {
                    X = Pos.Center(),
                    Y = Pos.Bottom(paymentMethodLabel)
                };
                var cartTable = new TableView()
                {
                    X = Pos.Center(),
                    Y = Pos.Bottom(quantityOfProductsLabel) + 2,
                    Width = Dim.Fill(1),
                    Height = Dim.Percent(60),
                    CanFocus = false
                };
                var columnNames = new string[] { "Kod kreskowy", "Nazwa", "Opis", "Ilość", "Cena", "Suma" };
                var dt = new DataTable();
                foreach (var columnName in columnNames)
                {
                    dt.Columns.Add(columnName);
                }
                foreach (var product in shoppingCartHistories[args.Item].PurchasedProducts)
                {
                    dt.Rows.Add(product.ProductId, DescriptionLimiter(product.Name, ConstIntegers.MaxLengthOfName), DescriptionLimiter(product.Description), product.Quantity, product.Price, product.Price * product.Quantity);
                }
                cartTable.Table = dt;
                var closeDetailsButton = new Button("Zamknij szczegóły")
                {
                    X = Pos.Center(),
                    Y = Pos.Bottom(cartTable),
                };
                closeDetailsButton.Clicked += () =>
                {
                    historyFrame.Remove(windowDetails);
                };
                windowDetails.Add(dateLabel, totalPriceLabel, paymentMethodLabel, quantityOfProductsLabel, cartTable, closeDetailsButton);
                historyFrame.Add(windowDetails);

            };
            var closeButton = new Button("Zamknij historie zakupów")
            {
                X = Pos.Left(listView),
                Y = Pos.Bottom(listView) + 1
            };
            closeButton.Clicked += () =>
            {
                Application.RequestStop();
            };
            historyFrame.KeyPress += (e) =>
            {
                if (e.KeyEvent.Key == Key.Esc)
                {
                    Application.RequestStop();
                }
            };
            historyFrame.Add(listView, closeButton);
            OpenFrameAndShutdown(historyFrame);
        }

        private string GetEnumDescription(PaymentMethodEnum value)
        {
            var field = value.GetType().GetField(value.ToString());
            if (field == null)
                return value.ToString();
            var attribute = System.Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}
