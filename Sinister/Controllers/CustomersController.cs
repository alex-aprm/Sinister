using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sinister.Models.CRM;
using Sinister.DAL;

namespace Sinister.Controllers
{
    public class CustomersController : CRUDController<Customer,Customers>
    {
    }
}
