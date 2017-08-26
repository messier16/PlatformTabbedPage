# PlatformTabbedPage
Make your Xamarin.Forms tabbed pages look more familiar to your users.  

## Sample

| ------------- | Android  | iOS |
| ------------- | ------------- | ------------- |
| Vanilla Forms | ![](https://media.giphy.com/media/l4FGoMd8dgNdGmlS8/giphy.gif)  | ![](https://media.giphy.com/media/l4FGBViPSdRinlMcg/giphy.gif) |
| Only tabs | ![](https://media.giphy.com/media/3oKIP581QGVRbP3Io8/giphy.gif)  | ![](https://media.giphy.com/media/l4FGICB9LNwsiyozm/giphy.gif) |
| With badges | ![](https://media.giphy.com/media/xUA7aV9yhdobQKGZ56/giphy.gif)  | ![](https://media.giphy.com/media/xUA7bg0VtPp251WZuE/giphy.gif) |

## Usage  

First of all, add the images to the specific folders for each application, for Android should be just one image for each tab, for iOS should be two images, one "normal" and another "active" that will be displayed when the user selects such tab. 

For example, assume we'll create an interface with four tabs, therefore, we provide the follwing images:

| Android  | iOS |  
| ------------- | ------------- |
| `config` | `config` & `config_active` |  
|`home`|`home` & `home_active`|
|`hashtag`|`hashtag` & `hashtag_active`|
|`message`|`message` & `message_active`|

Now, create a class that inherits from `PlatformTabbedPage`

```
public class HomeTabbedPage : PlatformTabbedPage
{
```

Add the pages to the `Children` collection of your newly created page:

```
var page = new ConfigurationPage() { Icon = "config" };
Children.Add(new BasicContentPage("Home") { Icon = "home" });
Children.Add(new BasicContentPage("Messages") { Icon = "message" });
Children.Add(new BasicContentPage("Trending") { Icon = "hashtag" });
Children.Add(page);
```

Aaaaand, that's it.

#### Badge support

Yes, this package has badge support, however, it is currently not included as part of the same package, you must install the appropiate package to make use of them.

## Install  
You must install **only one** of the following packages, installing both will bring nothing but pain to your builds.  

### Normal

`PM> Install-Package PlatformTabbedPage`   

### With badge support

`PM> Install-Package BadgedPlatformTabbedPage -Pre`  

## Acknowledgements  

 - Thans to the awesome [Plugin.Badge](https://github.com/xabre/xamarin-forms-tab-badge) from xabre
 
 ## License  

MIT