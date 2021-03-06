﻿using System;
using System.IO;
using System.Text;
using Microsoft.Owin;
using Newtonsoft.Json;
using Owin;

namespace WebCatcher
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Run(context =>
            {
                WriteRequest(context.Request);
                context.Response.ContentType = "text/plain";
                context.Response.StatusCode = 200;
                return context.Response.WriteAsync("Web Catcher says: Thank you!");
            });
        }

        protected void WriteRequest(IOwinRequest request)
        {
            var defaultForegroundColor = Console.ForegroundColor;
            Console.WriteLine("---------------------------------------------------------------");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("{0} {1}", request.Method, request.Uri.AbsoluteUri);
            Console.ForegroundColor = defaultForegroundColor;
            Console.WriteLine("---------------------------------------------------------------");
            using (var reader = new StreamReader(request.Body))
            {
                var content = reader.ReadToEnd();
                var formattedContent = FormatBody(request, content);
                Console.WriteLine(formattedContent);
            }

            Console.WriteLine("---------------------------------------------------------------");
        }

        private static string FormatBody(IOwinRequest request, string content)
        {
            if (request.ContentType.Contains("json"))
            {
                return JsonConvert.DeserializeObject(content)
                                  .ToString();
            }
            return content;
        }
    }
}