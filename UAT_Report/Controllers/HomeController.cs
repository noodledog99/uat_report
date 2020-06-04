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
using UAT_Report.Logics;

namespace UAT_Report.Controllers
{

    public class HomeController : Controller
    {
        private readonly ISaleOrderRepository collectionSaleOrder;
        private readonly ISubServiceRepository collectionSubService;
        private SaleOrderFunctions funcSale;

        public HomeController(ISaleOrderRepository collectionSaleOrder, ISubServiceRepository collectionSubService )
        {
            funcSale = new SaleOrderFunctions(collectionSubService, collectionSaleOrder);
            this.collectionSaleOrder = collectionSaleOrder;
            this.collectionSubService = collectionSubService;
            
        }

        public IActionResult Index()
        {
            var serviceTeams = new List<string>();
            var requestUAT = new List<int>();
            var receiveUAT = new List<int>();
           
            var uatThisWeek = funcSale.GetSaleOrderThisWeek().ToList();
            var uatLastWeek = funcSale.GetSaleOrderLastWeek().ToList();

            var requestUATs = funcSale.GetDifferenceUATDataTable().ToList();
            var differenceUATGraph = funcSale.GetDifferenceUATDataGraph(uatThisWeek).ToList();
            differenceUATGraph.ForEach(it =>
            {
                serviceTeams.Add(it.ServiceTeam);
                requestUAT.Add(it.RequestUAT);
                receiveUAT.Add(it.ReceiveUAT);
            });

            ViewBag.ServiceTeams = serviceTeams;
            ViewBag.RequestUAT = requestUAT;
            ViewBag.ReceiveUAT = receiveUAT;

           

            var ProgressUAT = new ProgressUAT
            {
                ProgressThisWeeks = uatThisWeek,
                ProgressLastWeeks = uatLastWeek,
                ProgressDifferences = requestUATs,
                TotalProgressThisWeeks = funcSale.GetTotalOfProgressUAT(uatThisWeek),
                TotalProgressLastWeeks = funcSale.GetTotalOfProgressUAT(uatLastWeek),
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
