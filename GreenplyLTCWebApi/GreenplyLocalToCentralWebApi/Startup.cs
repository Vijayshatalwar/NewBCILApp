using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using System.Web;

[assembly: OwinStartup(typeof(GreenplyLocalToCentralWebApi.Startup))]

namespace GreenplyLocalToCentralWebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
            ConfigureAuth(app);
        }
    }
}
