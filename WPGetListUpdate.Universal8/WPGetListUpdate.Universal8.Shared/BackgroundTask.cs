using System;
using System.Collections.Generic;
using System.Text;
using Windows.ApplicationModel.Background;
using WPGetListUpdate.Universal.Shared.Models;
using WPGetListUpdate.Universal.Shared;

namespace WPGetListUpdate.Universal
{
    class BackgroundTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deff = taskInstance.GetDeferral();
            WPSite WPSite = new WPSite("Maximum", new Uri("http://maximum-kirov.ru/"));
            WPListUpdate wplu = new WPListUpdate(WPSite);
            List<string> PluginsWithUpdate = await wplu.GetPluginsWithUpdate();
            deff.Complete();
        }
    }
}
