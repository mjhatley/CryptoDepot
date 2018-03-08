using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CryptoDepotFinal.Models;
namespace CryptoDepotFinal.Controllers
{
    public class HomeController : Controller
    {
        List<CoinDetail> lcn = new List<CoinDetail>();
        public ActionResult Index()
        {
            lcn = GetCoinData();
            return RedirectToAction("GetCoins");
            //   return View();
        }




        public ActionResult Invest(double? amount)
        {

            List<CoinDetail> TempData = GetCoinData();

            List<CoinDetail> tp = (from elt in TempData.Where(x => double.Parse(x.usd) <= amount) select elt).ToList();

            //Where(x => double.Parse(x.usd) >= amount);
            if (amount != null)
            {
                return View(tp);
            }

            else
                return View(TempData);
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
            string data = rd.ReadToEnd();
            JObject o = JObject.Parse(data);
            List<JToken> coinsRaw = o["coins"].ToList();

            for (int i = 0; i < 10; i++)
            {
                JToken j = coinsRaw[i];

                List<JToken> props = j.First.ToList();
                ViewBag.names += props[1];

            }





            //string data = rd.ReadToEnd();
            //JObject o = JObject.Parse(data);


            //ViewBag.ApiText = "The Bitcoin is" + o["coins"]["$$$"];




            return View();
        }

        public ActionResult Data()
        {
            ViewBag.Message = "Your Data page.";
            return GetData();
        }

        public List<CoinDetail> GetCoinData()
        {
            string url = "https://coinbin.org/coins";
            HttpWebRequest request = HttpWebRequest.CreateHttp(url);
            request.UserAgent = @"User-Agent: Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36 OPR/38.0.2220.41";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader rd = new StreamReader(response.GetResponseStream());
            string data = rd.ReadToEnd();
            JObject o = JObject.Parse(data);

            List<JToken> coins = o["coins"].ToList();
            lcn = new List<CoinDetail>();
            CoinDetail cn;
            foreach (var elt in o["coins"])
            {
                JToken a = elt.First();
                cn = new CoinDetail();
                cn.name = a["name"].ToString();
                cn.btc = a["btc"].ToString();
                cn.rank = a["rank"].ToString();
                cn.ticker = a["ticker"].ToString();
                cn.usd = a["usd"].ToString();

                lcn.Add(cn);

            }

            //  ViewBag.coins = items;
            return lcn;
        }

        public ActionResult GetCoins()
        {
            string url = "https://coinbin.org/coins";
            HttpWebRequest request = HttpWebRequest.CreateHttp(url);
            request.UserAgent = @"User-Agent: Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36 OPR/38.0.2220.41";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader rd = new StreamReader(response.GetResponseStream());
            string data = rd.ReadToEnd();
            JObject o = JObject.Parse(data);

            List<JToken> coins = o["coins"].ToList();
            lcn = new List<CoinDetail>();
            CoinDetail cn;
            foreach (var elt in o["coins"])
            {
                JToken a = elt.First();
                cn = new CoinDetail();
                cn.name = a["name"].ToString();
                cn.btc = a["btc"].ToString();
                cn.rank = a["rank"].ToString();
                cn.ticker = a["ticker"].ToString();
                cn.usd = a["usd"].ToString();

                lcn.Add(cn);

            }

            //  ViewBag.coins = items;
            return View(lcn);

        }
        public ActionResult Questions(GetSetData i)
        {
            ViewBag.Message = "Your Start page.";
            ViewBag.Amount = i.Amount;
            ViewBag.Period = i.Period;
            ViewBag.Risk = i.Risk;
            ViewBag.Growth = i.Growth;
            return View();
        }
        public ActionResult Results(GetSetData i)
        {
            ViewBag.Message = "Your Start page.";
            ViewBag.Amount = i.Amount;
            ViewBag.Period = i.Period;
            ViewBag.Risk = i.Risk;
            ViewBag.Growth = i.Growth;
            return View();
        }

    }
}