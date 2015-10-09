using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPGetListUpdate.PLib;
using WPGetListUpdate.PLib.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace WPGetListUpdate.Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public async Task TestWPGetListUpdate()
        {
            WPSite site = new WPSite("Максимум", "http://maximum-kirov.ru/");
            WPListUpdate wplu = new WPListUpdate(site,"wp_update.php");
            List<string> res = await wplu.GetPluginsWithUpdate();
            Assert.IsNotNull(res);
        }
        [TestMethod]
        public void TestURi()
        {
            HttpWebRequest wbReq = WebRequest.CreateHttp("http://maximum-kirov.ru/dfgdgfdgdfgfdg");
            WebResponse wbResp = wbReq.GetResponseAsync().Result;
            string res = (new StreamReader(wbResp.GetResponseStream())).ReadToEndAsync().Result;
            Assert.IsTrue(true);
        }
    }
}
