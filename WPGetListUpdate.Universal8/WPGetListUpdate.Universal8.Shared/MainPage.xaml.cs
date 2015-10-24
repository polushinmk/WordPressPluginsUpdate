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
            WPSite WPSite = new WPSite("Maximum", new Uri("http://maximum-kirov.ru/"));
            WPListUpdate wplu = new WPListUpdate(WPSite);
            List<string> PluginsWithUpdate = await wplu.GetPluginsWithUpdate();

            WPSite WPSite1 = new WPSite("Express", new Uri("http://express-kirov.ru/"));
            WPListUpdate wplu1 = new WPListUpdate(WPSite1);
            List<string> PluginsWithUpdate1 = await wplu1.GetPluginsWithUpdate();


            List<List<string>> lls = new List<List<string>>();
            lls.Add(PluginsWithUpdate);
            lls.Add(PluginsWithUpdate1);
            listView.ItemsSource = lls;
        }
    }
}
