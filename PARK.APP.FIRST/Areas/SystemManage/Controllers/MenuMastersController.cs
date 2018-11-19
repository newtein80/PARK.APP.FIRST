using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PARK.APP.FIRST.Areas.SystemManage.Models.Menu;
using PARK.APP.FIRST.Data;

namespace PARK.APP.FIRST.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class MenuMastersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MenuMastersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SystemManage/MenuMasters
        public async Task<IActionResult> Index()
        {
            return View(await _context.MenuMaster.ToListAsync());
        }

        // GET: SystemManage/MenuMasters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuMaster = await _context.MenuMaster
                .FirstOrDefaultAsync(m => m.MenuIdentity == id);
            if (menuMaster == null)
            {
                return NotFound();
            }

            return View(menuMaster);
        }

        // GET: SystemManage/MenuMasters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SystemManage/MenuMasters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MenuIdentity,MenuId,MenuName,Parent_MenuId,User_Roll,User_Auth,User_Duty,MenuArea,MenuController,MenuAction,Use_Yn,CssClass,Sort_Order,CreatedDate")] MenuMaster menuMaster)
        {
            if (ModelState.IsValid)
            {
                _context.Add(menuMaster);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(menuMaster);
        }

        // GET: SystemManage/MenuMasters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuMaster = await _context.MenuMaster.FindAsync(id);
            if (menuMaster == null)
            {
                return NotFound();
            }
            return View(menuMaster);
        }

        // POST: SystemManage/MenuMasters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MenuIdentity,MenuId,MenuName,Parent_MenuId,User_Roll,User_Auth,User_Duty,MenuArea,MenuController,MenuAction,Use_Yn,CssClass,Sort_Order,CreatedDate")] MenuMaster menuMaster)
        {
            if (id != menuMaster.MenuIdentity)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(menuMaster);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuMasterExists(menuMaster.MenuIdentity))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(menuMaster);
        }

        // GET: SystemManage/MenuMasters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuMaster = await _context.MenuMaster
                .FirstOrDefaultAsync(m => m.MenuIdentity == id);
            if (menuMaster == null)
            {
                return NotFound();
            }

            return View(menuMaster);
        }

        // POST: SystemManage/MenuMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var menuMaster = await _context.MenuMaster.FindAsync(id);
            _context.MenuMaster.Remove(menuMaster);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MenuMasterExists(int id)
        {
            return _context.MenuMaster.Any(e => e.MenuIdentity == id);
        }
    }
}
