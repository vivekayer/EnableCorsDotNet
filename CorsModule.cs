using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;

namespace EnableCorsDotNet
{
    /// <summary>
    /// Dynamically adds header to response if the request is from an allowed list
    /// </summary>
    public class CorsModule : IHttpModule
    {
        private List<string> origins = null;

        public void Dispose() { }

        public void Init(HttpApplication context)
        {
            // Expects a comma seperated list
            // <add key ="allowedOrigins" value="http://www.mysite.com, https://checkout.mysite.com"/>
            if (ConfigurationManager.AppSettings["allowedOrigins"] != null)
            {
                string allowedOrigins = ConfigurationManager.AppSettings["allowedOrigins"];
                origins = allowedOrigins.Split(',').Select(x => x.Trim()).ToList();
            }

            context.EndRequest += new EventHandler(context_EndRequest);
        }

        void context_EndRequest(object sender, EventArgs e)
        {
            HttpResponse response = HttpContext.Current.Response;

            try
            {
                Uri uriReferer = null;
                if (HttpContext.Current.Request.Headers["Referer"] != null)
                {
                    uriReferer = new Uri(HttpContext.Current.Request.Headers["Referer"].ToString());
                }
                else if (HttpContext.Current.Request.Headers["Origin"] != null)
                {
                    uriReferer = new Uri(HttpContext.Current.Request.Headers["Origin"].ToString());
                }

                string uri = uriReferer.GetLeftPart(UriPartial.Authority);

                if (this.origins != null && this.origins.Any() && origins.Contains(uri, StringComparer.OrdinalIgnoreCase))
                {
                    response.AddHeader("Access-Control-Allow-Origin", uri);
                }

            }
            catch /*( Exception ex )*/
            {
                // Enable for testing
                //response.AddHeader("CorsModuleDebugError", ex.Message);
            }
        }


    }
}
