﻿using System;
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

        protected override Customer ProcessSubAction(Customer customer, string SubAction, Guid? SubGid)
        {
           ModelState.Clear();
            switch (SubAction)
            {
                case "AddIdentify":
                    customer.Identifies.Add(new Identify());
                    break;
                case "RemoveIdentify":
                    Identify RecordToRemove = customer.Identifies.First(s => s.Gid == (SubGid ?? Guid.Empty));
                    customer.Identifies.Remove(RecordToRemove);
                    break;
             }
            return customer;
        }
    }

    public class IdentifiesController : CRUDController<Identify, Identifies>
    {
        

    }
}
