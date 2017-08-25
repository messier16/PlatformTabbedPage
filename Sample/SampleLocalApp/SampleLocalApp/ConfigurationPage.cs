using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Messier16.Forms.Controls;
using Xamarin.Forms;

namespace SampleLocalApp
{
    public class ConfigurationPage : ContentPage
    {
        Random r = new Random();

        private Dictionary<string, BarBackgroundApplyTo> ApplyTo = new Dictionary<string, BarBackgroundApplyTo>
        {
            {"None", BarBackgroundApplyTo.None},
            {"Android", BarBackgroundApplyTo.Android},
            {"iOS", BarBackgroundApplyTo.iOS},
            {"Both", BarBackgroundApplyTo.iOS |BarBackgroundApplyTo.Android }
        };

        public ConfigurationPage()
        {
            int tabColorIndex = 0;
            int tabBackgroundInde = 0;
            int badgeColor = App.HighlightColors.Length - 1;

            Title = "Config";
            var randomSelectedColor = new Button()
            {
                Text = "Next highlight color"
            };
            randomSelectedColor.Clicked += (sender, args) =>
            {
                var ran = tabColorIndex++ % App.HighlightColors.Length;
                App.HomeTabbedPage.SelectedColor = App.HighlightColors[ran];
                UpdateSelectedColors();
            };

            var randomBarBackgroundColor = new Button()
            {
                Text = "Next bar background color"
            };
            randomBarBackgroundColor.Clicked += (sender, args) =>
            {
                var ran = tabBackgroundInde++ % App.BackgroundColors.Length;
                App.HomeTabbedPage.BarBackgroundColor = App.BackgroundColors[ran];
                UpdateSelectedColors();
            };

            var randomBadgeColor = new Button()
            {
                Text = "Next badge color"
            };
            randomBadgeColor.Clicked += (sender, args) =>
            {
                var ran = badgeColor++ % App.HighlightColors.Length;
                TabBadge.SetBadgeColor(this, App.HighlightColors[ran]);
                TabBadge.SetBadgeText(this, "10");
            };

            var applyToDdl = new Picker();
            var list = ApplyTo.Keys.ToList();
            foreach (var applyToKey in list)
            {
                applyToDdl.Items.Add(applyToKey);
            }
            applyToDdl.SelectedIndex = 1; // Android only
            applyToDdl.SelectedIndexChanged += (sender, args) =>
            {
                App.HomeTabbedPage.BarBackgroundApplyTo = ApplyTo[list[applyToDdl.SelectedIndex]];
            };

            Content = new StackLayout
            {
                Children = {
                    randomSelectedColor,
                    randomBarBackgroundColor,
                    randomBadgeColor,
                    applyToDdl
                }
            };
        }

        protected override void OnAppearing()
        {
            UpdateSelectedColors();
        }

        void UpdateSelectedColors()
        {
            var selected = App.HomeTabbedPage.SelectedColor;
            var background = App.HomeTabbedPage.BarBackgroundColor;
        }
    }
}