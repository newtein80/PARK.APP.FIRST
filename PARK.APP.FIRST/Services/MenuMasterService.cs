using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PARK.APP.FIRST.Areas.SystemManage.Models.Menu;
using PARK.APP.FIRST.Data;

namespace PARK.APP.FIRST.Services
{
    // https://www.codeproject.com/Articles/1237650/ASP-NET-Core-User-Role-Base-Dynamic-Menu-Managemen
    public class MenuMasterService : IMenuMasterService
    {
        private readonly ApplicationDbContext applicationDbContext;

        public MenuMasterService(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public IEnumerable<MenuMaster> GetMenuMaster()
        {
            return applicationDbContext.MenuMaster.AsEnumerable();
        }

        public IEnumerable<MenuMaster> GetMenuMaster(string UserRole)
        {
            var result = applicationDbContext.MenuMaster.Where(m => m.User_Roll == UserRole).ToList();
            return result;
        }
    }
}
