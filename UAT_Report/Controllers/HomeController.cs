using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.IO;
using UAT_Report.Dac;
using UAT_Report.Models;
using Newtonsoft.Json;
using JsonConvert = Newtonsoft.Json.JsonConvert;
using System.Globalization;

namespace UAT_Report.Controllers
{

    public class HomeController : Controller
    {
        private readonly ISaleOrderRepository collectionSaleOrder;
        private readonly ISubServiceRepository collectionSubService;

        public HomeController(ISaleOrderRepository collectionSaleOrder, ISubServiceRepository collectionSubService)
        {
            this.collectionSaleOrder = collectionSaleOrder;
            this.collectionSubService = collectionSubService;
        }

        public IActionResult Index()
        {
            var serviceTeams = new List<string>();
            var requestUAT = new List<int>();
            var receiveUAT = new List<int>();
            var saleorders = collectionSaleOrder.GetAllSaleOrder();

            var requestUATs = saleorders
                .GroupBy(it => it.OwnerService)
                .Select(it => new DifferenceUAT
                {
                    ServiceTeam = it.Key,
                    RequestUAT = it.Count(),
                    ReceiveUAT = it.Where(s => s.Status == Status.Success.ToString()).Count()
                })
                .ToList();

            requestUATs.ForEach(it =>
            {
                serviceTeams.Add(it.ServiceTeam);
                requestUAT.Add(it.RequestUAT);
                receiveUAT.Add(it.ReceiveUAT);
            });

            ViewBag.ServiceTeams = serviceTeams;
            ViewBag.RequestUAT = requestUAT;
            ViewBag.ReceiveUAT = receiveUAT;

            var uatThisWeek = collectionSaleOrder.GetSaleOrderThisWeek(saleorders).ToList();
            var uatLastWeek = collectionSaleOrder.GetSaleOrderLastWeek(saleorders).ToList();

            var ProgressUAT = new ProgressUAT
            {
                ProgressThisWeeks = uatThisWeek,
                ProgressLastWeeks = uatLastWeek,
                ProgressDifferences = requestUATs,
                TotalProgressThisWeeks = collectionSaleOrder.GetTotalOfProgressUAT(uatThisWeek),
                TotalProgressLastWeeks = collectionSaleOrder.GetTotalOfProgressUAT(uatLastWeek),
            };
            
            return View(ProgressUAT);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
