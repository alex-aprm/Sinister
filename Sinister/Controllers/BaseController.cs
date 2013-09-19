using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sinister.DAL;
using Sinister.Global;

namespace Sinister.Controllers
{
    public class BaseController : Controller
    {
        public BaseController() : base()
        {
            string cstr = God.GetConnectionString("SinisterConnection");
            this.repository = new Repository(cstr);
        }
        protected Repository repository { get; set; }
    }
}
