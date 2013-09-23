using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sinister
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            ModelBinders.Binders.Add(typeof(decimal?), new DecimalModelBinder());

            DefaultModelBinder.ResourceClassKey = "Errors";

            var existingProvider = ModelValidatorProviders.Providers.Single(x => x is ClientDataTypeModelValidatorProvider);
            ModelValidatorProviders.Providers.Remove(existingProvider);
            ModelValidatorProviders.Providers.Add(new ClientNumberValidatorProvider());

            ModelBinders.Binders.Add(typeof(DateTime), new DateTimeBinder());
            ModelBinders.Binders.Add(typeof(DateTime?), new DateTimeBinder());

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}