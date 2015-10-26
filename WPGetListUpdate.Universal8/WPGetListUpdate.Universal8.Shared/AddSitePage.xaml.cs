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

// Документацию по шаблону элемента пустой страницы см. по адресу http://go.microsoft.com/fwlink/?LinkId=234238

namespace WPGetListUpdate.Universal
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class AddSitePage : Page
    {
        public AddSitePage()
        {
            this.InitializeComponent();
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            var sf = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFileAsync("sites.txt", Windows.Storage.CreationCollisionOption.OpenIfExists);
            List<string> file = new List<string>();
            using (var stream = await sf.OpenStreamForReadAsync())
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    while (!sr.EndOfStream)
                    {
                        file.Add(sr.ReadLine());
                    }
                }
            }
            using (var stream = await sf.OpenStreamForWriteAsync())
            {
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    foreach(string s in file)
                    {
                        sw.WriteLine(s);
                    }
                    sw.WriteLine(textBox.Text + "," + textBox_Copy.Text);
                }
            }
            this.Frame.Navigate(typeof(SiteManagerPage));
        }
    }
}
