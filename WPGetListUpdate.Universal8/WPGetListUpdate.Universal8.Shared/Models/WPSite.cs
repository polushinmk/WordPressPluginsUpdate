using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WPGetListUpdate.Universal.Shared.Models
{
    public sealed class WPSite
    {
        public string SiteName { get; private set; }
        public Uri SiteURi { get; private set; }
        public IList<string> PluginNamesNeedUpdate { get; set; }

         public WPSite(string siteName, string siteUri)
        {
            if (!IsValid(siteUri))
            {
                throw new ArgumentOutOfRangeException("siteUri", siteUri, "Плохой URL");
            }
            else
            {
                this.SiteName = siteName;
                this.SiteURi = new Uri(siteUri);
            }
        } 
        public WPSite(string siteName, Uri siteUri)
        {
            this.SiteName = siteName;
            this.SiteURi = siteUri;
            this.PluginNamesNeedUpdate = new List<string>();
        }
        private bool IsValid(string uri)
        {
            return Regex.IsMatch(uri, @"^(https?:\/\/)?([\da-zA-Z\.-]+)\.([a-zA-Z\.]{2,6})([\/\w \.-]*)*\/?$");
        }
        public override string ToString()
        {
            return this.SiteName;
        }
    }
}
