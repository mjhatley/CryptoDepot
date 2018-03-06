using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CryptoDepotFinal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult GetData()
        {
            ViewBag.Message = "Your contact page.";
            HttpWebRequest request = WebRequest.CreateHttp("https://coinbin.org/coins");
            string url_history = "https://coinbin.org/lbc/history";
            request.UserAgent = @"User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64)AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.116 Safari/537.36";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader rd = new StreamReader(response.GetResponseStream());

            string ApiText = rd.ReadToEnd();
            string data = rd.ToString();
            JObject o = JObject.Parse(ApiText);
            
            
            ViewBag.ApiText = "The Bitcoin is" + o["coins"];




            return View();
        }

        public ActionResult Data()
        {
            ViewBag.Message = "Your Data page.";
            return GetData();
        }
    }
}