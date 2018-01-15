using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(ChatSggw.API.Startup))]

namespace ChatSggw.API
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            ConfigureAuth(app);
            SwaggerConfig.Register(config);
            WebApiConfig.Register(config);
            app.UseWebApi(config);
        }
    }
}
