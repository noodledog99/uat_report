using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using UAT_Report.Dac;
using UAT_Report.Models;

namespace UAT_Report.Controllers
{
    public class SaleOrdersController : Controller
    {
        private readonly ISubServiceRepository collection;

        public SaleOrdersController(ISubServiceRepository collection)
        {
            this.collection = collection;
        }
        public IActionResult SaleOrder()
        {
            return View();
        }
        [HttpGet]
        public IActionResult SaleOrder(int? id)
        {
          
            return View();
        }
        [HttpPost]
        public IActionResult SaleOrder(SaleOrder model)
        {
            if(ModelState.IsValid)
            {
                model.SOId = Guid.NewGuid().ToString();
                collection.Create(model);
            }
            //ViewBag.Status = new SelectList(model.Status);
            return View();
        }
    }
}