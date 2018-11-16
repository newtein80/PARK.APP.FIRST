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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description")] ManageRoleViewModel manageRoleViewModel)
        {
            var role = await _roleManager.FindByNameAsync(manageRoleViewModel.Name);

            if(role == null)
            {
                try
                {
                    var result = await _roleManager.CreateAsync(
                        new ApplicationRole
                        {
                            Name = manageRoleViewModel.Name,
                            Description = manageRoleViewModel.Description
                        });

                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", result.Errors.First().ToString());
                        return View();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return NotFound($"Unable to load user with ID ''.");
            }
        }

        public async Task<IActionResult> Details(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound($"Unable to load user with ID ''.");
            }

            var vm_role = new ManageRoleViewModel()
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description
            };

            return View(vm_role);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound($"Unable to load user with ID ''.");
            }

            var vm_role = new ManageRoleViewModel()
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description
            };

            return View(vm_role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(string id, [Bind("Name,Description")] ApplicationRole applicationRole)
        public async Task<IActionResult> Edit(string id, [Bind("Name,Description")] ManageRoleViewModel manageRoleViewModel)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound($"Unable to load user with ID ''.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // http://qaru.site/questions/15283775/rolemanagerdeleteasync-or-updateasync-not-working-as-expected-in-aspnet-core-20
                    role.Name = manageRoleViewModel.Name;
                    role.Description = manageRoleViewModel.Description;

                    //var result = await _roleManager.UpdateAsync(applicationRole);
                    var result = await _roleManager.UpdateAsync(role);

                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", result.Errors.First().ToString());
                        return View(manageRoleViewModel);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }
            return View(manageRoleViewModel);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound($"Unable to load user with ID ''.");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var result = await _roleManager.DeleteAsync(role);

                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError("", result.Errors.First().ToString());
                            return View();
                        }
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }
        }
    }
}