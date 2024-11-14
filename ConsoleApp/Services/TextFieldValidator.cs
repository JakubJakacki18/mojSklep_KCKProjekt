using System.Globalization;
using Terminal.Gui;

namespace ConsoleApp.Services
{
    public class TextFieldValidator
    {
        public static void AllowOnlyIntegers(TextField textField)
        {
            var decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            textField.TextChanged += (args) =>
           {
               int originalCursorPosition = textField.CursorPosition;
               string text = textField.Text.ToString() ?? string.Empty;
               bool hasLeadingZero = text.StartsWith("0") && text.Length > 1;
               if (!int.TryParse(text, out _) && text != "" || hasLeadingZero)
               {
                   textField.Text = text.Substring(0, text.Length - 1);
                   textField.CursorPosition = Math.Max(0, originalCursorPosition - 1);
               }
           };
        }
        public static void AllowOnlyDoubles(TextField textField)
        {
            var decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            textField.TextChanged += (args) =>
            {
                string text = textField.Text.ToString() ?? string.Empty;
                int originalCursorPosition = textField.CursorPosition;
                if (string.IsNullOrEmpty(text))
                    return;
                bool isValidDouble = double.TryParse(text, NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out _);
                bool startsWithInvalidChar = text.StartsWith(decimalSeparator) || text.StartsWith("-");
                int decimalIndex = text.IndexOf(decimalSeparator);
                bool hasMoreThanTwoDecimals = decimalIndex >= 0 && text.Length - decimalIndex - 1 > 2;
                bool hasLeadingZero = text.StartsWith("0") && text.Length > 1 && text[1].ToString() != decimalSeparator;



                if (!isValidDouble || startsWithInvalidChar || hasMoreThanTwoDecimals || hasLeadingZero)
                {
                    textField.Text = text.Substring(0, text.Length - 1);
                    textField.CursorPosition = Math.Max(0, originalCursorPosition - 1);
                }
            };
        }
    }
}
