using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PARK.APP.FIRST.Areas.UserManage.Models;
using PARK.APP.FIRST.Models.ApplicationModel;

namespace PARK.APP.FIRST.Areas.UserManage.Controllers
{
    public class ManageRoleController : ManageRoleControllerBaseController
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public ManageRoleController(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        // https://www.youtube.com/watch?v=OiYVuNg5cO4
        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();

            //var roles_v2 = _roleManager.Roles.ToAsyncEnumerable

            var vm_role = new List<ManageRoleViewModel>();

            roles.ForEach(item
                 => vm_role.Add(
                     new ManageRoleViewModel()
                     {
                         Id = item.Id,
                         Name = item.Name,
                         Description = item.Description
                     }
                ));


            return View(vm_role);
        }
    }
}