using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PARK.APP.FIRST.Areas.VulnManage.Models.Vuln;

namespace PARK.APP.FIRST.Areas.VulnManage.Controllers
{
    [Area("VulnManage")]
    public class TvulnGroupsController : Controller
    {
        private readonly VulnDbContext _context;

        public TvulnGroupsController(VulnDbContext context)
        {
            _context = context;
        }

        // GET: VulnManage/TvulnGroups
        public async Task<IActionResult> Index()
        {
            var vulnDbContext = _context.TvulnGroup.Include(t => t.CompSeqNavigation);
            return View(await vulnDbContext.ToListAsync());
        }

        // GET: VulnManage/TvulnGroups/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tvulnGroup = await _context.TvulnGroup
                .Include(t => t.CompSeqNavigation)
                .FirstOrDefaultAsync(m => m.GroupSeq == id);
            if (tvulnGroup == null)
            {
                return NotFound();
            }

            return View(tvulnGroup);
        }

        // GET: VulnManage/TvulnGroups/Create
        public IActionResult Create()
        {
            ViewData["CompSeq"] = new SelectList(_context.TcompInfo, "CompSeq", "CompName");
            return View();
        }

        // POST: VulnManage/TvulnGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GroupSeq,UpperSeq,Level,GroupType,CompSeq,DiagKind,DiagTerm,GroupId,GroupName,DiagTool,SortOrder,CreateUserId,CreateDt,UpdateUserId,UpdateDt")] TvulnGroup tvulnGroup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tvulnGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompSeq"] = new SelectList(_context.TcompInfo, "CompSeq", "CompName", tvulnGroup.CompSeq);
            return View(tvulnGroup);
        }

        // GET: VulnManage/TvulnGroups/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tvulnGroup = await _context.TvulnGroup.FindAsync(id);
            if (tvulnGroup == null)
            {
                return NotFound();
            }
            ViewData["CompSeq"] = new SelectList(_context.TcompInfo, "CompSeq", "CompName", tvulnGroup.CompSeq);
            return View(tvulnGroup);
        }

        // POST: VulnManage/TvulnGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("GroupSeq,UpperSeq,Level,GroupType,CompSeq,DiagKind,DiagTerm,GroupId,GroupName,DiagTool,SortOrder,CreateUserId,CreateDt,UpdateUserId,UpdateDt")] TvulnGroup tvulnGroup)
        {
            if (id != tvulnGroup.GroupSeq)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tvulnGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TvulnGroupExists(tvulnGroup.GroupSeq))
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
            ViewData["CompSeq"] = new SelectList(_context.TcompInfo, "CompSeq", "CompName", tvulnGroup.CompSeq);
            return View(tvulnGroup);
        }

        // GET: VulnManage/TvulnGroups/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tvulnGroup = await _context.TvulnGroup
                .Include(t => t.CompSeqNavigation)
                .FirstOrDefaultAsync(m => m.GroupSeq == id);
            if (tvulnGroup == null)
            {
                return NotFound();
            }

            return View(tvulnGroup);
        }

        // POST: VulnManage/TvulnGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var tvulnGroup = await _context.TvulnGroup.FindAsync(id);
            _context.TvulnGroup.Remove(tvulnGroup);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TvulnGroupExists(long id)
        {
            return _context.TvulnGroup.Any(e => e.GroupSeq == id);
        }
    }
}
