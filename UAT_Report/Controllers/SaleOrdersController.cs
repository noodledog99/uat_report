using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UAT_Report.Dac;
using UAT_Report.Models;

namespace UAT_Report.Controllers
{
    public class SaleOrdersController : Controller
    {
        private readonly ISaleOrderRepository collectionSaleOrder;
        private readonly ISubServiceRepository collectionSubService;
        public SaleOrdersController(ISaleOrderRepository collectionSaleOrder, ISubServiceRepository collectionSubService)
        {
            this.collectionSaleOrder = collectionSaleOrder;
            this.collectionSubService = collectionSubService;
        }

        public IActionResult SaleOrder()
        {
           
            var ownerServices = new List<SelectListItem> { new SelectListItem { Text = "--- Select ---", Value = "" } };
            var ownerServiceLst = collectionSubService
                .GetAllSubService()
                .Select(it => it.OwnerServiceName)
                .Distinct()
                .ToList();


            ownerServiceLst.ForEach(it => ownerServices.Add(new SelectListItem { Text = it, Value = it }));
            ViewBag.OwnerService = ownerServices;

            var subServices = new List<SelectListItem> { new SelectListItem { Text = "--- Select ---", Value = "" } };
            var SubServiceList = collectionSubService
                .GetAllSubService()
                .Select(i => i.SubServiceName)
                .Distinct()
                .ToList();

            SubServiceList.ForEach(i => subServices.Add(new SelectListItem { Text = i, Value = i }));
            ViewBag.SubService = subServices ;
            
            return View();
        }
        [HttpPost]
        public IActionResult SaleOrder(SaleOrder model)
        {
            if (ModelState.IsValid)
            {
                model.SOId = Guid.NewGuid().ToString();
                collectionSaleOrder.Create(model);
            }


            return View();
        }

        //public IActionResult GetSelectSubServices(string owner)
        //{

        //}
    }
}