using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Android.Graphics;
using Android.Graphics.Drawables;
using Xamarin.Forms.Platform.Android.AppCompat;
using Xamarin.Forms;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Messier16.Forms.Controls;
using Messier16.Forms.Controls.Droid;
using Xamarin.Forms.Platform.Android;
using FormsColor = Xamarin.Forms.Color;

[assembly: ExportRenderer(typeof(PlatformTabbedPage), typeof(PlatformTabbedPageRenderer))]
namespace Messier16.Forms.Controls.Droid
{
    public class PlatformTabbedPageRenderer : TabbedPageRenderer
    {
        public static void Init()
        {
            var unused = DateTime.UtcNow;
        }

        private PlatformTabbedPage FormsTabbedPage => Element as PlatformTabbedPage;
        private Android.Graphics.Color _selectedColor = Android.Graphics.Color.Black;
        private static readonly Android.Graphics.Color DefaultUnselectedColor = FormsColor.Gray.Darken().ToAndroid();
        private static Android.Graphics.Color _barBackgroundDefault;
        private Android.Graphics.Color _unselectedColor = DefaultUnselectedColor;

        ViewPager _viewPager;
        TabLayout _tabLayout;
        LinearLayout _tabStrip;

        private const int DeleayBeforeTabAdded = 10;
        protected readonly Dictionary<Element, BadgeView> BadgeViews = new Dictionary<Element, BadgeView>();

        protected override void OnElementChanged(ElementChangedEventArgs<TabbedPage> e)
        {

            base.OnElementChanged(e);

            // Get tabs
            for (var i = 0; i < ChildCount; i++)
            {
                var v = GetChildAt(i);
                var pager = v as ViewPager;
                if (pager != null)
                    _viewPager = pager;
                else if (v is TabLayout)
                    _tabLayout = (TabLayout)v;
            }

            _tabStrip = _tabLayout.FindChildOfType<LinearLayout>();

            if (e.OldElement != null)
            {
                _tabLayout.TabSelected -= TabLayout_TabSelected;
                _tabLayout.TabUnselected -= TabLayout_TabUnselected;

                Cleanup(e.OldElement);
                Cleanup(Element);
            }

            if (e.NewElement != null)
            {
                _barBackgroundDefault = (_tabLayout.Background as ColorDrawable)?.Color ??
                    Android.Graphics.Color.Blue;
                SetSelectedColor();
                SetBarBackgroundColor();
                _tabLayout.TabSelected += TabLayout_TabSelected;
                _tabLayout.TabUnselected += TabLayout_TabUnselected;

                SetupTabColors();
                SelectTab(0);
            }

            for (var i = 0; i < _tabLayout.TabCount; i++)
            {
                AddTabBadge(i);
            }

            Element.ChildAdded += OnTabAdded;
            Element.ChildRemoved += OnTabRemoved;

        }

        void SelectTab(int position)
        {
            if (_tabLayout.TabCount > position)
            {
                _tabLayout.GetTabAt(position).Icon?
                    .SetColorFilter(_selectedColor, PorterDuff.Mode.SrcIn);
                _tabLayout.GetTabAt(position).Select();
            }
            else
            {
                throw new IndexOutOfRangeException();
            }
        }


        void SetupTabColors()
        {
            _tabLayout.SetSelectedTabIndicatorColor(_selectedColor);
            _tabLayout.SetTabTextColors(_unselectedColor, _selectedColor);
            for (int i = 0; i < _tabLayout.TabCount; i++)
            {
                var tab = _tabLayout.GetTabAt(i);
                tab.Icon?.SetColorFilter(_unselectedColor, PorterDuff.Mode.SrcIn);
            }
        }

        private void TabLayout_TabUnselected(object sender, TabLayout.TabUnselectedEventArgs e)
        {
            var tab = e.Tab;
            tab.Icon?.SetColorFilter(_unselectedColor, PorterDuff.Mode.SrcIn);
        }

        private void TabLayout_TabSelected(object sender, TabLayout.TabSelectedEventArgs e)
        {
            var tab = e.Tab;
            _viewPager.CurrentItem = tab.Position;
            tab.Icon?.SetColorFilter(_selectedColor, PorterDuff.Mode.SrcIn);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            int lastPosition = _tabLayout.SelectedTabPosition;
            switch (e.PropertyName)
            {
                case nameof(PlatformTabbedPage.BarBackgroundColor):
                case nameof(PlatformTabbedPage.BarBackgroundApplyTo):
                    SetBarBackgroundColor();
                    SetupTabColors();
                    SelectTab(lastPosition);
                    break;
                case nameof(PlatformTabbedPage.SelectedColor):
                    SetSelectedColor();
                    SetupTabColors();
                    SelectTab(lastPosition);
                    break;
                default:
                    base.OnElementPropertyChanged(sender, e);
                    break;
            }
        }

        private void AddTabBadge(int tabIndex)
        {
            var element = Element.Children[tabIndex];
            var view = _tabLayout?.GetTabAt(tabIndex).CustomView ?? _tabStrip?.GetChildAt(tabIndex);

            var badgeView = (view as ViewGroup)?.FindChildOfType<BadgeView>();

            if (badgeView == null)
            {
                var imageView = (view as ViewGroup)?.FindChildOfType<ImageView>();

                var badgeTarget = imageView?.Drawable != null
                    ? (Android.Views.View)imageView
                    : (view as ViewGroup)?.FindChildOfType<TextView>();

                //create badge for tab
                badgeView = new BadgeView(Context, badgeTarget);
            }

            BadgeViews[element] = badgeView;

            //get text
            var badgeText = TabBadge.GetBadgeText(element);
            badgeView.Text = badgeText;

            // set color if not default
            var tabColor = TabBadge.GetBadgeColor(element);
            if (tabColor != FormsColor.Default)
            {
                badgeView.BadgeColor = tabColor.ToAndroid();
            }

            // set text color if not default
            var tabTextColor = TabBadge.GetBadgeTextColor(element);
            if (tabTextColor != FormsColor.Default)
            {
                badgeView.TextColor = tabTextColor.ToAndroid();
            }

            // set font if not default
            var font = TabBadge.GetBadgeFont(element);
            if (font != Font.Default)
            {
                badgeView.Typeface = font.ToTypeface();
            }

            element.PropertyChanged += OnTabbedPagePropertyChanged;
        }


        protected virtual void OnTabbedPagePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var element = sender as Element;
            if (element == null)
                return;

            if (!BadgeViews.TryGetValue(element, out BadgeView badgeView))
            {
                return;
            }

            if (e.PropertyName == TabBadge.BadgeTextProperty.PropertyName)
            {
                badgeView.Text = TabBadge.GetBadgeText(element);
                return;
            }

            if (e.PropertyName == TabBadge.BadgeColorProperty.PropertyName)
            {
                badgeView.BadgeColor = TabBadge.GetBadgeColor(element).ToAndroid();
                return;
            }

            if (e.PropertyName == TabBadge.BadgeTextColorProperty.PropertyName)
            {
                badgeView.TextColor = TabBadge.GetBadgeTextColor(element).ToAndroid();
                return;
            }

            if (e.PropertyName == TabBadge.BadgeFontProperty.PropertyName)
            {
                badgeView.Typeface = TabBadge.GetBadgeFont(element).ToTypeface();
                return;
            }
        }

        private void SetSelectedColor()
        {

            if (FormsTabbedPage.SelectedColor != default(Xamarin.Forms.Color))
                _selectedColor = FormsTabbedPage.SelectedColor.ToAndroid();
        }

        private void SetBarBackgroundColor()
        {
            if (FormsTabbedPage.BarBackgroundApplyTo.HasFlag(BarBackgroundApplyTo.Android))
            {
                _tabLayout.SetBackgroundColor(FormsTabbedPage.BarBackgroundColor.ToAndroid());
                _unselectedColor = FormsTabbedPage.BarBackgroundColor != default(Xamarin.Forms.Color)
                    ? FormsTabbedPage.BarBackgroundColor.Darken().ToAndroid()
                    : DefaultUnselectedColor;
            }
            else
            {
                _tabLayout.SetBackgroundColor(_barBackgroundDefault);
                _unselectedColor = DefaultUnselectedColor;
            }
        }

        private void Cleanup(TabbedPage page)
        {
            if (page == null)
            {
                return;
            }

            foreach (var tab in page.Children)
            {
                tab.PropertyChanged -= OnTabbedPagePropertyChanged;
            }

            page.ChildRemoved -= OnTabRemoved;
            page.ChildAdded -= OnTabAdded;

            BadgeViews.Clear();
        }

        private void OnTabRemoved(object sender, ElementEventArgs e)
        {
            e.Element.PropertyChanged -= OnTabbedPagePropertyChanged;
            BadgeViews.Remove(e.Element);
        }

        private async void OnTabAdded(object sender, ElementEventArgs e)
        {
            await Task.Delay(DeleayBeforeTabAdded);

            var page = e.Element as Page;
            if (page == null)
                return;

            var tabIndex = Element.Children.IndexOf(page);
            AddTabBadge(tabIndex);
        }

        protected override void Dispose(bool disposing)
        {
            Cleanup(Element);
            base.Dispose(disposing);
        }
    }
}
