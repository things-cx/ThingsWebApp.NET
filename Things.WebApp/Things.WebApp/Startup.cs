﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Rewrite.Internal;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Things.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
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

            //Try to use app.Use()
            //TODO: think about looking at context.Request.Headers["accept-language"] as well!

            app.MapWhen(context => String.IsNullOrWhiteSpace(context.Request.Cookies["lang"]), HandleEnBranch);
            app.MapWhen(context => context.Request.Cookies["lang"] == "en-US", HandleEnBranch);
            app.MapWhen(context => context.Request.Cookies["lang"] == "af-ZA", HandleAfBranch);

            app.Run(async context =>
            {
                await context.Response.WriteAsync("No lnaguage found");
            });
        }

        #region Cultures

        private static void HandleEnBranch(IApplicationBuilder app)
        {
            app.UseFileServer(new FileServerOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/en-US")),
                EnableDefaultFiles = true
            });
        }

        private static void HandleAfBranch(IApplicationBuilder app)
        {
            app.UseFileServer(new FileServerOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/af-ZA")),
                EnableDefaultFiles = true
            });
        }

        #endregion
    }
}
