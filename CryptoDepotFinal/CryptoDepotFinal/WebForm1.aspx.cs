using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CryptoDepotFinal
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (var webClient = new WebClient())
            {
                //get a string representation of our json
                string rawJSON = webClient.DownloadString("https://coinbin.org/coins");
                //convert the JSON string to a string of objects
                Models.CoinCollection coinCollection = JsonConvert.DeserializeObject<Models.CoinCollection>(rawJSON);
                // Do some computation

            }
        }

        
    }
}