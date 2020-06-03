using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using UAT_Report.Dac;
using UAT_Report.Models;

namespace UAT_Report.Logics
{
    public class SaleOrderFunctions
    {
        private List<ProgressOnWeek> subServiceModel;
        private readonly ISubServiceRepository collectionSubService;
        private readonly ISaleOrderRepository collectionSaleOrder;

        public SaleOrderFunctions(ISubServiceRepository collectionSubService, ISaleOrderRepository collectionSaleOrder)
        {
            this.collectionSubService = collectionSubService;
            this.collectionSaleOrder = collectionSaleOrder;
        }

        public IEnumerable<ProgressOnWeek> GetSaleOrderThisWeek()
        {
            var saleorderByDateTime = collectionSaleOrder.GetAllSaleOrder()
             .Where(it => it.SODate >= GetDateStartOfLastWeek())
             .GroupBy(it => it.OwnerService)
             .Select(it => new ProgressOnWeek
             {
                 ServiceTeam = it.Key,
                 RequestUAT = it.Count(),
                 NumberReciveUAT = it.Where(s => s.Status == Status.Success.ToString()).Count(),
                 PercentReciveUAT = (int)Math.Round((decimal)it.Where(s => s.Status == Status.Success.ToString()).Count() * 100 / it.Count()),
                 NumberPendding = it.Where(s => s.Status == Status.Pending.ToString()).Count(),
                 PercentPendding = (int)Math.Round((decimal)it.Where(s => s.Status == Status.Pending.ToString()).Count() * 100 / it.Count())
             })
             .ToList();

            var summaryWeek = GetAllSummaryWeek(saleorderByDateTime);
            return summaryWeek;
        }

        public IEnumerable<ProgressOnWeek> GetSaleOrderLastWeek()
        {
            var saleorderByDateTime = collectionSaleOrder.GetAllSaleOrder()
           .Where(it => it.SODate >= GetDateStartOfLastWeek() && it.SODate < GetDateEndOfLastWeek())
           .GroupBy(it => it.OwnerService)
           .Select(it => new ProgressOnWeek
           {
               ServiceTeam = it.Key,
               RequestUAT = it.Count(),
               NumberReciveUAT = it.Where(s => s.Status == Status.Success.ToString()).Count(),
               PercentReciveUAT = (int)Math.Round((decimal)it.Where(s => s.Status == Status.Success.ToString()).Count() * 100 / it.Count()),
               NumberPendding = it.Where(s => s.Status == Status.Pending.ToString()).Count(),
               PercentPendding = (int)Math.Round((decimal)it.Where(s => s.Status == Status.Pending.ToString()).Count() * 100 / it.Count())
           })
           .ToList();

            var summaryWeek = GetAllSummaryWeek(saleorderByDateTime);
            return summaryWeek;
        }

        public TotalUAT GetTotalOfProgressUAT(IEnumerable<ProgressOnWeek> progressOnWeeks)
        {
            try
            {
                var calPercentReceive = (decimal)progressOnWeeks.Sum(it => it.NumberReciveUAT) * 100 / progressOnWeeks.Sum(it => it.RequestUAT);
                var calPercentPending = (decimal)progressOnWeeks.Sum(it => it.NumberPendding) * 100 / progressOnWeeks.Sum(it => it.RequestUAT);
                return new TotalUAT
                {
                    TotalRequestUAT = progressOnWeeks.Sum(it => it.RequestUAT),
                    TotalNumberReceive = progressOnWeeks.Sum(it => it.NumberReciveUAT),
                    TotalPercentReceive = Math.Round(calPercentReceive),
                    TotalNumberPending = progressOnWeeks.Sum(it => it.NumberPendding),
                    TotalPercentPending = Math.Round(calPercentPending),
                };
            }
            catch (DivideByZeroException)
            {
                return new TotalUAT();
            }
        }

        private IEnumerable<ProgressOnWeek> GetAllSummaryWeek(List<ProgressOnWeek> saleorderByDateTime)
        {
            subServiceModel = new List<ProgressOnWeek>();
            collectionSubService.GetAllSubService()
                .Select(it => it.OwnerServiceName)
                .Distinct()
                .ToList()
                .ForEach(it => subServiceModel
                .Add(new ProgressOnWeek
                {
                    ServiceTeam = it,
                    RequestUAT = 0,
                    NumberReciveUAT = 0,
                    PercentReciveUAT = 0,
                    NumberPendding = 0,
                    PercentPendding = 0
                }));

            saleorderByDateTime.ForEach(it =>
            {
                var subServiceData = subServiceModel.FirstOrDefault(s => s.ServiceTeam == it.ServiceTeam);
                subServiceData.RequestUAT = it.RequestUAT;
                subServiceData.NumberReciveUAT = it.NumberReciveUAT;
                subServiceData.PercentReciveUAT = it.PercentReciveUAT;
                subServiceData.NumberPendding = it.NumberPendding;
                subServiceData.PercentPendding = it.PercentPendding;
            });

            return subServiceModel;
        }

        public IEnumerable<DifferenceUAT> GetDifferenceUATDataTable()
        {
            var requestUATs = collectionSaleOrder.GetAllSaleOrder()
                  .GroupBy(it => it.OwnerService)
                  .Select(it => new DifferenceUAT
                  {
                      ServiceTeam = it.Key,
                      RequestUAT = it.Where(x => x.SODate >= GetDateStartOfWeek() && x.SODate < GetDateEndOfWeek()).Count(),
                      ReceiveUAT = it.Where(s => s.Status == Status.Success.ToString() && (s.SODate >= GetDateStartOfWeek() && s.SODate < GetDateEndOfWeek())).Count()
                  })
                  .ToList();

            return requestUATs;
        }

        public IEnumerable<DifferenceUAT> GetDifferenceUATDataGraph(List<ProgressOnWeek> uatThisWeek)
        {
            var requestUATs = uatThisWeek
                  .Select(it => new DifferenceUAT
                  {
                      ServiceTeam = it.ServiceTeam,
                      RequestUAT = it.RequestUAT,
                      ReceiveUAT = it.NumberReciveUAT
                  })
                  .ToList();

            return requestUATs;
        }

        public DateTime GetDateStartOfWeek()
        {
            DateTime startOfWeek = DateTime.Today.AddDays(
            (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek - (int)DateTime.Today.DayOfWeek);
            return startOfWeek;
        }

        public DateTime GetDateEndOfWeek()
        {
            DateTime endOfWeek = GetDateStartOfWeek().AddDays(6);
            return endOfWeek;
        }

        public DateTime GetDateStartOfLastWeek()
        {
            DateTime startOfLastWeek = DateTime.Now.AddDays(-(int)(DateTime.Today.DayOfWeek - (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek) - 7);
            return startOfLastWeek;
        }

        public DateTime GetDateEndOfLastWeek()
        {
            DateTime endOfLastWeek = GetDateStartOfLastWeek().AddDays(6);
            return endOfLastWeek;
        }

    }
}
