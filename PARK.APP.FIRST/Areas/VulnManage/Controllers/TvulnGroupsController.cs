using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PARK.APP.FIRST.Areas.VulnManage.Models.Vuln;
using PARK.APP.FIRST.Services;

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
        public async Task<IActionResult> IndexDefault()
        {
            //var vulnDbContext = _context.TvulnGroup.Include(t => t.CompSeqNavigation);
            //return View(await vulnDbContext.ToListAsync());


            DbParameter outputParam = null;
            //exec dbo.SP_VULN_GROUP_LIST '29', 'script','','','','','',1,10,1,0
            return View(await _context.LoadStoredProc("dbo.SP_VULN_GROUP_LIST")
               .WithSqlParam("comp_seq", 1)
               .WithSqlParam("diag_type", 1)
               .WithSqlParam("diag_kind", 1)
               .WithSqlParam("group_seq", 1)
               .WithSqlParam("group_name", 1)
               .WithSqlParam("user_id", 1)
               .WithSqlParam("sort_field", 1)
               .WithSqlParam("is_desc", 1)
               .WithSqlParam("pagesize", 1)
               .WithSqlParam("pageindex", 1)
               .WithSqlParam("allCount", 1)
               .WithSqlParam("allCount", (dbParam) =>
               {
                   dbParam.Direction = System.Data.ParameterDirection.Output;
                   dbParam.DbType = System.Data.DbType.Int32;
                   outputParam = dbParam;
               })
               .ExecuteStoredNonQueryAsync());
               //.ExecuteStoredProc((handler) =>
               //{
               //    var fooResults = handler.ReadToList<TvulnGroup>();
               //    // do something with your results.
               //});
        }

        public class FooDto
        {
            public Int64 GROUP_SEQ { get; set; }
            public string GROUP_NAME { get; set; }
            public string DIAG_KIND { get; set; }
            public string GROUP_ID { get; set; }
            public string DIAG_TOOL { get; set; }
            public DateTime CREATE_DT { get; set; }
            public Int64 COMP_SEQ { get; set; }
            public string DIAG_KIND_NAME { get; set; }
            public string DIAG_TOOL_NAME { get; set; }
        }

        public IActionResult Index()
        {
            var users = new List<FooDto>();

            DbParameter outputParam = null;

            // exec dbo.SP_VULN_GROUP_LIST '29', 'script','','','','','',1,10,1,0
            _context.LoadStoredProc("dbo.SP_VULN_GROUP_LIST")
                    .WithSqlParam("comp_seq", "29")
                    .WithSqlParam("diag_type", "SCRIPT")
                    .WithSqlParam("diag_kind", "")
                    .WithSqlParam("group_seq", "")
                    .WithSqlParam("group_name", "")
                    .WithSqlParam("user_id", "")
                    .WithSqlParam("sort_field", "")
                    .WithSqlParam("is_desc", 1)
                    .WithSqlParam("pagesize", 10)
                    .WithSqlParam("pageindex", 1)
                    .WithSqlParam("allCount", (dbParam) =>
                    {
                        dbParam.Direction = System.Data.ParameterDirection.Output;
                        dbParam.DbType = System.Data.DbType.Int32;
                        outputParam = dbParam;
                    })
                    .ExecuteStoredProc((handler) =>
                    {
                        users = handler.ReadToList<FooDto>().Select(u => new FooDto
                        {
                            GROUP_SEQ = u.GROUP_SEQ,
                            COMP_SEQ = u.COMP_SEQ,
                            CREATE_DT = u.CREATE_DT,
                            DIAG_KIND = u.DIAG_KIND,
                            DIAG_KIND_NAME = u.DIAG_KIND_NAME,
                            DIAG_TOOL = u.DIAG_TOOL,
                            DIAG_TOOL_NAME = u.DIAG_TOOL_NAME,
                            GROUP_ID = u.GROUP_ID,
                            GROUP_NAME = u.GROUP_NAME
                        }).ToList();
                    });

            int outputParamValue = (int)outputParam?.Value;

            return View(users);
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
