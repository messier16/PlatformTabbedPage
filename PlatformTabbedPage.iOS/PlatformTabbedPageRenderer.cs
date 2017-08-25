using System;
using System.ComponentModel;
using System.Threading.Tasks;
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
        UIColor _defaultBarBackgroundColor;

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);


            if (e.OldElement != null)
            {
                e.OldElement.PropertyChanged -= OnElementPropertyChanged;
                Cleanup(e.OldElement as TabbedPage);
            }
            if (e.NewElement != null)
            {
				e.NewElement.PropertyChanged += OnElementPropertyChanged;

				Tabbed.ChildAdded += OnTabAdded;
				Tabbed.ChildRemoved += OnTabRemoved;
            }

            _defaultTintColor = TabBar.TintColor;
            _defaultBarBackgroundColor = TabBar.BackgroundColor;
        }

        void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(PlatformTabbedPage.BarBackgroundColor):
                case nameof(PlatformTabbedPage.BarBackgroundApplyTo):
                    SetBarBackgroundColor();
                    SetTintedColor();
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
            SetTintedColor();
            SetBarBackgroundColor();

            if (FormsTabbedPage != null)
            {
                for (int i = 0; i < TabBar.Items.Length; i++)
                {
                    AddTabBadge(i);

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

        private void SetBarBackgroundColor()
        {
            if (FormsTabbedPage.BarBackgroundApplyTo.HasFlag(BarBackgroundApplyTo.iOS))
            {
                TabBar.BackgroundColor = FormsTabbedPage.BarBackgroundColor != default(Color)
                    ? FormsTabbedPage.BarBackgroundColor.ToUIColor()
                    : _defaultBarBackgroundColor;
            }
            else
            {
                TabBar.BackgroundColor = _defaultBarBackgroundColor;
            }
		}

		private void AddTabBadge(int tabIndex)
		{
			var element = Tabbed.Children[tabIndex];
			element.PropertyChanged += OnTabbedPagePropertyChanged;

			if (TabBar.Items.Length > tabIndex)
			{
				var tabBarItem = TabBar.Items[tabIndex];
				UpdateTabBadgeText(tabBarItem, element);
				UpdateTabBadgeColor(tabBarItem, element);
				UpdateTabBadgeTextAttributes(tabBarItem, element);
			}
		}

		private void UpdateTabBadgeText(UITabBarItem tabBarItem, Element element)
		{
			var text = TabBadge.GetBadgeText(element);

			tabBarItem.BadgeValue = string.IsNullOrEmpty(text) ? null : text;
		}

		private void UpdateTabBadgeTextAttributes(UITabBarItem tabBarItem, Element element)
		{
			if (!tabBarItem.RespondsToSelector(new ObjCRuntime.Selector("setBadgeTextAttributes:forState:")))
			{
				// method not available, ios < 10
				Console.WriteLine("Plugin.Badge: badge text attributes only available starting with iOS 10.0.");
				return;
			}

			var attrs = new UIStringAttributes();

			var textColor = TabBadge.GetBadgeTextColor(element);
			if (textColor != Color.Default)
			{
				attrs.ForegroundColor = textColor.ToUIColor();
			}

			var font = TabBadge.GetBadgeFont(element);
			if (font != Font.Default)
			{
				attrs.Font = font.ToUIFont();
			}

			tabBarItem.SetBadgeTextAttributes(attrs, UIControlState.Normal);
		}

		private void UpdateTabBadgeColor(UITabBarItem tabBarItem, Element element)
		{
			if (!tabBarItem.RespondsToSelector(new ObjCRuntime.Selector("setBadgeColor:")))
			{
				// method not available, ios < 10
				Console.WriteLine("Plugin.Badge: badge color only available starting with iOS 10.0.");
				return;
			}

			var tabColor = TabBadge.GetBadgeColor(element);
			if (tabColor != Color.Default)
			{
				tabBarItem.BadgeColor = tabColor.ToUIColor();
			}
		}

		private void OnTabbedPagePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			var page = sender as Page;
			if (page == null)
				return;

			if (e.PropertyName == TabBadge.BadgeTextProperty.PropertyName)
			{
				if (CheckValidTabIndex(page, out int tabIndex))
					UpdateTabBadgeText(TabBar.Items[tabIndex], page);
				return;
			}

			if (e.PropertyName == TabBadge.BadgeColorProperty.PropertyName)
			{
				if (CheckValidTabIndex(page, out int tabIndex))
					UpdateTabBadgeColor(TabBar.Items[tabIndex], page);
				return;
			}

			if (e.PropertyName == TabBadge.BadgeTextColorProperty.PropertyName || e.PropertyName == TabBadge.BadgeFontProperty.PropertyName)
			{
				if (CheckValidTabIndex(page, out int tabIndex))
					UpdateTabBadgeTextAttributes(TabBar.Items[tabIndex], page);
				return;
			}
		}

		public bool CheckValidTabIndex(Page page, out int tabIndex)
		{
			tabIndex = Tabbed.Children.IndexOf(page);
			return tabIndex < TabBar.Items.Length;
		}


		private async void OnTabAdded(object sender, ElementEventArgs e)
		{
			//workaround for XF, tabbar is not updated at this point and we have no way of knowing for sure when it will be updated. so we have to wait ... 
			await Task.Delay(10);
			var page = e.Element as Page;
			if (page == null)
				return;

			var tabIndex = Tabbed.Children.IndexOf(page);
			AddTabBadge(tabIndex);
		}

		private void OnTabRemoved(object sender, ElementEventArgs e)
		{
			e.Element.PropertyChanged -= OnTabbedPagePropertyChanged;
		}

		protected override void Dispose(bool disposing)
		{
			Cleanup(Tabbed);

			base.Dispose(disposing);
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

			page.ChildAdded -= OnTabAdded;
			page.ChildRemoved -= OnTabRemoved;
		}
    }
}
