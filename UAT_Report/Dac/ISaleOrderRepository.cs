using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UAT_Report.Models;

namespace UAT_Report.Dac
{
    public interface ISaleOrderRepository
    {
        IEnumerable<SaleOrder> GetAllSaleOrder();
        SaleOrder Get(Expression<Func<SaleOrder, bool>> expression);
        void Create(SaleOrder document);
        void Update(SaleOrder document);
       
    }
}
