using System;
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

        [BindProperty]
        public ManageUserInfoChangeViewModel Input { get; set; }

        // https://www.c-sharpcorner.com/article/asp-net-core-mvc-authentication-and-role-based-authorization-with-asp-net-core/
        public class ManageUserInfoChangeViewModel
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

        [HttpGet]
        public IActionResult AddUser()
        {
            ManageUserInfoChangeViewModel model = new ManageUserInfoChangeViewModel
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
        public async Task<IActionResult> AddUser(ManageUserInfoChangeViewModel model)
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
    }
}