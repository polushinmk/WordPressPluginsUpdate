using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;

namespace BackgroundTaskWinMD
{
    public sealed class WPGetListUpdateBackgroundTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var def = taskInstance.GetDeferral();
            var sf = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFileAsync("sites.txt", Windows.Storage.CreationCollisionOption.OpenIfExists);
            List<string> sites = new List<string>();
            using (var stream = await sf.OpenStreamForReadAsync())
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    while (!sr.EndOfStream)
                    {
                        string[] s = sr.ReadLine().Split(',');
                        HttpWebRequest wbReq = WebRequest.CreateHttp(s[1]+"/wp_update.php");
                        WebResponse wbResp = await wbReq.GetResponseAsync();
                        using (StreamReader sr1 = new StreamReader(wbResp.GetResponseStream()))
                        {
                            while (!sr1.EndOfStream)
                            {
                                string plugins = await sr1.ReadToEndAsync();

                                if (plugins != "none" && plugins != string.Empty && plugins.Split(',').Length > 0)
                                {
                                    sites.Add(s[0]);

                                }
                            }
                        }
                            
                    }
                }
            }

            var tiledoc = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150Text04);
            var nodes = tiledoc.SelectNodes("tile/visual/binding/text");
            
            
            string allsites=string.Empty;
            foreach(string s in sites)
            {
                allsites += s + ", ";
            }
            nodes[0].InnerText = "Обновление от "+ DateTime.Now + ". Имеются обновления плагинов на следующих сайтах: " + allsites.TrimEnd(new char[] { ',',' '});
            
            TileNotification tile = new TileNotification(tiledoc);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tile);
            def.Complete();
        }
    }
}
