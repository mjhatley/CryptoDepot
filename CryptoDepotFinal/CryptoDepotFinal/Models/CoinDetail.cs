using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CryptoDepotFinal.Models
{
    public class CoinDetail
    {


        public string btc { get; set; }
        public string name { get; set; }
        public string rank { get; set; }
        public string ticker { get; set; }
        public string usd { get; set; }
        public double stdv { get; set; }
        public CoinDetail()
        {

        }
    }
}