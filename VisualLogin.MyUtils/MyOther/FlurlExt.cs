using System.Net;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace VisualLogin.MyUtils.MyOther
{
    public static class FlurlExt
    {
        public static IFlurlRequest WithProxyCookies(this string obj, string proxy=null )
        {
            var builder = new FlurlClientBuilder();
            if (string.IsNullOrWhiteSpace(proxy))
            {
                builder.ConfigureInnerHandler(handler =>
                {
                    handler.UseCookies = true;
                    handler.CookieContainer = new CookieContainer();
                });
            }
            else
            {
                builder.ConfigureInnerHandler(handler =>
                {
                    handler.Proxy = new WebProxy(proxy);
                    handler.UseProxy = true;
                    handler.UseCookies = true;
                    handler.CookieContainer = new CookieContainer();
                });
            }
         
            var cli=builder.Build();
            return cli.Request(obj);
        }
    }
}