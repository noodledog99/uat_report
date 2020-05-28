using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UAT_Report.Dac;
using UAT_Report.Models;

namespace UAT_Report.Controllers
{
    public class SubServicesController : Controller
    {
        private readonly ISubServiceRepository collection;

        public SubServicesController(ISubServiceRepository collection)
        {
            this.collection = collection;
        }

        public IActionResult SubService()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SubService(SubService model)
        {
            if (ModelState.IsValid)
            {
                model.SubServiceId = Guid.NewGuid().ToString();
                collection.Create(model);
            }
            return View();
        }
    }
}