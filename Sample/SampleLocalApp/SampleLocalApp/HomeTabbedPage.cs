using System;
using Messier16.Forms.Controls;
using Xamarin.Forms;

namespace SampleLocalApp
{
    public class HomeTabbedPage : PlatformTabbedPage
    {
        public HomeTabbedPage()
        {
            BarBackgroundColor = App.BackgroundColors[3];
            SelectedColor = App.HighlightColors[0];
            BarBackgroundApplyTo = BarBackgroundApplyTo.Android;

            var page = new ConfigurationPage() { Icon = "config", Title="Config" };
            Children.Add(new BasicContentPage("Home") { Icon = "home" });
            Children.Add(new BasicContentPage("Messages") { Icon = "message" });
            Children.Add(new BasicContentPage("Trending") { Icon = "hashtag" });
            Children.Add(page);
            
        }
    }
}