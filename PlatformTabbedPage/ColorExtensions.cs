using Xamarin.Forms;

namespace Messier16.Forms.Controls
{
    public static class ColorExtensions
    {
        public static Color Darken(this Color color)
        {
            return color.WithLuminosity(color.Luminosity * 0.7);
        }
    }
}
