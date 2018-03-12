using System;

using System.Collections.Generic;

using System.Linq;

using System.Web;



namespace CryptoDepotFinal.Models

{

    public class CoinHistory

    {



        public DateTime timestamp { get; set; }

        public double value { get; set; }

        public string currency { get; set; }

        public string when { get; set; }



        public CoinHistory()

        {



        }

    }

}