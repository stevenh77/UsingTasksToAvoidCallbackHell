﻿using System.IO;
using System.Web;
namespace TaskBasedServices.Web.Services
{
    public class MoneyMarketRate : IHttpHandler
    {
        public bool IsReusable { get { return false; } }

        public void ProcessRequest(HttpContext context)
        {
            var dataPath = HttpContext.Current.Server.MapPath("../Data/moneyMarketRate.json");
            using (var reader = new StreamReader(dataPath))
            {
                var result = reader.ReadToEnd();
                context.Response.ContentType = "text/json";
                context.Response.Write(result);
            }
        }
    }
}