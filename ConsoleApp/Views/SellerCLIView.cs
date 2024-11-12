﻿using ConsoleApp.Data;
using ConsoleApp.Services;
using Library.Data;
using Library.Interfaces;
using Library.Models;
using Terminal.Gui;
using static Terminal.Gui.View;


namespace ConsoleApp.Views
{
    public class SellerCLIView : RoleCLIView, ISellerView
	{

		public int ShowMenu()
		{
			Application.Init();
			top = Application.Top;
			mainWindow = new Window("Sklep internetowy - " + ConstString.AppName)
			{
				X = 0, // Położenie okna w poziomie
				Y = 1, // Położenie okna w pionie
				Width = Dim.Fill(), // Rozciągnięcie okna na całą szerokość
				Height = Dim.Fill(), // Rozciągnięcie okna na całą wysokość
				CanFocus = false
			};

			// Stworzenie okna dialogowego, które będzie pełniło rolę menu
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


		public ProductModel? AddProduct()
		{
			ProductModel? product = null;
			var win = new Window("Dodaj produkt")
			{
				X = 0,
				Y = 1,
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
						Price = double.Parse(productPrice.Text.ToString()),
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
			OpenWindow(win);
			return product;
		}

		public void EditProduct()
		{
			throw new NotImplementedException();
		}
	}
}
