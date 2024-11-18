using ConsoleApp.Data;
using ConsoleApp.Services;
using Library.Data;
using Library.Interfaces;
using Library.Models;
using System.Text;
using Terminal.Gui;


namespace ConsoleApp.Views
{
    public class SellerCLIView : RoleCLIView, ISellerView
    {

        public int ShowMenu()
        {
            InitializeWindow();
            var menuWindow = new FrameView("Menu")
            {
                X = Pos.Center(),
                Y = Pos.Center(),
                Width = 40,
                Height = 10,
                ColorScheme = ColorTheme.GrayThemePalette,
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
            var showAllProducts = new Button($"_{iter++}. Przejrzyj i edytuj produkty")
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
            OpenFrameAndShutdown(menuWindow);
			return selection;
        }


        public ProductModel? AddProduct()
        {
            InitializeWindow();
            ProductModel? product = null;
            var win = new FrameView("Dodaj produkt")
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(1),
                Height = Dim.Fill(1),
                ColorScheme = ColorTheme.GrayThemePalette
            };
            var productNameLabel = new Label("Nazwa produktu")
            {
                X = 1,
                Y = 1
            };
            var productName = new TextField()
            {
                X = Pos.Left(productNameLabel),
                Y = Pos.Bottom(productNameLabel),
                Width = 30
            };
            var productDescriptionLabel = new Label("Opis produktu")
            {
                X = Pos.Left(productNameLabel),
                Y = Pos.Bottom(productName)
            };
            var productDescription = new TextView()
            {
                X = Pos.Left(productNameLabel),
                Y = Pos.Bottom(productDescriptionLabel),
                Width = 40,
                Height = 3
            };
            var productPriceLabel = new Label("Cena produktu")
            {
                X = Pos.Left(productNameLabel),
                Y = Pos.Bottom(productDescription),
            };
            var productPrice = new TextField()
            {
                X = Pos.Left(productNameLabel),
                Y = Pos.Bottom(productPriceLabel),
                Width = 30
            };
            var productQuantityLabel = new Label("Ilość produktu")
            {
                X = Pos.Left(productNameLabel),
                Y = Pos.Bottom(productPrice),
            };
            var productQuantity = new TextField()
            {
                X = Pos.Left(productNameLabel),
                Y = Pos.Bottom(productQuantityLabel),
                Width = 30
            };
            var productShelfRowLabel = new Label("Umieszczenie w magazynie - Rząd")
            {
                X = Pos.Left(productNameLabel),
                Y = Pos.Bottom(productQuantity),
            };
            var productShelfRow = new TextField()
            {
                X = Pos.Left(productNameLabel),
                Y = Pos.Bottom(productShelfRowLabel),
                Width = 30
            };
            var productShelfColumnLabel = new Label("Umieszczenie w magazynie - Kolumna")
            {
                X = Pos.Left(productNameLabel),
                Y = Pos.Bottom(productShelfRow),
            };
            var productShelfColumn = new TextField()
            {
                X = Pos.Left(productNameLabel),
                Y = Pos.Bottom(productShelfColumnLabel),
                Width = 30
            };

            var addProductButton = new Button("Dodaj produkt")
            {
                X = Pos.Left(productNameLabel),
                Y = Pos.Bottom(productShelfColumn) + 1
            };
            var rejectProductButton = new Button("Zaniechaj dodawanie produktu")
            {
                X = Pos.Right(addProductButton) + 1,
                Y = Pos.Top(addProductButton)
            };
            addProductButton.Clicked += () =>
            {
                bool success = false;
                try
                {
                    product = new ProductModel
                    {
                        Name = productName.Text.ToString(),
                        Price = decimal.Parse(productPrice.Text.ToString()),
                        Description = productDescription.Text.ToString(),
                        Quantity = int.Parse(productQuantity.Text.ToString()),
                        shelfRow = int.Parse(productShelfRow.Text.ToString()),
                        shelfColumn = int.Parse(productShelfColumn.Text.ToString())
                    };
                    success = true;
                }
                catch
                {
                    MessageBox.ErrorQuery("Błąd", "Niepoprawne dane", "Ok");
                }
                if (success)
                    Application.RequestStop();

            };
            rejectProductButton.Clicked += () =>
            {
                Application.RequestStop();
            };

            TextFieldValidator.AllowOnlyDoubles(productPrice);
            TextFieldValidator.AllowOnlyIntegers(productQuantity);
            TextFieldValidator.AllowOnlyIntegers(productShelfRow);
            TextFieldValidator.AllowOnlyIntegers(productShelfColumn);
            //win.Add(productNameLabel, productDescriptionLabel, productPriceLabel, productQuantityLabel,productShelfColumnLabel,productShelfRowLabel);
            //win.Add(productName, productPrice, productDescription, productQuantity, productShelfRow, productShelfColumn, addProductButton, rejectProductButton);
            win.Add(
                productNameLabel,
                productName,
                productDescriptionLabel,
                productDescription,
                productPriceLabel,
                productPrice,
                productQuantityLabel,
                productQuantity,
                productShelfRowLabel,
                productShelfRow,
                productShelfColumnLabel,
                productShelfColumn,
                addProductButton,
                rejectProductButton
                );
            OpenFrameAndShutdown(win);
            return product;
        }

        public void EditProduct()
        {
            throw new NotImplementedException();
        }

        public (ShowProductsSellerActionEnum,ProductModel?) ShowAllProductsAndEdit(List<ProductModel> products)
        {
            (ShowProductsSellerActionEnum, ProductModel?) result = (ShowProductsSellerActionEnum.exit,null);

			InitializeWindow();

			if (products.Count == 0)
			{
				var nullFrame = new FrameView("Lista produktów")
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
				nullFrame.Add(nullLabel, exitNullButton);
				OpenFrameAndShutdown(nullFrame);
				return result;
			}

			var win = new FrameView("Lista produktów")
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(1),
                Height = Dim.Fill(1),
                ColorScheme = ColorTheme.GrayThemePalette
            };

            var productNames = products.Select(p => new string[]
            {
                p.Id.ToString(),
                p.Name,
                p.Description ?? "",
                p.Price.ToString("0.00"),
                p.Quantity.ToString(),
                p.shelfRow.ToString(),
                p.shelfColumn.ToString()
            }).ToList();
            string[] header = { "Kod kreskowy", "Nazwa", "Opis", "Cena", "Ilość", "Rząd", "Kolumna" };
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
                var editProductWindow = new Window()
                {
                    X = 0,
                    Y = 0,
                    Width = Dim.Fill(1),
                    Height = Dim.Fill(1),
                    ColorScheme = ColorTheme.GrayThemePalette
				}; 
                var editProductLabel = new Label("Edytuj produkt")
				{
					X = Pos.Center(),
					Y = 0
				};
				var product = products[args.Item];
				var productNameLabel = new Label("Nazwa produktu")
				{
					X = 1,
					Y = 1
				};
				var productName = new TextField(product.Name)
				{
					X = Pos.Left(productNameLabel),
					Y = Pos.Bottom(productNameLabel),
					Width = 30
				};
				var productDescriptionLabel = new Label("Opis produktu")
				{
					X = Pos.Left(productNameLabel),
					Y = Pos.Bottom(productName)
				};
				var productDescription = new TextView()
				{
					X = Pos.Left(productNameLabel),
					Y = Pos.Bottom(productDescriptionLabel),
					Width = 40,
					Height = 3,
                    Text= product.Description
				};
				var productPriceLabel = new Label("Cena produktu")
				{
					X = Pos.Left(productNameLabel),
					Y = Pos.Bottom(productDescription),
				};
				var productPrice = new TextField(product.Price.ToString())
				{
					X = Pos.Left(productNameLabel),
					Y = Pos.Bottom(productPriceLabel),
					Width = 30
				};
				var productQuantityLabel = new Label("Ilość produktu")
				{
					X = Pos.Left(productNameLabel),
					Y = Pos.Bottom(productPrice),
				};
				var productQuantity = new TextField(product.Quantity.ToString())
				{
					X = Pos.Left(productNameLabel),
					Y = Pos.Bottom(productQuantityLabel),
					Width = 30
				};
				var productShelfRowLabel = new Label("Umieszczenie w magazynie - Rząd")
				{
					X = Pos.Left(productNameLabel),
					Y = Pos.Bottom(productQuantity),
				};
				var productShelfRow = new TextField(product.shelfRow.ToString())
				{
					X = Pos.Left(productNameLabel),
					Y = Pos.Bottom(productShelfRowLabel),
					Width = 30
				};
				var productShelfColumnLabel = new Label("Umieszczenie w magazynie - Kolumna")
				{
					X = Pos.Left(productNameLabel),
					Y = Pos.Bottom(productShelfRow),
				};
				var productShelfColumn = new TextField(product.shelfColumn.ToString())
				{
					X = Pos.Left(productNameLabel),
					Y = Pos.Bottom(productShelfColumnLabel),
					Width = 30
				};

                var saveProductButton = new Button("Zapisz zmian")
                {
                    X = Pos.Left(productNameLabel),
				    Y = Pos.Bottom(productShelfColumn) + 1

				};
                saveProductButton.Clicked += () => 
                {
					product.Name = productName.Text.ToString();
					product.Price = decimal.Parse(productPrice.Text.ToString());
					product.Description = productDescription.Text.ToString();
					product.Quantity = int.Parse(productQuantity.Text.ToString());
					product.shelfRow = int.Parse(productShelfRow.Text.ToString());
					product.shelfColumn = int.Parse(productShelfColumn.Text.ToString());

					result = (ShowProductsSellerActionEnum.update, product);
					Application.RequestStop();
				};
                var rejectChangesButton = new Button("Odrzuć zmiany") 
                {
					X= Pos.Right(saveProductButton) + 1,
					Y = Pos.Top(saveProductButton)
				};
                rejectChangesButton.Clicked += () =>
				{
                    win.Remove(editProductWindow);
				};
                var removeProductButton = new Button("Usuń produkt")
				{
					X = Pos.Right(rejectChangesButton) + 1,
					Y = Pos.Top(rejectChangesButton)
				};
                removeProductButton.Clicked += () =>
				{
					var paymentMethodWindow = new Window("Potwierdzenie usunięcia")
					{
						X = Pos.Center(),
						Y = Pos.Center(),
						Width = 50,
						Height = 9,
						ColorScheme = ColorTheme.RedThemePalette
					};

					var isUserSureLabel = new Label("Czy na pewno chcesz usunąć produkt?")
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
						result = (ShowProductsSellerActionEnum.delete, product);
						Application.RequestStop();
					};
					var noButton = new Button("Nie")
					{
						X = Pos.Center(),
						Y = Pos.Bottom(yesButton)
					};
					noButton.Clicked += () =>
					{
						editProductWindow.Remove(paymentMethodWindow);
					};
					editProductWindow.Add(paymentMethodWindow);
					paymentMethodWindow.Add(isUserSureLabel, yesButton, noButton);
				};

				TextFieldValidator.AllowOnlyDoubles(productPrice);
				TextFieldValidator.AllowOnlyIntegers(productQuantity);
				TextFieldValidator.AllowOnlyIntegers(productShelfRow);
				TextFieldValidator.AllowOnlyIntegers(productShelfColumn);



				editProductWindow.Add(editProductLabel, 
                    productNameLabel, 
                    productName, 
                    productDescriptionLabel, 
                    productDescription, 
                    productPriceLabel, 
                    productPrice, 
                    productQuantityLabel,
                    productQuantity, 
                    productShelfRowLabel,
					productShelfRow,
					productShelfColumnLabel,
					productShelfColumn,
					saveProductButton,
					rejectChangesButton,
                    removeProductButton

					);
				win.Add(editProductWindow);

            };
            var closeButton = new Button("Zamknij")
            {
               X=Pos.Right(listView)+1,     
               Y=Pos.Top(listView)+1,
            };
            closeButton.Clicked += () =>
			{
				Application.RequestStop();
			};
			win.Add(listView,closeButton);
            OpenFrameAndShutdown(win);
            return result;
        }
    }
}
