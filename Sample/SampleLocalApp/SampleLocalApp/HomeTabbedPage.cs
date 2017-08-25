using System;
using Messier16.Forms.Controls;
using Xamarin.Forms;

namespace SampleLocalApp
{
    public class HomeTabbedPage : PlatformTabbedPage
    {
        public HomeTabbedPage()
        {
            BarBackgroundColor = App.BarBackgroundColors[3];
            SelectedColor = App.SelectedColors[0];
            BarBackgroundApplyTo = BarBackgroundApplyTo.Android;

            var page = new ConfigurationPage() { Icon = "config" };

            Children.Add(new BasicContentPage("Home") { Icon = "home" });
            Children.Add(new BasicContentPage("Messages"){ Icon = "message" });
            Children.Add(new BasicContentPage("Trending"){ Icon = "hashtag" });


            Children.Add(page);
        }
    }
}