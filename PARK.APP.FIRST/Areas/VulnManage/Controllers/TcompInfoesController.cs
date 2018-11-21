using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PARK.APP.FIRST.Areas.VulnManage.Models.Vuln;

namespace PARK.APP.FIRST.Areas.VulnManage.Controllers
{
    [Area("VulnManage")]
    public class TcompInfoesController : Controller
    {
        private readonly VulnDbContext _context;

        public TcompInfoesController(VulnDbContext context)
        {
            _context = context;
        }

        #region+ ServerSide Paging, Sorting, Filtering
        /// <summary>
        /// jqx-grid source class
        /// </summary>
        public class JSONData
        {
            /// <summary>
            /// source-root="grid_TcompInfo_List"
            /// </summary>
            public List<TcompInfo> grid_TcompInfo_List { get; set; }

            /// <summary>
            /// source-total-records="TotalRecords"
            /// </summary>
            public int TotalRecords { get; set; }
        }

        /// <summary>
        /// source-url : @Url.Action("GetPageData","TcompInfoes")
        /// </summary>
        /// <param name="jsonData">jqx에서 넘어오는 값</param>
        /// <returns></returns>
        [HttpPost]
        public string GetPageData(string jsonData)
        {
            JToken token = JObject.Parse(jsonData);

            List<JToken> filterGroups = token.SelectToken("filterGroups").Children().ToList();
            int pageSize = (int)token.SelectToken("pagesize");
            int pageNum = (int)token.SelectToken("pagenum");
            string sortField = (string)token.SelectToken("sortdatafield");
            string sortOrder = (string)token.SelectToken("sortorder");

            int count = 0;

            // grid 에 최종적으로 binding 될 대상 Model 의 List<>
            List<TcompInfo> grid_TcompInfo = new List<TcompInfo>();
            // Database 에 있는 Model 대상의 table 의 전체 데이터
            List<TcompInfo> all_TcompInfo = _context.TcompInfo.ToList();

            #region+ Filter 및 Sorting 설정 - 고정(손댈 곳이 없음)
            // 1. Sorting 하여 정렬
            if (sortField != "")
            {
                if (sortOrder == "asc")
                {
                    all_TcompInfo = (from p in all_TcompInfo
                                     orderby (p.GetType().GetProperty(sortField).GetValue(p))
                                    select p).ToList();

                }
                else if (sortOrder == "desc")
                {
                    all_TcompInfo = (from p in all_TcompInfo
                                     orderby (p.GetType().GetProperty(sortField).GetValue(p)) descending
                                    select p).ToList();
                }
            }

            // 2. Filter 검색
            if (filterGroups.Count > 0)
            {
                // 2-1. 정렬된 기준으로 Filter 할 대상 변수를 선언
                List<TcompInfo> filtered_TcompInfo = all_TcompInfo;
                for (int j = 0; j < filterGroups.Count; j++)
                {
                    List<JToken> filters = filterGroups[j].SelectToken("filters").Children().ToList();

                    // 2-2. 정렬된 기준으로 Filter 할 대상을 재선언
                    List<TcompInfo> filterGroup = filtered_TcompInfo;

                    // 2-3. 필터 된 결과를 담을 변수를 선언
                    List<TcompInfo> filterGroupResult = new List<TcompInfo>();
                    for (int i = 0; i < filters.Count; i++)
                    {
                        string filterLabel = (string)filters[i].SelectToken("label");
                        string filterValue = (string)filters[i].SelectToken("value");
                        string filterDataField = (string)filters[i].SelectToken("field");
                        string filterCondition = (string)filters[i].SelectToken("condition");
                        string filterType = (string)filters[i].SelectToken("type");
                        string filterOperator = (string)filters[i].SelectToken("operator");

                        List<TcompInfo> currentResult = new List<TcompInfo>();

                        switch (filterCondition)
                        {
                            case "NOT_EMPTY":
                                currentResult = (from p in filterGroup
                                                 where ((p.GetType().GetProperty(filterDataField).GetValue(p)) != null)
                                                 select p).ToList();
                                break;
                            case "NOT_NULL":
                                currentResult = (from p in filterGroup
                                                 where ((p.GetType().GetProperty(filterDataField).GetValue(p)).ToString() != "")
                                                 select p).ToList();
                                break;
                            case "NULL":
                                currentResult = (from p in filterGroup
                                                 where ((p.GetType().GetProperty(filterDataField).GetValue(p)) == null)
                                                 select p).ToList();
                                break;
                            case "EMPTY":
                                currentResult = (from p in filterGroup
                                                 where ((p.GetType().GetProperty(filterDataField).GetValue(p)).ToString() == "")
                                                 select p).ToList();
                                break;
                            case "CONTAINS_CASE_SENSITIVE":
                                currentResult = (from p in filterGroup
                                                 where ((p.GetType().GetProperty(filterDataField).GetValue(p)).ToString().Contains(filterValue))
                                                 select p).ToList();
                                break;
                            case "CONTAINS":
                                currentResult = (from p in filterGroup
                                                 where ((p.GetType().GetProperty(filterDataField).GetValue(p)).ToString().IndexOf(filterValue, StringComparison.CurrentCultureIgnoreCase) != -1)
                                                 select p).ToList();
                                break;
                            case "DOES_NOT_CONTAIN_CASE_SENSITIVE":
                                currentResult = (from p in filterGroup
                                                 where (!(p.GetType().GetProperty(filterDataField).GetValue(p)).ToString().Contains(filterValue))
                                                 select p).ToList();
                                break;
                            case "DOES_NOT_CONTAIN":
                                currentResult = (from p in filterGroup
                                                 where ((p.GetType().GetProperty(filterDataField).GetValue(p)).ToString().IndexOf(filterValue, StringComparison.CurrentCultureIgnoreCase) == -1)
                                                 select p).ToList();
                                break;
                            case "EQUAL_CASE_SENSITIVE":
                                currentResult = (from p in filterGroup
                                                 where ((p.GetType().GetProperty(filterDataField).GetValue(p)).ToString() == filterValue)
                                                 select p).ToList();
                                break;
                            case "EQUAL":
                                currentResult = (from p in filterGroup
                                                 where ((p.GetType().GetProperty(filterDataField).GetValue(p)).ToString().IndexOf(filterValue, StringComparison.CurrentCultureIgnoreCase) == 0)
                                                 select p).ToList();
                                break;
                            case "NOT_EQUAL_CASE_SENSITIVE":
                                currentResult = (from p in filterGroup
                                                 where ((p.GetType().GetProperty(filterDataField).GetValue(p)).ToString() != filterValue)
                                                 select p).ToList();
                                break;
                            case "NOT_EQUAL":
                                currentResult = (from p in filterGroup
                                                 where ((p.GetType().GetProperty(filterDataField).GetValue(p)).ToString().IndexOf(filterValue, StringComparison.CurrentCultureIgnoreCase) != 0)
                                                 select p).ToList();
                                break;
                            case "GREATER_THAN":
                                currentResult = (from p in filterGroup
                                                 where (float.Parse(p.GetType().GetProperty(filterDataField).GetValue(p).ToString()) > float.Parse(filterValue))
                                                 select p).ToList();
                                break;
                            case "LESS_THAN":
                                currentResult = (from p in filterGroup
                                                 where (float.Parse(p.GetType().GetProperty(filterDataField).GetValue(p).ToString()) < float.Parse(filterValue))
                                                 select p).ToList();
                                break;
                            case "GREATER_THAN_OR_EQUAL":
                                currentResult = (from p in filterGroup
                                                 where (float.Parse(p.GetType().GetProperty(filterDataField).GetValue(p).ToString()) >= float.Parse(filterValue))
                                                 select p).ToList();
                                break;
                            case "LESS_THAN_OR_EQUAL":
                                currentResult = (from p in filterGroup
                                                 where (float.Parse(p.GetType().GetProperty(filterDataField).GetValue(p).ToString()) <= float.Parse(filterValue))
                                                 select p).ToList();
                                break;
                            case "STARTS_WITH_CASE_SENSITIVE":
                                currentResult = (from p in filterGroup
                                                 where ((p.GetType().GetProperty(filterDataField).GetValue(p)).ToString().StartsWith(filterValue))
                                                 select p).ToList();
                                break;
                            case "STARTS_WITH":
                                currentResult = (from p in filterGroup
                                                 where ((p.GetType().GetProperty(filterDataField).GetValue(p)).ToString().StartsWith(filterValue, StringComparison.CurrentCultureIgnoreCase))
                                                 select p).ToList();
                                break;
                            case "ENDS_WITH_CASE_SENSITIVE":
                                currentResult = (from p in filterGroup
                                                 where ((p.GetType().GetProperty(filterDataField).GetValue(p)).ToString().EndsWith(filterValue))
                                                 select p).ToList();
                                break;
                            case "ENDS_WITH":
                                currentResult = (from p in filterGroup
                                                 where ((p.GetType().GetProperty(filterDataField).GetValue(p)).ToString().EndsWith(filterValue, StringComparison.CurrentCultureIgnoreCase))
                                                 select p).ToList();
                                break;
                        }

                        if (filterOperator == "or")
                        {
                            filterGroupResult.AddRange(currentResult);
                        }
                        else
                        {
                            filterGroup = currentResult;
                            filterGroupResult = currentResult;
                        }
                    }

                    // 2-4. 최종 필터 된 결과 2-1 에서 선언한 변수에 할당
                    filtered_TcompInfo = filterGroupResult;
                }

                // 2-5. 최종 결과에 할당
                all_TcompInfo = filtered_TcompInfo;
            }
            #endregion

            // 3. Paging index, size 에 맞춰 grid 에 최종적으로 binding 될 대상 Model 의 List<> 에 담음
            for (int i = pageNum * pageSize; i < all_TcompInfo.Count && count < pageSize; i++)
            {
                grid_TcompInfo.Add(all_TcompInfo[i]);
                count++;
            }

            // 4. return 할 Json object(jqx-grid source class) 에 담음
            JSONData data = new JSONData();
            data.TotalRecords = all_TcompInfo.Count;
            data.grid_TcompInfo_List = grid_TcompInfo;

            // 5. Serialize 해서 return
            return JsonConvert.SerializeObject(data);
        }
        #endregion

        // GET: VulnManage/TcompInfoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.TcompInfo.ToListAsync());
        }

        // GET: VulnManage/TcompInfoes/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tcompInfo = await _context.TcompInfo
                .FirstOrDefaultAsync(m => m.CompSeq == id);
            if (tcompInfo == null)
            {
                return NotFound();
            }

            return View(tcompInfo);
        }

        // GET: VulnManage/TcompInfoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VulnManage/TcompInfoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompSeq,CompName,CompDescription,CompRef,StandardYear,DiagType,ConfirmYn,UseYn,CreateUserId,CreateDt,UpdateUserId,UpdateDt,CompDetailGubun")] TcompInfo tcompInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tcompInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tcompInfo);
        }

        // GET: VulnManage/TcompInfoes/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tcompInfo = await _context.TcompInfo.FindAsync(id);
            if (tcompInfo == null)
            {
                return NotFound();
            }
            return View(tcompInfo);
        }

        // POST: VulnManage/TcompInfoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CompSeq,CompName,CompDescription,CompRef,StandardYear,DiagType,ConfirmYn,UseYn,CreateUserId,CreateDt,UpdateUserId,UpdateDt,CompDetailGubun")] TcompInfo tcompInfo)
        {
            if (id != tcompInfo.CompSeq)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tcompInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TcompInfoExists(tcompInfo.CompSeq))
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
            return View(tcompInfo);
        }

        // GET: VulnManage/TcompInfoes/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tcompInfo = await _context.TcompInfo
                .FirstOrDefaultAsync(m => m.CompSeq == id);
            if (tcompInfo == null)
            {
                return NotFound();
            }

            return View(tcompInfo);
        }

        // POST: VulnManage/TcompInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var tcompInfo = await _context.TcompInfo.FindAsync(id);
            _context.TcompInfo.Remove(tcompInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TcompInfoExists(long id)
        {
            return _context.TcompInfo.Any(e => e.CompSeq == id);
        }
    }
}
