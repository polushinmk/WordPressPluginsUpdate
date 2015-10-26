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

// Документацию по шаблону элемента пустой страницы см. по адресу http://go.microsoft.com/fwlink/?LinkId=234238

namespace WPGetListUpdate.Universal
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class SiteManagerPage : Page
    {
        public SiteManagerPage()
        {
            this.InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //var sf = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync("sites.txt");
            var sf = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFileAsync("sites.txt", Windows.Storage.CreationCollisionOption.OpenIfExists);
            using (var stream = await sf.OpenStreamForReadAsync())
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    while (!sr.EndOfStream)
                    {
                        string[] site = sr.ReadLine().Split(',');
                        if (site.Length > 1)
                        {
                            WPSite wps = new WPSite(site[0], site[1]);
                            listView.Items.Add(wps);
                        }
                    }
                }
            }
        }

        private void AppBarButton_Return_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private async void AppBarButton_Delete_Click(object sender, RoutedEventArgs e)
        {
            var sf = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFileAsync("sites.txt", Windows.Storage.CreationCollisionOption.OpenIfExists);
            List<string> sites = new List<string>();
            using (var stream = await sf.OpenStreamForReadAsync())
            {
                using (StreamReader sr = new StreamReader(stream))
                {

                    while (!sr.EndOfStream)
                    {
                        sites.Add(sr.ReadLine());
                    }
                }
            }

            using (var stream = await sf.OpenStreamForWriteAsync())
            {
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    sw.AutoFlush = true;
                    foreach (string s in sites)
                    {
                        if (!s.Contains(listView.SelectedValue.ToString()))
                        {
                            sw.WriteLine(s);
                        }
                    }
                }
            }
            listView.Items.Remove(listView.SelectedItem);
        }

        private void AppBarButton_Add_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddSitePage));
        }
    }
}
