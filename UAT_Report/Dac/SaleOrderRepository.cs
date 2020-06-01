using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UAT_Report.Models;

namespace UAT_Report.Dac
{
    public class SaleOrderRepository : ISaleOrderRepository
    {
        public readonly IMongoCollection<SaleOrder> collection;
        public SaleOrderRepository(DbConfig dbConfig)
        {
            var client = new MongoClient(dbConfig.MongoDbConnectionString);
            var database = client.GetDatabase(dbConfig.MongoDbName);
            collection = database.GetCollection<SaleOrder>(dbConfig.SaleOrder);
        }

        public void Create(SaleOrder document)
            => collection.InsertOne(document);

        public SaleOrder Get(Expression<Func<SaleOrder, bool>> expression)
            => collection.Find(expression).FirstOrDefault();

        public IEnumerable<SaleOrder> GetAllSaleOrder()
            => collection.Find(it => true).ToList();

        public void Update(SaleOrder document)
        {
            var def = Builders<SaleOrder>.Update
               .Set(it => it.Status, document.Status)
               .Set(it => it.SONumber, document.SONumber)
               .Set(it => it.Description, document.Description)
               .Set(it => it.OwnerService, document.OwnerService)
               .Set(it => it.SubServiceName, document.SubServiceName)
               .Set(it => it.CustomerNo, document.CustomerNo)
               .Set(it => it.CustomerName, document.CustomerName);
            collection.UpdateOne(it => it.SOId == document.SOId, def);
        }

        public IEnumerable<ProgressOnWeek> GetSaleOrderThisWeek(IEnumerable<SaleOrder> saleOrders)
        {
            DateTime startOfWeek = DateTime.Today.AddDays(
               (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek - (int)DateTime.Today.DayOfWeek);
            DateTime endOfWeek = startOfWeek.AddDays(6);
            return GetDataByDateTime(saleOrders, startOfWeek, endOfWeek).ToList();
        }

        public IEnumerable<ProgressOnWeek> GetSaleOrderLastWeek(IEnumerable<SaleOrder> saleOrders)
        {
            DateTime startOfLastWeek = DateTime.Now.AddDays(-(int)(DateTime.Today.DayOfWeek - (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek) - 7);
            DateTime endOfLastWeek = startOfLastWeek.AddDays(6);
            return GetDataByDateTime(saleOrders, startOfLastWeek, endOfLastWeek).ToList();
        }

        private IEnumerable<ProgressOnWeek> GetDataByDateTime(IEnumerable<SaleOrder> saleorders, DateTime start, DateTime end)
        {
            var saleorderByDateTime = saleorders.Where(it => it.SODate >= start && it.SODate < end)
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

            return saleorderByDateTime;
        }

        public TotalUAT GetTotalOfProgressUAT(IEnumerable<ProgressOnWeek> progressOnWeeks)
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
    }
}
