using Microsoft.Owin;  
using Owin;  
  
[assembly: OwinStartup(typeof(Bingo.Startup))]  
  
namespace Bingo
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}