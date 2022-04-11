using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(WebApplication1.App_Start.Startup))]

namespace WebApplication1.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
