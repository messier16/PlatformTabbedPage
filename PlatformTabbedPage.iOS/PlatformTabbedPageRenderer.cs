using System;
using System.ComponentModel;
using CoreGraphics;
using Messier16.Forms.Controls;
using Messier16.Forms.Controls.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(PlatformTabbedPage), typeof(PlatformTabbedPageRenderer))]
namespace Messier16.Forms.Controls.iOS
{
    public class PlatformTabbedPageRenderer : TabbedRenderer
    {
        public new static void Init()
        {
            var unused = DateTime.UtcNow;
        }

        PlatformTabbedPage FormsTabbedPage => Element as PlatformTabbedPage;
        UIColor _defaultTintColor;
        UIColor _defaultUnselectedItemTintColor;
        UIColor _defaultBarBackgroundColor;
        UIBarStyle _barStyle;

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);


            if (e.OldElement != null)
            {
                e.OldElement.PropertyChanged -= OnElementPropertyChanged;
            }
            if (e.NewElement != null)
            {
                e.NewElement.PropertyChanged += OnElementPropertyChanged;
            }

            _defaultTintColor = TabBar.TintColor;
            _defaultUnselectedItemTintColor = TabBar.UnselectedItemTintColor;
            _defaultBarBackgroundColor = UIColor.Black;
            _barStyle = UIBarStyle.Default;
        }

        void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(PlatformTabbedPage.BarBackgroundColor):
                case nameof(PlatformTabbedPage.BarStyle):
                case nameof(PlatformTabbedPage.BarBackgroundApplyTo):
                case nameof(PlatformTabbedPage.UnselectedItemTintColor):
                    SetBarBackgroundColor();
                    SetBarStyle();
                    SetTintedColor();
                    SetUnselectedIconColor();
                    break;
                case nameof(PlatformTabbedPage.SelectedColor):
                    SetTintedColor();
                    break;
            }
        }

        public override void ViewWillAppear(bool animated)
        {

            if (TabBar?.Items == null)
                return;
            SetBarStyle();
            SetTintedColor();
            SetBarBackgroundColor();
            SetUnselectedIconColor();

            if (FormsTabbedPage != null)
            {
                for (int i = 0; i < TabBar.Items.Length; i++)
                {
                    var item = TabBar.Items[i];
                    var icon = FormsTabbedPage.Children[i].Icon;

                    if (item == null)
                        return;
                    try
                    {
                        icon = icon + "_active";
                        if (item.SelectedImage?.AccessibilityIdentifier == icon)
                            return;
                        item.SelectedImage = UIImage.FromBundle(icon);
                        item.SelectedImage.AccessibilityIdentifier = icon;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Unable to set selected icon: " + ex);
                    }
                }
            }

            base.ViewWillAppear(animated);
        }

        private void SetTintedColor()
        {
            TabBar.TintColor = FormsTabbedPage.SelectedColor != default(Color) ?
                FormsTabbedPage.SelectedColor.ToUIColor() :
                _defaultTintColor;
        }

        private void SetUnselectedIconColor()
        {
            TabBar.UnselectedItemTintColor = FormsTabbedPage.UnselectedItemTintColor != default(Color) ?
                FormsTabbedPage.UnselectedItemTintColor.ToUIColor() :
                _defaultUnselectedItemTintColor;
        }

        private void SetBarBackgroundColor()
        {
            if (FormsTabbedPage.BarBackgroundApplyTo.HasFlag(BarBackgroundApplyTo.iOS))
            {
                TabBar.BarTintColor = FormsTabbedPage.BarBackgroundColor != default(Color)
                    ? FormsTabbedPage.BarBackgroundColor.ToUIColor()
                    : _defaultBarBackgroundColor;

            }
            else
            {
                TabBar.BackgroundColor = _defaultBarBackgroundColor;
            }
        }

        private void SetBarStyle()
        {
            TabBar.BarStyle = FormsTabbedPage.BarStyle == BarStyle.Default ? UIBarStyle.Default : UIBarStyle.Black;
        }
    }
}
