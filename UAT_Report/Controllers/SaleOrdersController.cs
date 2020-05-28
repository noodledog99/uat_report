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
            var ownerServiceSelectList = new List<SelectListItem>();
            var ownerServiceLst = collectionSubService
                .GetAllSubService()
                .Select(it => it.OwnerServiceName)
                .Distinct()
                .ToList();
            ownerServiceLst.ForEach(it => ownerServiceSelectList.Add(new SelectListItem { Text = it, Value = it }));
            ViewBag.OwnerService = ownerServiceSelectList;
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
            ModelState.Clear();
            return View();
        }

        [HttpGet]
        public IActionResult GetSelectSubServices(string owner)
        {
            var subServiceLst = new List<object>();
            var subServices = collectionSubService.GetAllSubService()
                .Where(it => it.OwnerServiceName == owner)
                .Select(it => it.SubServiceName)
                .Distinct()
                .ToList();
            subServices.ForEach(it => subServiceLst
            .Add(
                new
                {
                    Text = it,
                    Value = it
                })
            );
            return Json(subServices);
        }
    }
}