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
            {"Both", BarBackgroundApplyTo.Both }
        };

        private Dictionary<string, BarStyle> BarStyles = new Dictionary<string, BarStyle>
        {
            {"Default", BarStyle.Default},
            {"Opaque", BarStyle.Black},
        };

        Picker applyToDdl;
        Picker applyBarStyleDdl;
        Label SelectedColorLabel;
        Label BarbackgroundColorLabel;

        public ConfigurationPage()
        {
            int tabColorIndex = 0;
            int tabBackgroundInde = 0;
            Title = "Config";
            var randomSelectedColor = new Button()
            {
                Text = "Next highlight color"
            };
            SelectedColorLabel = new Label { HorizontalTextAlignment = TextAlignment.Center };
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
            BarbackgroundColorLabel = new Label { HorizontalTextAlignment = TextAlignment.Center };
            randomBarBackgroundColor.Clicked += (sender, args) =>
            {
                var ran = tabBackgroundInde++ % App.BackgroundColors.Length;
                App.HomeTabbedPage.BarBackgroundColor = App.BackgroundColors[ran];
                UpdateSelectedColors();
            };

            applyToDdl = new Picker();
            var list = ApplyTo.Keys.ToList();
            foreach (var applyToKey in list)
            {
                applyToDdl.Items.Add(applyToKey);
            }
            applyToDdl.SelectedIndexChanged += (sender, args) =>
            {
                App.HomeTabbedPage.BarBackgroundApplyTo = ApplyTo[list[applyToDdl.SelectedIndex]];
            };

            applyBarStyleDdl = new Picker();
            var stylesList = BarStyles.Keys.ToList();
            foreach (var barStyle in stylesList)
            {
                applyBarStyleDdl.Items.Add(barStyle);
            }
            applyBarStyleDdl.SelectedIndexChanged += (sender, args) =>
            {
                App.HomeTabbedPage.BarStyle = BarStyles[stylesList[applyBarStyleDdl.SelectedIndex]];
            };

            Content = new StackLayout
            {
                Children = {
                    randomSelectedColor,
                    SelectedColorLabel,
                    randomBarBackgroundColor,
                    BarbackgroundColorLabel,
                    new Label { HorizontalTextAlignment = TextAlignment.Center, Text="Apply background color to" },
                    applyToDdl,
                    new Label { HorizontalTextAlignment = TextAlignment.Center, Text="Baar background style" },
                    applyBarStyleDdl
                }
            };
        }

        protected override void OnAppearing()
        {
            applyToDdl.SelectedIndex = 3; // Both
            applyBarStyleDdl.SelectedIndex = 0;
            UpdateSelectedColors();
        }

        void UpdateSelectedColors()
        {
            var selected = App.HomeTabbedPage.SelectedColor;
            var background = App.HomeTabbedPage.BarBackgroundColor;

            BarbackgroundColorLabel.Text = $"Background R:{background.R * 255:000} G:{background.G * 255:000} B:{background.B * 255:000}";
            SelectedColorLabel.Text = $"Selected R:{selected.R * 255:000} G:{selected.G * 255:000} B:{selected.B * 255:000}";
        }
    }
}