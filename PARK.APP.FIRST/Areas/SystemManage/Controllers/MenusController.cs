using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PARK.APP.FIRST.Areas.SystemManage.Models.Menu;
using PARK.APP.FIRST.Data;
using Newtonsoft.Json;

namespace PARK.APP.FIRST.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    //[Route("Menus")]
    public class MenusController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MenusController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SystemManage/Menus
        public async Task<IActionResult> Index()
        {
            return View(await _context.TMenu.ToListAsync());
        }


        public class JSONData
        {
            public List<TMenu> Menus
            {
                get;
                set;
            }
            public int TotalRecords
            {
                get;
                set;
            }
        }

        public string GetList()
        {
            var m_Menus = _context.TMenu.ToList();
            JSONData data = new JSONData();
            data.Menus = m_Menus;
            data.TotalRecords = m_Menus.Count;

            return JsonConvert.SerializeObject(data);
        }

        // GET: SystemManage/Menus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tMenu = await _context.TMenu
                .FirstOrDefaultAsync(m => m.MenuSeq == id);
            if (tMenu == null)
            {
                return NotFound();
            }

            return View(tMenu);
        }

        // GET: SystemManage/Menus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SystemManage/Menus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MenuSeq,MenuName,UpperMenuSeq,PgmId,MenuDescription,ImagePath,UseYn,SortOrder")] TMenu tMenu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tMenu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tMenu);
        }

        // GET: SystemManage/Menus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tMenu = await _context.TMenu.FindAsync(id);
            if (tMenu == null)
            {
                return NotFound();
            }
            return View(tMenu);
        }

        // POST: SystemManage/Menus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MenuSeq,MenuName,UpperMenuSeq,PgmId,MenuDescription,ImagePath,UseYn,SortOrder")] TMenu tMenu)
        {
            if (id != tMenu.MenuSeq)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tMenu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TMenuExists(tMenu.MenuSeq))
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
            return View(tMenu);
        }

        // GET: SystemManage/Menus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tMenu = await _context.TMenu
                .FirstOrDefaultAsync(m => m.MenuSeq == id);
            if (tMenu == null)
            {
                return NotFound();
            }

            return View(tMenu);
        }

        // POST: SystemManage/Menus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tMenu = await _context.TMenu.FindAsync(id);
            _context.TMenu.Remove(tMenu);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TMenuExists(int id)
        {
            return _context.TMenu.Any(e => e.MenuSeq == id);
        }
    }
}
