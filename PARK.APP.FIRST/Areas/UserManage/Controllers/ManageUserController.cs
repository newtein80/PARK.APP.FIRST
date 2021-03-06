﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PARK.APP.FIRST.Areas.UserManage.Models;
using PARK.APP.FIRST.Filters;
using PARK.APP.FIRST.Models.ApplicationModel;

namespace PARK.APP.FIRST.Areas.UserManage.Controllers
{
    [Area("UserManage")]
    public class ManageUserController : ManageUserControllerBaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public ManageUserController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        #region+ UserList
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

        // https://www.c-sharpcorner.com/article/asp-net-core-mvc-authentication-and-role-based-authorization-with-asp-net-core/
        public IActionResult UserList()
        {
            var users = new List<ManageUserListViewModel>();

            users = _userManager.Users.Select(u => new ManageUserListViewModel
            {
                Id = u.Id,
                ConcurrencyStamp = u.ConcurrencyStamp,
                Email = u.Email,
                EmailConfirmed = u.EmailConfirmed,
                LockoutEnabled = u.LockoutEnabled,
                NormalizedEmail = u.NormalizedEmail,
                NormalizedUserName = u.NormalizedUserName,
                PhoneNumber = u.PhoneNumber,
                UserName = u.UserName,
                SecurityStamp = u.SecurityStamp
            }).ToList();

            return View(users);
        }
        #endregion

        #region+ EditUser
        public class ManageUserInfoEditViewModel
        {
            [Required]
            public string Id { get; set; }
            [Required]
            public string UserName { get; set; }
            public string PhoneNumber { get; set; }
            [Required]
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID ''.");
            }

            var vm_user = new ManageUserInfoEditViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName
            };

            return View(vm_user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(string id, [Bind("Name,Description")] ApplicationRole applicationRole)
        public async Task<IActionResult> EditUser(string id, [Bind("Id, Email, UserName, PhoneNumber")] ManageUserInfoEditViewModel manageUserInfoEditViewModel)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID ''.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // http://qaru.site/questions/15283775/rolemanagerdeleteasync-or-updateasync-not-working-as-expected-in-aspnet-core-20
                    user.Email = manageUserInfoEditViewModel.Email;
                    user.UserName = manageUserInfoEditViewModel.UserName;
                    user.PhoneNumber = manageUserInfoEditViewModel.PhoneNumber;

                    //var result = await _roleManager.UpdateAsync(applicationRole);
                    var result = await _userManager.UpdateAsync(user);

                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", result.Errors.First().ToString());
                        return View(manageUserInfoEditViewModel);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                return RedirectToAction(nameof(UserList));
            }
            else
            {
                // https://stackoverflow.com/questions/5212248/get-error-message-if-modelstate-isvalid-fails
                //var errors = ModelState.Select(x => x.Value.Errors)
                //            .Where(y => y.Count > 0)
                //            .ToList();

                // https://stackoverflow.com/questions/15333513/why-modelstate-isvalid-always-return-false-in-mvc/15333657
                var errorsItems = ModelState
                            .Where(x => x.Value.Errors.Count > 0)
                            .Select(x => new { x.Key, x.Value.Errors })
                            .ToArray();

                foreach(var errorItem in errorsItems)
                {
                    ModelState.AddModelError(errorItem.Key, errorItem.Errors[0].ErrorMessage);
                }

            }
            return View(manageUserInfoEditViewModel);
        }
        #endregion

        #region+ AddUser
        // https://www.c-sharpcorner.com/article/asp-net-core-mvc-authentication-and-role-based-authorization-with-asp-net-core/
        public class ManageUserInfoAddViewModel
        {
            public string PhoneNumber { get; set; }
            [Required]
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }

            // https://stackoverflow.com/questions/32987119/validate-model-on-specific-string-values
            // https://www.dotnettricks.com/learn/mvc/server-side-model-validation-in-mvc-razor
            public string UserName { get; set; }
            public string Id { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            public List<SelectListItem> ApplicationRoles { get; set; }

            [Required]
            [Display(Name = "Role")]
            public string ApplicationRoleId { get; set; }
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            ManageUserInfoAddViewModel model = new ManageUserInfoAddViewModel
            {
                ApplicationRoles = _roleManager.Roles.Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Id
                }).ToList()
            };
            return View(model);
        }

        // https://www.c-sharpcorner.com/article/asp-net-core-mvc-authentication-and-role-based-authorization-with-asp-net-core/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(ManageUserInfoAddViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email
                };

                ApplicationRole applicationRole = await _roleManager.FindByIdAsync(model.ApplicationRoleId);

                if (applicationRole != null)
                {
                    IdentityResult result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        IdentityResult roleResult = await _userManager.AddToRoleAsync(user, applicationRole.Name);

                        if (roleResult.Succeeded)
                        {
                            return RedirectToAction("UserList");
                        }
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return RedirectToAction("AddUser");
        }
        #endregion

        #region+ DetailUser
        public async Task<IActionResult> DetailUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID ''.");
            }

            var vm_user = new ApplicationUserAllViewModel()
            {
                Id = user.Id,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                UserName = user.UserName
            };

            return View(vm_user);
        }
        #endregion

        #region+ DeleteUser
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID ''.");
            }
            else
            {
                if (ModelState.IsValid)// ????
                {
                    try
                    {
                        var result = await _userManager.DeleteAsync(user);

                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError("", result.Errors.First().ToString());//????
                            return View();
                        }
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(UserList));
            }
        }
        #endregion
    }
}