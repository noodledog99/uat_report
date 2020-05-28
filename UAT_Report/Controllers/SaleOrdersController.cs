using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace UAT_Report.Controllers
{
    public class SaleOrdersController : Controller
    {
        public IActionResult SaleOrder()
        {
            return View();
        }
    }
}