using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PARK.APP.FIRST.Areas.VulnManage.Models.Vuln;
using PARK.APP.FIRST.Areas.VulnManage.Repositories;

namespace PARK.APP.FIRST.Areas.VulnManage.Controllers
{
    [Area("VulnManage")]
    public class TvulnsController : Controller
    {
        private readonly VulnDbContext _context;
        private readonly ITVulnRepository tVulnRepository;

        public TvulnsController(VulnDbContext context, ITVulnRepository repository)
        {
            tVulnRepository = repository;
            _context = context;
        }

        //public TvulnsController(ITVulnRepository tVulnRepository)
        //{
        //    this.tVulnRepository = tVulnRepository;
        //}

        //public TvulnsController(VulnDbContext context)
        //{
        //    _context = context;
        //}


        // GET: VulnManage/Tvulns
        public async Task<IActionResult> Index()
        {
            //var vulnDbContext = _context.Tvuln.Include(t => t.GroupSeqNavigation);
            //return View(await vulnDbContext.ToListAsync());

            // https://exceptionnotfound.net/using-dapper-asynchronously-in-asp-net-core-2-1/
            return View(await tVulnRepository.GetByVulnGroupSeq(202));
        }

        // GET: VulnManage/Tvulns/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tvuln = await _context.Tvuln
                .Include(t => t.GroupSeqNavigation)
                .FirstOrDefaultAsync(m => m.VulnSeq == id);
            if (tvuln == null)
            {
                return NotFound();
            }

            return View(tvuln);
        }

        // GET: VulnManage/Tvulns/Create
        public IActionResult Create()
        {
            ViewData["GroupSeq"] = new SelectList(_context.TvulnGroup, "GroupSeq", "GroupType");
            return View();
        }

        // POST: VulnManage/Tvulns/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VulnSeq,GroupSeq,ManualYn,AutoYn,ManageId,ClientStandardId,VulnName,SortOrder,RuleYn,Rate,Score,ApplyTime,Detail,DetailPath,Judgement,Effect,Remedy,RemedyPath,Refrrence,ParserContents,OrgParserContents,ApplyTarget,UseYn,ExceptCd,ExceptTermType,ExceptTermFr,ExceptTermTo,ExceptReason,ExceptUserId,ExceptDt,CreateUserId,CreateDt,UpdateUserId,UpdateDt,Vulgroup,Vulno,RemedyDetail,Overview,ManagementVulnYn")] Tvuln tvuln)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tvuln);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GroupSeq"] = new SelectList(_context.TvulnGroup, "GroupSeq", "GroupType", tvuln.GroupSeq);
            return View(tvuln);
        }

        // GET: VulnManage/Tvulns/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tvuln = await _context.Tvuln.FindAsync(id);
            if (tvuln == null)
            {
                return NotFound();
            }
            ViewData["GroupSeq"] = new SelectList(_context.TvulnGroup, "GroupSeq", "GroupType", tvuln.GroupSeq);
            return View(tvuln);
        }

        // POST: VulnManage/Tvulns/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("VulnSeq,GroupSeq,ManualYn,AutoYn,ManageId,ClientStandardId,VulnName,SortOrder,RuleYn,Rate,Score,ApplyTime,Detail,DetailPath,Judgement,Effect,Remedy,RemedyPath,Refrrence,ParserContents,OrgParserContents,ApplyTarget,UseYn,ExceptCd,ExceptTermType,ExceptTermFr,ExceptTermTo,ExceptReason,ExceptUserId,ExceptDt,CreateUserId,CreateDt,UpdateUserId,UpdateDt,Vulgroup,Vulno,RemedyDetail,Overview,ManagementVulnYn")] Tvuln tvuln)
        {
            if (id != tvuln.VulnSeq)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tvuln);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TvulnExists(tvuln.VulnSeq))
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
            ViewData["GroupSeq"] = new SelectList(_context.TvulnGroup, "GroupSeq", "GroupType", tvuln.GroupSeq);
            return View(tvuln);
        }

        // GET: VulnManage/Tvulns/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tvuln = await _context.Tvuln
                .Include(t => t.GroupSeqNavigation)
                .FirstOrDefaultAsync(m => m.VulnSeq == id);
            if (tvuln == null)
            {
                return NotFound();
            }

            return View(tvuln);
        }

        // POST: VulnManage/Tvulns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var tvuln = await _context.Tvuln.FindAsync(id);
            _context.Tvuln.Remove(tvuln);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TvulnExists(long id)
        {
            return _context.Tvuln.Any(e => e.VulnSeq == id);
        }
    }
}
