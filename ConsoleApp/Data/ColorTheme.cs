using Terminal.Gui;

namespace ConsoleApp.Data
{
    public class ColorTheme
    {
        public static readonly ColorScheme GrayThemePalette = new ColorScheme
        {
            Normal = Application.Driver.MakeAttribute(Color.Black, Color.Gray),

            Focus = Application.Driver.MakeAttribute(Color.Black, Color.Cyan),
            HotNormal = Application.Driver.MakeAttribute(Color.Blue, Color.Gray),
            HotFocus = Application.Driver.MakeAttribute(Color.Black, Color.Cyan)
        };
        public static readonly ColorScheme BlueThemePalette = new ColorScheme
        {
            Normal = Application.Driver.MakeAttribute(Color.Black, Color.Blue),

            Focus = Application.Driver.MakeAttribute(Color.Black, Color.Cyan),
            HotNormal = Application.Driver.MakeAttribute(Color.Blue, Color.Blue),
            HotFocus = Application.Driver.MakeAttribute(Color.Black, Color.Cyan)
        };
        public static readonly ColorScheme GreenThemePalette = new ColorScheme
        {
            Normal = Application.Driver.MakeAttribute(Color.Black, Color.Green),

            Focus = Application.Driver.MakeAttribute(Color.Black, Color.Cyan),
            HotNormal = Application.Driver.MakeAttribute(Color.Blue, Color.Green),
            HotFocus = Application.Driver.MakeAttribute(Color.Black, Color.Cyan)
        };
        public static readonly ColorScheme RedThemePalette = new ColorScheme
        {
            Normal = Application.Driver.MakeAttribute(Color.White, Color.Red),

            Focus = Application.Driver.MakeAttribute(Color.Black, Color.Gray),
            HotNormal = Application.Driver.MakeAttribute(Color.Brown, Color.Red),
            HotFocus = Application.Driver.MakeAttribute(Color.BrightYellow, Color.Brown)
        };
    }
}
