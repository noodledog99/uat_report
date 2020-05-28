using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UAT_Report.Models;

namespace UAT_Report.Dac
{
    public interface ISubServiceRepository
    {
        IEnumerable<SubService> GetAllSubService();
        SubService Get(Expression<Func<SubService, bool>> expression);
        void Create(SubService document);
        void Update(SubService document);
        void Create(SaleOrder model);
    }
}
