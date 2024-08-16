using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using System.Web;

[assembly: OwinStartup(typeof(GreenplyCentralToLocalWebApi.Startup))]

namespace GreenplyCentralToLocalWebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
            ConfigureAuth(app);
        }
    }
}
