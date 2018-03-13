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
        //list of coins
        List<Coin> lcn = new List<Coin>();
        //entity framework database initialization 
        CryptoDepotFinalEntities db = new CryptoDepotFinalEntities();
        //Number of coins that will be listed on a page
        int NberPerPage = 50;


        public ActionResult Index()
        {
            //pull the list of coin by the api and store in the variable lcn 
            // because we will use it in the fonction getcointop to find the top 5
            lcn = GetCoinData();
            // we return the top 5 coins
            return View(GetCoinsTop());
        }

        public ActionResult Risk(double? risk)
        {




            //This is where the data is sorted from the user imput.
            //Goes inside The Coins database and calculates the standard deviation
            List<Coin> tp = (from elt in db.Coins.Where(x => x.stdv <= risk) select elt).ToList();


            return View(tp);
        }









        public ActionResult Invest(GetSetData i)
        {

            //Takes the information from the database Coins
            //AsNoTracking is used to create snapshot of database table youre working with
            List<Coin> TempData = db.Coins.AsNoTracking().ToList();

            //This filters through the snapshot by the USD that is less than amount entered.
            //Saves it in list named tp
            List<Coin> tp = TempData.Where(x => double.Parse(x.usd) <= i.Amount).ToList();
            //after filtering by amount we now filter by the risk
            //if risk is low we take the bottom bottom standarddeviation which is 30%
            if (i.Risk.ToLower() == "low")
                tp = tp.Where(x => x.stdv < 0.3).ToList();
            else
            if (i.Risk.ToLower() == "medium")
                tp = tp.Where(x => x.stdv >= 0.3 && x.stdv < 0.6).ToList();

            else
            if (i.Risk.ToLower() == "high")
                tp = tp.Where(x => x.stdv >= 0.6).ToList();
            return View(tp);

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

        //public ActionResult GetData()
        //{
        //    ViewBag.Message = "Your contact page.";
        //    HttpWebRequest request = WebRequest.CreateHttp("https://coinbin.org/coins");
        //    string url_history = "https://coinbin.org/lbc/history";
        //    request.UserAgent = @"User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64)AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.116 Safari/537.36";
        //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //    StreamReader rd = new StreamReader(response.GetResponseStream());
        //    string data = rd.ReadToEnd();
        //    JObject o = JObject.Parse(data);
        //    List<JToken> coinsRaw = o["coins"].ToList();


        //    for (int i = 0; i < 10; i++)
        //    {
        //        JToken j = coinsRaw[i];

        //        List<JToken> props = j.First.ToList();
        //        ViewBag.names += props[1];

        //    }





        //string data = rd.ReadToEnd();
        //JObject o = JObject.Parse(data);


        //ViewBag.ApiText = "The Bitcoin is" + o["coins"]["$$$"];




        //    return View();
        //}

        //public ActionResult Data()
        //{
        //    ViewBag.Message = "Your Data page.";
        //    return GetData();
        //}


        //Gather all of the web information from the web API


        public List<Coin> GetCoinData()
        {
            string url = "https://coinbin.org/coins";
            HttpWebRequest request = HttpWebRequest.CreateHttp(url);
            request.UserAgent = @"User-Agent: Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36 OPR/38.0.2220.41";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            //This is where we take the API and turn it into a J object
            StreamReader rd = new StreamReader(response.GetResponseStream());
            string data = rd.ReadToEnd();
            JObject o = JObject.Parse(data);

            //Making a list of the JTokens called "o" from "coins" in API
            List<JToken> coins = o["coins"].ToList();
            lcn = new List<Coin>();
            Coin cn;
            //iterate through the Jtoken object and create a new coin object and add it to the list of coins (Lcn)
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

            //^^ returns the list of coins
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
            
            //This is where we get the coins standard deviation
            //"history" is the HISTORY section of API
            List<JToken> coins = o["history"].ToList();
            //Calculates average of value in the coins entire history
            // X is the variable that represents each coin
            double average = coins.Average(x => double.Parse(x["value"].ToString()));
            int N = coins.Count();
            double sum = coins.Sum(x => ((double.Parse(x["value"].ToString()) - average) * (double.Parse(x["value"].ToString()) - average)));
            double std = Math.Sqrt(sum / N);


            return (std);

        }

        //This sets up the pages of coins on our data page
        public ActionResult GetCoins(string Name = "", int page = 1)
        {
            //take the list of coin from the API and store in the list data and order it by rank
            
            List<Coin> data = GetCoinData().OrderBy(d => int.Parse(d.rank)).ToList();

            ViewBag.Name = Name;
            ViewBag.NberPerPage = NberPerPage;

            //This is the search bar
            if (Name.Length > 1)
            {
                //if someone has typed the name of a coin in then we filter by that input and then return the list of coins that start with the input
                data = data.Where(k => k.name.ToLower().StartsWith(Name.ToLower())).ToList();
            }

            //total number of items from the API
            ViewBag.Number = data.Count();

            //Skip the first few items in coin, creates a page,
            //NberPerPage is 50 per page 
            
            data = data.Skip(NberPerPage * (page - 1)).Take(NberPerPage).ToList();


            return View(data);
        }
        public List<Coin> GetCoinsTop()
        {
            //takes the list of coins and sorts them by the rank (top 5)
            return lcn.OrderBy(f => int.Parse(f.rank)).Take(5).ToList();
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
        //public ActionResult Results(GetSetData i)
        //{
        //    ViewBag.Message = "Your Start page.";
        //    ViewBag.Amount = i.Amount;
        //    ViewBag.Period = i.Period;
        //    ViewBag.Risk = i.Risk;
        //    ViewBag.Growth = i.Growth;
        //    return View();
        //}

        //public ActionResult GetCoinRisk()
        //{
        //    List<string> cn = new List<string> { "611", "btc", "808", "brain", "bost" };
        //    string risk = "";
        //    cn.ForEach(c => risk += " >>>>>" + c + "=" + GetCoinsSTDV(c));
        //    ViewBag.risk = risk;
        //    return View();

        //}
        public ActionResult GetCoinForecast(string coin)
        {

            //    try

            //    {

            //Talks to the forecast section of the API
            string url = "https://coinbin.org/" + coin + "/forecast"; HttpWebRequest request = HttpWebRequest.CreateHttp(url);

            request.UserAgent = @"User-Agent: Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36 OPR/38.0.2220.41";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse(); StreamReader rd = new StreamReader(response.GetResponseStream());

            string data = rd.ReadToEnd();

            //Creates JOject and makes a list of coins from forecast
            JObject o = JObject.Parse(data);
            List<JToken> coins = o["forecast"].ToList();

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
            }            
            //Returns the list of history
            return View(lh);
            //}
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

