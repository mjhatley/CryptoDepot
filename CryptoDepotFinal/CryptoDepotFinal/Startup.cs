using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CryptoDepotFinal.Startup))]
namespace CryptoDepotFinal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
