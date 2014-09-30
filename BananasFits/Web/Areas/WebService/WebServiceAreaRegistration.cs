using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Mvc;

namespace Web.Areas.WebService
{
    public class WebServiceAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "WebService";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "WebService_default",
                "WebService/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}