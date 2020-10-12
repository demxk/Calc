using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Microsoft.Extensions.Hosting;
using Calculator;
using Microsoft.AspNetCore.Mvc;

namespace CalcMiddle
{
    public class CalculatorMiddleware
    {
        public readonly RequestDelegate _next;

        public CalculatorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext ctx)
        {
            ctx.Response.ContentType = "application/json";
            ctx.Response.StatusCode = 200;
            var result = new Result(0);
            var op = ctx.Request.Query["op"];
            var a = ctx.Request.Query["a"];
            var b = ctx.Request.Query["b"];

            switch (op)
            {
                case "add":
                    result.result = Calculator.Calculator.Add(Convert.ToInt32(a), Convert.ToInt32(b));
                    break;
                case "sub":
                    result.result = Calculator.Calculator.Sub(Convert.ToInt32(a), Convert.ToInt32(b));
                    break;
                case "mul":
                    result.result = Calculator.Calculator.Mul(Convert.ToInt32(a), Convert.ToInt32(b));
                    break;
                default:
                    try
                    {
                        result.result = Calculator.Calculator.Div(Convert.ToInt32(a), Convert.ToInt32(b));
                    }
                    catch (DivideByZeroException)
                    {
                        result.isOk = false;
                    }

                    break;
            }

            await ctx.Response.WriteAsync(JsonConvert.SerializeObject(result));
        }
    }

    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, v
        // isit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public class Result
        {
            public bool ok { get; }
            public int result { get; }
        }

        private static void HandleBranch(IApplicationBuilder app)
        {
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.MapWhen(ctx => ctx.Request.Query.ContainsKey("op")
                               && ctx.Request.Query.ContainsKey("a") && ctx.Request.Query.ContainsKey("b"),
                appBuilder => appBuilder.UseMiddleware<CalculatorMiddleware>());
        }
    }
}
