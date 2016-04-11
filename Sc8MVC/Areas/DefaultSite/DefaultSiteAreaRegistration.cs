using System.Web.Mvc;

namespace Sc8MVC.Areas.DefaultSite
{
    public class DefaultSiteAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "DefaultSite";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "DefaultSite_default",
                "DefaultSite/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}