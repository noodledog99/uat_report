using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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

        public IActionResult SaleOrderIndex()
        {
             var saleorder = collectionSaleOrder.GetAllSaleOrder();
             return View(saleorder.ToList());
        }
       
        public IActionResult SaleOrderInsert()
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
        public IActionResult SaleOrderInsert(SaleOrder model)
        {
            if (ModelState.IsValid)
            {
                model.SOId = Guid.NewGuid().ToString();
                model.Status = Status.Pending.ToString();
               
                collectionSaleOrder.Create(model);
            }
            ModelState.Clear();
            return RedirectToAction("SaleOrderInsert");
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

        [HttpGet]
        public IActionResult SaleOrderServiceView(string serviceName)
        {
            var saleOrders = collectionSaleOrder.GetAllSaleOrder().Where(it => it.OwnerService == serviceName).ToList();
            return View(saleOrders);
        }
    }
}