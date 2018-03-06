using CryptoDepotFinal.Models;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Collections;

namespace CryptoDepotFinal.Controllers
{
    public class HomeController : Controller
    {
        public string BTC { get; set; }
        public string Name { get; set; }
        public string Rank { get; set; }
        public string Ticker { get; set; }
        public string USD { get; set; }

        public ActionResult Index()
        {


            HttpWebRequest request = WebRequest.CreateHttp("https://coinbin.org/coins");
            request.UserAgent = @"User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.116 Safari/537.36";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader rd = new StreamReader(response.GetResponseStream());
            string Read = rd.ReadToEnd();
            //ViewBag.APIText = Read;

            Read = Read.ToLower();

            ArrayList d = new ArrayList();

            JObject o = JObject.Parse(Read);

            //ViewBag.Coins = o["coins"]["eth"];


            foreach(var item in o["coins"])
            {

               this.Rank = o["coins"]["rank"].ToString();
               this.Name = o["coins"]["name"].ToString();
               this.BTC = o["coins"]["btc"].ToString();
               this.USD = o["coins"]["usd"].ToString();
               this.Ticker = o["coins"]["ticker"].ToString();
                int rank = Convert.ToInt16(this.Rank);
                d.Add(item);

            }

           // d.Sort);



            //while(i<11)
            //{
            //     = d[i];
            //    i++;
            //}


            

            //ViewBag.one = d["coins"]["name"];
            //ViewBag.two = o["coins"]["name"]["rank"];
            //ViewBag.three = o["coins"][];
            //ViewBag.four = o["coins"][];
            //ViewBag.five = o["coins"][];
            //ViewBag.six = o["coins"][];
            //ViewBag.seven = o["coins"][];
            //ViewBag.eight = o["coins"][];
            //ViewBag.nine = o["coins"][];
            //ViewBag.ten = o["coins"][];




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

        public ActionResult Data()
        {
            HttpWebRequest request = WebRequest.CreateHttp("https://coinbin.org/coins");
            request.UserAgent = @"User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.116 Safari/537.36";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader rd = new StreamReader(response.GetResponseStream());
            string Read = rd.ReadToEnd();
            //ViewBag.APIText = Read;

            Read = Read.ToLower();

            JObject o = JObject.Parse(Read);

            ViewBag.Coins = o["coins"]["eth"];


            return View();
        }



    }
}
