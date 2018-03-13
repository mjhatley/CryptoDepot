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
    //private double GetStandardDeviation(List<double> doubleList)
    //{
    //    double average = doubleList.Average();
    //    double sumOfDerivation = 0;
    //    foreach (double value in doubleList)
    //    {
    //        sumOfDerivation += (value) * (value);
    //    }
    //    double sumOfDerivationAverage = sumOfDerivation / (doubleList.Count - 1);
    //    return Math.Sqrt(sumOfDerivationAverage - (average * average));
    //}


    public class HomeController : Controller
    {
        List<Coin> lcn = new List<Coin>();
       CryptoDepotFinalEntities db = new   CryptoDepotFinalEntities();
        int NberPerPage = 50;
        public ActionResult Index()
        {
            lcn = GetCoinData();
            //return RedirectToAction("GetCoins");
             return View(GetCoinsTop());
        }

        public ActionResult Risk(double? risk)
        {
          //  List<Coin> TempData = GetCoinData();
           
         

            //This is where the data is sorted from the user imput.
            List<Coin> tp = (from elt in db.Coins.Where(x =>  x.stdv  <= risk) select elt).ToList();
           // tp.ForEach(d=>d.stdv= GetCoinsSTDV(d.name));
                //Where(x => double.Parse(x.usd) >= amount);
            //if (risk! = null)
                
                return View(tp);
        }

           
   




        //}
        //public ActionResult GetCoinHistory()
        //{
        //    string url = "https://coinbin.org/coins/history";
        //    HttpWebRequest request = HttpWebRequest.CreateHttp(url);
        //    request.UserAgent = @"User-Agent: Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36 OPR/38.0.2220.41";
        //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        //    StreamReader rd = new StreamReader(response.GetResponseStream());
        //    string data = rd.ReadToEnd();
        //    JObject o = JObject.Parse(data);

        //    List<JToken> coins = o["history"].ToList();
        //    lcn = new List<Coin>();
        //    coins.
        //    return View();
        //}

        public ActionResult Invest(GetSetData i)
        {
           

            List<Coin> TempData = db.Coins.AsNoTracking().ToList();


            List<Coin> tp = TempData.Where(x => double.Parse(x.usd) <= i.Amount).ToList();
            if (i.Risk.ToLower() == "low")
                tp = tp.Where(x => x.stdv < 0.3).ToList();
            else
            if (i.Risk.ToLower() == "medium")
                tp = tp.Where(x => x.stdv >= 0.3 && x.stdv <0.6).ToList();

            else
            if (i.Risk.ToLower() == "high")
                tp = tp.Where(x => x.stdv >= 0.6).ToList();

            //(from elt in db.Coins.Where(x => int.Parse( x.usd )< i.Amount) select elt).ToList();

            //Where(x => double.Parse(x.usd) >= amount);
            if (i.Amount != null)
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

        public List<Coin> GetCoinData()
        {
            string url = "https://coinbin.org/coins";
            HttpWebRequest request = HttpWebRequest.CreateHttp(url);
            request.UserAgent = @"User-Agent: Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36 OPR/38.0.2220.41";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader rd = new StreamReader(response.GetResponseStream());
            string data = rd.ReadToEnd();
            JObject o = JObject.Parse(data);

            List<JToken> coins = o["coins"].ToList();
            lcn = new List<Coin>();
            Coin cn;
            foreach (var elt in o["coins"])
            {
                JToken a = elt.First();
                cn = new Coin();
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

        public double GetCoinsSTDV(string coin)
        {

            string url = "https://coinbin.org/" + coin + "/history";

            HttpWebRequest request = HttpWebRequest.CreateHttp(url);
            request.UserAgent = @"User-Agent: Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36 OPR/38.0.2220.41";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader rd = new StreamReader(response.GetResponseStream());
            string data = rd.ReadToEnd();
            JObject o = JObject.Parse(data);

            List<JToken> coins = o["history"].ToList();
            double average = coins.Average(x => double.Parse(x["value"].ToString()));
            int N = coins.Count();
            double sum = coins.Sum(x => ((double.Parse(x["value"].ToString()) - average) * (double.Parse(x["value"].ToString()) - average)));
            double std = Math.Sqrt(sum / N);


            return (std);

        }


        public ActionResult GetCoins(string Name="", int page =1)
        {
            List<Coin> data = GetCoinData().OrderBy(d=> int.Parse(d.rank)).ToList();

            ViewBag.Name = Name;
            ViewBag.NberPerPage = NberPerPage;

            if (Name.Length>1)
            {
                data = data.Where(k => k.name.ToLower().StartsWith(Name.ToLower())).ToList();
            }

  ViewBag.Number = data.Count();

            data = data.Skip(NberPerPage * (page - 1)).Take(NberPerPage).ToList();

          
           return View(data);
        }
        public List<Coin> GetCoinsTop()
        {
            return  lcn.OrderBy(f=>int.Parse( f.rank)).Take(5).ToList();
        }
        public ActionResult GetCoins1()
        {
            string url = "https://coinbin.org/coins";
            HttpWebRequest request = HttpWebRequest.CreateHttp(url);
            request.UserAgent = @"User-Agent: Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36 OPR/38.0.2220.41";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader rd = new StreamReader(response.GetResponseStream());
            string data = rd.ReadToEnd();
            JObject o = JObject.Parse(data);

            List<JToken> coins = o["coins"].ToList();
            lcn = new List<Coin>();
            Coin cn;
            foreach (var elt in o["coins"])
            {
                JToken a = elt.First();
                cn = new Coin();
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
        //public ActionResult Questions(GetSetData i)
        //{
        //    ViewBag.Message = "Your Start page.";
        //    ViewBag.Amount = i.Amount;
        //    ViewBag.Period = i.Period;
        //    ViewBag.Risk = i.Risk;
        //    ViewBag.Growth = i.Growth;
        //    return View();
        //}
        public ActionResult Results(GetSetData i)
        {
            ViewBag.Message = "Your Start page.";
            ViewBag.Amount = i.Amount;
            ViewBag.Period = i.Period;
            ViewBag.Risk = i.Risk;
            ViewBag.Growth = i.Growth;
            return View();
        }

        public ActionResult GetCoinRisk()
        {
            List<string> cn = new List<string> { "611", "btc", "808", "brain", "bost" };
            string risk = "";
            cn.ForEach(c => risk += " >>>>>" + c + "=" + GetCoinsSTDV(c));
            ViewBag.risk = risk;
            return View();

            ; }
            public ActionResult GetCoinForecast(string coin)

            {

                //    try

                //    {

                string url = "https://coinbin.org/" + coin + "/forecast"; HttpWebRequest request = HttpWebRequest.CreateHttp(url);

                request.UserAgent = @"User-Agent: Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36 OPR/38.0.2220.41";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse(); StreamReader rd = new StreamReader(response.GetResponseStream());

                string data = rd.ReadToEnd();

                JObject o = JObject.Parse(data); List<JToken> coins = o["forecast"].ToList();

                List<CoinHistory> lh = new List<CoinHistory>();

                CoinHistory hist = new CoinHistory();

                //we stop when there is not history

                //if (coins.Count == 0)

                foreach (var item in coins)

                {

                    hist = new CoinHistory();

                    //hist.currency = item["value.currency"].ToString();

                    hist.value = double.Parse(item["usd"].ToString());

                    hist.when = item["when"].ToString();

                    hist.timestamp = DateTime.Parse(item["timestamp"].ToString()); lh.Add(hist);
                }            // double lastv = double.Parse(coins.OrderByDescending(f => f["timestamp"]).First()["value"].ToString());

                return View(lh);            //}

                //catch (Exception)

                //{

                //    return View();

                //}

            }
        public ActionResult Questions()
        {
            return View();
        }
         public ActionResult GetCoinHistory(string coin)

            {

                //    try

                //    {

                string url = "https://coinbin.org/" + coin + "/history"; HttpWebRequest request = HttpWebRequest.CreateHttp(url);

                request.UserAgent = @"User-Agent: Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36 OPR/38.0.2220.41";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();



                StreamReader rd = new StreamReader(response.GetResponseStream());

                string data = rd.ReadToEnd();

                JObject o = JObject.Parse(data); List<JToken> coins = o["history"].ToList();

                List<CoinHistory> lh = new List<CoinHistory>();

                CoinHistory hist = new CoinHistory();

                //we stop when there is not history

                //if (coins.Count == 0)

                foreach (var item in coins)

                {

                    hist = new CoinHistory();

                    hist.currency = item["value.currency"].ToString();

                    hist.value = double.Parse(item["value"].ToString());

                    hist.when = item["when"].ToString();

                    hist.timestamp = DateTime.Parse(item["timestamp"].ToString()); lh.Add(hist);



                }               // double lastv = double.Parse(coins.OrderByDescending(f => f["timestamp"]).First()["value"].ToString());

                return View(lh);            //}

                //catch (Exception)

                //{

                //    return View();

                //}

            }
        }

    }

