using PARK.APP.FIRST.Areas.SystemManage.Models.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PARK.APP.FIRST.Services
{
    // https://www.codeproject.com/Articles/1237650/ASP-NET-Core-User-Role-Base-Dynamic-Menu-Managemen
    public interface IMenuMasterService
    {
        IEnumerable<MenuMaster> GetMenuMaster();
        IEnumerable<MenuMaster> GetMenuMaster(String UserRole);
    }
}
