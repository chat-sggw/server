using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
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
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));
            WebApiConfig.Register(config);
            ConfigureAuth(app);
            SwaggerConfig.Register(config);
            app.UseWebApi(config);
        }
    }
}
