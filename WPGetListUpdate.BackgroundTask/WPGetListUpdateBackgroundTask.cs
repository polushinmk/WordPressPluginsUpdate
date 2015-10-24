using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Windows.ApplicationModel.Background;
using WPGetListUpdate.Universal.Shared;
using WPGetListUpdate.Universal.Shared.Models;

namespace WPGetListUpdate.BackgroundTask
{
    public sealed class WPGetListUpdateBackgroundTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            Debug.WriteLine("Task.Run");
            var _deferal = taskInstance.GetDeferral();
            WPSite WPSite = new WPSite("Maximum", new Uri("http://maximum-kirov.ru/"));
            WPListUpdate wplu = new WPListUpdate(WPSite);
            string PluginsWithUpdate = await wplu.GetPluginsWithUpdate();
            Debug.WriteLine(PluginsWithUpdate);
            _deferal.Complete();
        }
    }
}
