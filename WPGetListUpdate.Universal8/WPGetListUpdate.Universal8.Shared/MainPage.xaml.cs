using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WPGetListUpdate.Universal.Shared.Models;
using WPGetListUpdate.Universal.Shared;
using Windows.Networking.Connectivity;
using Windows.UI.Notifications;

// Документацию по шаблону элемента пустой страницы см. по адресу http://go.microsoft.com/fwlink/?LinkId=234238

namespace WPGetListUpdate.Universal
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            listView.Items.Clear();
            ConnectionProfile profile = NetworkInformation.GetInternetConnectionProfile();
            bool isinternet = (profile != null) && profile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
            List<string> sites = new List<string>();
            if (isinternet)
            {
                var sf = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFileAsync("sites.txt", Windows.Storage.CreationCollisionOption.OpenIfExists);
                using (var stream = await sf.OpenStreamForReadAsync())
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        while (!sr.EndOfStream)
                        {
                            string[] s = sr.ReadLine().Split(',');
                            if (s.Length > 1)
                            {
                                sites.Add(s[0]);
                                WPSite wps = new WPSite(s[0], s[1]);
                                WPListUpdate wplu = new WPListUpdate(wps);
                                List<string> plugins = await wplu.GetPluginsWithUpdate();
                                plugins.Insert(0, "Сайт " + s[0] + ":");
                                plugins.Add(string.Empty);
                                foreach (string ss in plugins)
                                {
                                    listView.Items.Add(ss);
                                }
                            }
                        }
                    }
                }
                if (sites.Count > 0)
                {
                    var tiledoc = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150Text04);
                    var nodes = tiledoc.SelectNodes("tile/visual/binding/text");


                    string allsites = string.Empty;
                    foreach (string s in sites)
                    {
                        allsites += s + ", ";
                    }
                    nodes[0].InnerText = "Имеются обновления плагинов на следующих сайтах: " + allsites.TrimEnd(new char[] { ',', ' ' });

                    TileNotification tile = new TileNotification(tiledoc);
                    TileUpdateManager.CreateTileUpdaterForApplication().Update(tile);
                }
            }
        }

        private void AppBarButton_Add_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SiteManagerPage));
        }

        

        
    }
}
