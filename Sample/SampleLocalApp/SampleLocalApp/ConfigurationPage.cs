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
        private Dictionary<string, BarBackgroundApplyTo> ApplyTo = new Dictionary<string, BarBackgroundApplyTo>
        {
            {"None", BarBackgroundApplyTo.None},
            {"Android", BarBackgroundApplyTo.Android},
            {"iOS", BarBackgroundApplyTo.iOS},
            {"Both", BarBackgroundApplyTo.iOS |BarBackgroundApplyTo.Android }
        };

        Button _changeHighlightColorButton,
               _changeBackgroundColorButton,
               _setBadgeColorButton;

        BoxView _highlightColor,
                _backgroundColor,
                _badgeColor;

        Picker _selectPlatformPicker,
               _selectPageForBadgePicker;

        Entry _badgeValueEntry;

        public ConfigurationPage()
        {
            var grid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(1,GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1,GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1,GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1,GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1,GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1,GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1,GridUnitType.Auto) }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star)},
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)},
                }
            };

            _changeHighlightColorButton = new Button { Text = "Next highlight color" };
            _changeHighlightColorButton.Clicked += ChangeHighlightColorButton_Clicked;
            _highlightColor = new BoxView();

            _changeBackgroundColorButton= new Button { Text = "Next background color" };
            _changeBackgroundColorButton.Clicked += ChangeBackgroundColorButton_Clicked;
            _backgroundColor = new BoxView();

            _selectPlatformPicker = new Picker();
            ApplyTo.Keys.ToList().ForEach(_selectPlatformPicker.Items.Add);
            _selectPlatformPicker.SelectedIndexChanged += SelectPlatformPicker_SelectedIndexChanged;

            _selectPageForBadgePicker = new Picker();
            _selectPageForBadgePicker.SelectedIndexChanged += SelectPageForBadgePicker_SelectedIndexChanged;

            _setBadgeColorButton = new Button { Text = "Next badge color" };
            _setBadgeColorButton.Clicked += ChangeBadgeColorButton_Clicked;
            _badgeColor = new BoxView();

            _badgeValueEntry = new Entry { Placeholder = "Badge content" };
            _badgeValueEntry.TextChanged += BadgeValueEntry_TextChanged;

            grid.Children.Add(_changeHighlightColorButton, 0, 0);
            grid.Children.Add(_highlightColor, 1, 0);

            grid.Children.Add(_changeBackgroundColorButton, 0, 1);
            grid.Children.Add(_backgroundColor, 1, 1);

            grid.Children.Add(_selectPlatformPicker, 0, 2);
            Grid.SetColumnSpan(_selectPlatformPicker, 2);

            grid.Children.Add(_selectPageForBadgePicker, 0, 3);
            Grid.SetColumnSpan(_selectPageForBadgePicker, 2);

            grid.Children.Add(_setBadgeColorButton, 0, 4);
            grid.Children.Add(_badgeColor, 1, 4);

            grid.Children.Add(_badgeValueEntry, 0, 5);
            Grid.SetColumnSpan(_badgeValueEntry, 2);

            Content = grid;
        }

        protected override void OnAppearing()
        {
            _selectPlatformPicker.SelectedIndex = 1; // Android only
            _highlightColor.BackgroundColor = App.HomeTabbedPage.BarBackgroundColor;
            _backgroundColor.BackgroundColor = App.HomeTabbedPage.BarBackgroundColor;
            _badgeColor.BackgroundColor = Color.Red;
            App.HomeTabbedPage.Children
               .Select(p => p.Title).ToList()
               .ForEach(_selectPageForBadgePicker.Items.Add);
            _selectPageForBadgePicker.SelectedIndex = 0;
        }

        int highlightColor = 0;
        void ChangeHighlightColorButton_Clicked(object sender, EventArgs e)
        {
            var ran = highlightColor++ % App.HighlightColors.Length;
            App.HomeTabbedPage.SelectedColor = App.HighlightColors[ran];
            _highlightColor.BackgroundColor =App.HighlightColors[ran];
        }

        int backgroundColor;
        void ChangeBackgroundColorButton_Clicked(object sender, EventArgs e)
        {
            var ran = backgroundColor++ % App.HighlightColors.Length;
            App.HomeTabbedPage.BarBackgroundColor = App.BackgroundColors[ran];
            _backgroundColor.BackgroundColor = App.BackgroundColors[ran];
        }

        void SelectPlatformPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var val = ApplyTo.ElementAt(_selectPlatformPicker.SelectedIndex);
            App.HomeTabbedPage.BarBackgroundApplyTo = val.Value;
        }

        int badgeColor;
        void ChangeBadgeColorButton_Clicked(object sender, EventArgs e)
        {
            var ran = badgeColor++ % App.HighlightColors.Length;
            var color = App.HighlightColors[ran];
            var page = App.HomeTabbedPage.Children[_selectPageForBadgePicker.SelectedIndex];
            TabBadge.SetBadgeColor(page, color);
            _badgeColor.BackgroundColor = color;

        }

        void SelectPageForBadgePicker_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        void BadgeValueEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            var page = App.HomeTabbedPage.Children[_selectPageForBadgePicker.SelectedIndex];
            TabBadge.SetBadgeText(page, e.NewTextValue);
        }
    }
}