using PARK.APP.FIRST.Areas.VulnManage.Models.Vuln;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PARK.APP.FIRST.Areas.VulnManage.Repositories
{
    // https://exceptionnotfound.net/using-dapper-asynchronously-in-asp-net-core-2-1/
    public interface ITVulnRepository
    {
        Task<List<Tvuln>> GetByVulnGroupSeq(int vulnGroupSeq);
    }
}