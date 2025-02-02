using System.Diagnostics;

class InterfaceSelector
{
	static void Main(string[] args)
	{
		string appPath = "";
		string? line;
		do
		{
			System.Console.WriteLine("0 - console \n1 - wpf");
			line = Console.ReadLine();
			switch (line)
			{
				case "0":
					appPath = @"C:\Users\Jakub\source\repos\KCKProjekt\ConsoleApp\bin\Debug\net8.0\ConsoleApp.exe";
					break;
				case "1":
					appPath = @"C:\Users\Jakub\source\repos\KCKProjekt\WpfApp\bin\Debug\net8.0-windows8.0\WpfApp.exe";
					break;
				default:
					System.Console.WriteLine("Bład, wpisz jeszcze raz");
					break;

			}
		} while (line != "0" && line != "1");
		Process process = new();
		process.StartInfo.FileName = appPath;
		process.Start();
	}
}