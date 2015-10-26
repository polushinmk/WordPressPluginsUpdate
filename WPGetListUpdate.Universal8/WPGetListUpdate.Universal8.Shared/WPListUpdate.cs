using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPGetListUpdate.Universal.Shared.Models;
using System.IO;

using System.Net;


namespace WPGetListUpdate.Universal.Shared
{
    public sealed class WPListUpdate
    {
        public WPSite Site { get; private set; }
        public string PathToScripts { get; set; }
        public WPListUpdate(WPSite site) : this(site, "/wp_update.php") { }

        public WPListUpdate(WPSite site, string pathToScript)
        {
            if (!TestURL(site.SiteURi + pathToScript))
            {
                throw new ArgumentOutOfRangeException("pathToScript", "Указан неверный путь");
            }
            else
            {
                this.Site = site;
                this.PathToScripts = pathToScript;
            }

        }

        public bool TestURL(string url)
        {
            try
            {
                HttpWebRequest wbReq = WebRequest.CreateHttp(url);
                WebResponse wbResp = wbReq.GetResponseAsync().Result;
                return true;
            }
            catch
            {
                return false;
            }

        }

        public async Task<List<string>> GetPluginsWithUpdate()
        {
            HttpWebRequest wbReq = WebRequest.CreateHttp(this.Site.SiteURi + this.PathToScripts);
            WebResponse wbResp = await wbReq.GetResponseAsync();
            using (StreamReader sr = new StreamReader(wbResp.GetResponseStream()))
            {
                string plugins = await sr.ReadToEndAsync();
                return plugins.Split(',').ToList();
            }
        }
    }
}
