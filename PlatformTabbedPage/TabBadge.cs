using Xamarin.Forms;

namespace Messier16.Forms.Controls
{
    public class TabBadge
    {
        public static BindableProperty BadgeTextProperty = 
            BindableProperty.CreateAttached("BadgeText", typeof(string), typeof(TabBadge), default(string));

        public static string GetBadgeText(BindableObject view)
        {
            return (string)view.GetValue(BadgeTextProperty);
        }

        public static void SetBadgeText(BindableObject view, string value)
        {
            view.SetValue(BadgeTextProperty, value);
        }


        public static BindableProperty BadgeColorProperty = 
            BindableProperty.CreateAttached("BadgeColor", typeof(Color), typeof(TabBadge), Color.Default);

        public static Color GetBadgeColor(BindableObject view)
        {
            return (Color)view.GetValue(BadgeColorProperty);
        }

        public static void SetBadgeColor(BindableObject view, Color value)
        {
            view.SetValue(BadgeColorProperty, value);
        }

        public static BindableProperty BadgeTextColorProperty = 
            BindableProperty.CreateAttached("BadgeTextColor", typeof(Color), typeof(TabBadge), Color.Default);

        public static Color GetBadgeTextColor(BindableObject view)
        {
            return (Color)view.GetValue(BadgeTextColorProperty);
        }

        public static void SetBadgeTextColor(BindableObject view, Color value)
        {
            view.SetValue(BadgeTextColorProperty, value);
        }

        public static BindableProperty BadgeFontProperty = 
            BindableProperty.CreateAttached("BadgeFont", typeof(Font), typeof(TabBadge), Font.Default);

        public static Font GetBadgeFont(BindableObject view)
        {
            return (Font)view.GetValue(BadgeFontProperty);
        }

        public static void SetBadgeFont(BindableObject view, Font value)
        {
            view.SetValue(BadgeFontProperty, value);
        }
    }
}
