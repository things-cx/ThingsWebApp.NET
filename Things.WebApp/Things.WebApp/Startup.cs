using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Rewrite.Internal;

namespace Things.WebApp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //http://stackoverflow.com/questions/42819588/unexpected-token-in-angular2-cli
            app.UseRewriter(new RewriteOptions
            {
                Rules =
                {
                    new RewriteRule(@"^(?!.*(assets\/.*|.chunk.js|.bundle.js|.bundle.map|.bundle.js.gz|.bundle.css|.bundle.css.gz|.png|.jpg|.ico|.json)).*$", "/", true)

                }
            });

            //TODO: I think I should change this!
            app.UseFileServer();
        }
    }
}
