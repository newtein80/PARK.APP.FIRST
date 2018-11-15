using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PARK.APP.FIRST.Areas.UserManage.Models;
using PARK.APP.FIRST.Models.ApplicationModel;

namespace PARK.APP.FIRST.Areas.UserManage.Controllers
{
    [Area("UserManage")]
    public class ManageUserController : ManageUserControllerBaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ManageUserController(UserManager<ApplicationUser> userManager)
        {
            this._userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var admins = (await _userManager.GetUsersInRoleAsync("Admin")).ToArray();

            var everyone = await _userManager.Users.ToArrayAsync();

            var model = new ManageUserViewModel
            {
                Administrators = admins,
                Everyone = everyone
            };

            return View(model);
        }
    }
}