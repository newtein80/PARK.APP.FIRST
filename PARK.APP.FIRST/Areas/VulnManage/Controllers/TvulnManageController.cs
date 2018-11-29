using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PARK.APP.FIRST.Areas.VulnManage.Models.Vuln;
using PARK.APP.FIRST.Areas.VulnManage.Repositories;
using PARK.APP.FIRST.Services;

namespace PARK.APP.FIRST.Areas.VulnManage.Controllers
{
    [Area("VulnManage")]
    public class TvulnManageController : Controller
    {
        private readonly VulnDbContext _context;
        private readonly ITVulnRepository tVulnRepository;

        public TvulnManageController(VulnDbContext context, ITVulnRepository tVulnRepository)
        {
            _context = context;
            this.tVulnRepository = tVulnRepository;
        }

        #region+ Model Class (Index_11)
        public class ViewModel11
        {
            public List<PageVuln11> PageVuln11s { get; set; }
            public PageDefault11 PageDefault11s { get; set; }
            public int PageListTotalCount { get; set; }
            public PageSearchModel11 PageSearchModel11s { get; set; }
        }

        public class PageVuln11
        {
            public Int64 VULN_SEQ { get; set; }
            public string VULN_NAME { get; set; }
            public Int64 GROUP_SEQ { get; set; }
            public string MANAGE_ID { get; set; }
            public Int64 VULGROUP { get; set; }
            public string VULNO { get; set; }
            public DateTime UPDATE_DT { get; set; }
            public string CREATE_USER_ID { get; set; }
            public Int64 SORT_ORDER { get; set; }
        }

        public class PageDefault11
        {
            public string Theme { get; set; }
            public string Setting { get; set; }

            public PageDefault11()
            {
                Theme = "metro";
                Setting = "Controller Setting";
            }
        }

        public class PageSearchModel11
        {
            [Display(Name = "VULN NAME")]
            public string Vuln_name { get; set; }
            [Display(Name = "COMPLIANCE NAME")]
            public string Comp_name { get; set; }

            public PageSearchModel11()
            {
                Vuln_name = "";
                Comp_name = "";
            }
        }
        #endregion

        #region+ Index_06
        // google : asp.net core jqxgrid dapper
        // https://stackoverflow.com/questions/49872246/understanding-async-await-using-dapper-repositories-in-asp-net-core
        [HttpGet]
        public IActionResult Index_11()
        {
            // https://exceptionnotfound.net/using-dapper-asynchronously-in-asp-net-core-2-1/
            var pageVulns = new List<PageVuln11>();

            DbParameter outputParam = null;
            // exec dbo.SP_VULN_LIST '', '', '', 0, '', 0, '', 0, '', '', 0, 0, '', '', '', '', 1, 10, 1, 1
            _context.LoadStoredProc("dbo.SP_VULN_LIST")
                .WithSqlParam("gubun", "")
                .WithSqlParam("diag_type", "")
                .WithSqlParam("diag_kind", "")
                .WithSqlParam("comp_seq", 0)
                .WithSqlParam("comp_name", "")
                .WithSqlParam("group_seq", 0)
                .WithSqlParam("group_name", "")
                .WithSqlParam("vuln_seq", 0)
                .WithSqlParam("vuln_name", "")
                .WithSqlParam("manage_id", "")
                .WithSqlParam("rate", 0)
                .WithSqlParam("score", 0)
                .WithSqlParam("use_yn", "")
                .WithSqlParam("exception_yn", "")
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
                    pageVulns = handler.ReadToList<PageVuln11>().ToList();

                });

            int outputParamValue = (int)outputParam?.Value;

            ViewModel11 viewModel06s = new ViewModel11
            {
                PageVuln11s = pageVulns,
                PageDefault11s = new PageDefault11(),
                PageListTotalCount = outputParamValue,
                PageSearchModel11s = new PageSearchModel11()//초기값
            };

            return View(viewModel06s);
        }

        [HttpPost]
        public IActionResult Index_11(ViewModel11 viewModels)
        {
            // https://exceptionnotfound.net/using-dapper-asynchronously-in-asp-net-core-2-1/
            var pageVulns = new List<PageVuln11>();

            DbParameter outputParam = null;
            // exec dbo.SP_VULN_LIST '', '', '', 0, '', 0, '', 0, '', '', 0, 0, '', '', '', '', 1, 10, 1, 1
            _context.LoadStoredProc("dbo.SP_VULN_LIST")
                .WithSqlParam("gubun", "")
                .WithSqlParam("diag_type", "")
                .WithSqlParam("diag_kind", "")
                .WithSqlParam("comp_seq", 0)
                .WithSqlParam("comp_name", "")
                .WithSqlParam("group_seq", 0)
                .WithSqlParam("group_name", "")
                .WithSqlParam("vuln_seq", 0)
                .WithSqlParam("vuln_name", viewModels.PageSearchModel11s.Vuln_name ?? "")
                .WithSqlParam("manage_id", "")
                .WithSqlParam("rate", 0)
                .WithSqlParam("score", 0)
                .WithSqlParam("use_yn", "")
                .WithSqlParam("exception_yn", "")
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
                    pageVulns = handler.ReadToList<PageVuln11>().ToList();

                });

            int outputParamValue = (int)outputParam?.Value;
            ViewModel11 viewModel11s = new ViewModel11
            {
                PageVuln11s = pageVulns,
                PageDefault11s = new PageDefault11(),
                PageListTotalCount = outputParamValue,
                PageSearchModel11s = viewModels.PageSearchModel11s//중요!!!!!!!!! 검색 submit 후 의 값
            };

            return View(viewModel11s);
        }

        [HttpPost]
        public string GetData11(string jsonData, PageSearchModel11 viewModels)
        {
            #region+ EF 사용
            //int count = 0;

            //List<PageVuln11> allVulns = _context.Tvuln.Select(r => new PageVuln11
            //{
            //    GROUP_SEQ = r.GroupSeq,
            //    VULN_SEQ = r.VulnSeq,
            //    CREATE_USER_ID = r.CreateUserId,
            //    MANAGE_ID = r.ManageId,
            //    SORT_ORDER = (long)r.SortOrder,
            //    UPDATE_DT = Convert.ToDateTime(r.UpdateDt),
            //    VULGROUP = 998,
            //    VULNO = "a",
            //    VULN_NAME = r.VulnName
            //}).ToList();

            //ViewModel11 viewModel11s = new ViewModel11
            //{
            //    PageVuln11s = FilterItems(jsonData, allVulns, ref count),
            //    PageDefault11s = new PageDefault11(),
            //    PageListTotalCount = count,
            //    PageSearchModel11s = new PageSearchModel11()
            //};
            //return JsonConvert.SerializeObject(viewModel11s);
            #endregion

            #region+ Test.01 - SP 사용 (하지만 위와 다를바 없음. SP로 전체를 가져옴)
            // https://exceptionnotfound.net/using-dapper-asynchronously-in-asp-net-core-2-1/
            var pageVulns = new List<PageVuln11>();

            DbParameter outputParam = null;
            // exec dbo.SP_VULN_LIST '', '', '', 0, '', 0, '', 0, '', '', 0, 0, '', '', '', '', 1, 10, 1, 1
            _context.LoadStoredProc("dbo.SP_VULN_LIST")
                .WithSqlParam("gubun", "")
                .WithSqlParam("diag_type", "")
                .WithSqlParam("diag_kind", "")
                .WithSqlParam("comp_seq", 0)
                .WithSqlParam("comp_name", "")
                .WithSqlParam("group_seq", 0)
                .WithSqlParam("group_name", "")
                .WithSqlParam("vuln_seq", 0)
                .WithSqlParam("vuln_name", viewModels.Vuln_name ?? "")
                .WithSqlParam("manage_id", "")
                .WithSqlParam("rate", 0)
                .WithSqlParam("score", 0)
                .WithSqlParam("use_yn", "")
                .WithSqlParam("exception_yn", "")
                .WithSqlParam("user_id", "")
                .WithSqlParam("sort_field", "")
                .WithSqlParam("is_desc", 1)
                .WithSqlParam("pagesize", 1000000)
                .WithSqlParam("pageindex", 1)
                .WithSqlParam("allCount", (dbParam) =>
                {
                    dbParam.Direction = System.Data.ParameterDirection.Output;
                    dbParam.DbType = System.Data.DbType.Int32;
                    outputParam = dbParam;
                })
                .ExecuteStoredProc((handler) =>
                {
                    pageVulns = handler.ReadToList<PageVuln11>().ToList();

                });

            int outputParamValue = (int)outputParam?.Value;

            ViewModel11 viewModel11s = new ViewModel11
            {
                PageVuln11s = FilterItems(jsonData, pageVulns, ref outputParamValue),
                PageDefault11s = new PageDefault11(),
                PageListTotalCount = outputParamValue,
                PageSearchModel11s = new PageSearchModel11()
            };
            return JsonConvert.SerializeObject(viewModel11s);
            #endregion
        }
        #endregion

        #region+ 중요!!!! 나중에 Filter 로 옮겨야 함!!!!!!!!!!!!!!!!
        public static List<T> FilterItems<T>(string jsonData, List<T> allItems, ref int count)
        {
            int pageSize = 10;
            int pageNum = 0;
            if (jsonData != "{}")
            {

                JToken token = JObject.Parse(jsonData);

                List<JToken> filterGroups = token.SelectToken("filterGroups").Children().ToList();
                pageSize = (int)token.SelectToken("pagesize");
                pageNum = (int)token.SelectToken("pagenum");

                string sortField = (string)token.SelectToken("sortdatafield") ?? "";
                string sortOrder = (string)token.SelectToken("sortorder") ?? "";

                //filter all items of class T in the List<T> allItems
                if (sortField != "")
                {
                    if (sortOrder == "asc")
                    {
                        allItems = (from p in allItems
                                    orderby (p.GetType().GetProperty(sortField).GetValue(p))
                                    select p).ToList();

                    }
                    else if (sortOrder == "desc")
                    {
                        allItems = (from p in allItems
                                    orderby (p.GetType().GetProperty(sortField).GetValue(p)) descending
                                    select p).ToList();
                    }
                }
                if (filterGroups.Count > 0)
                {
                    List<T> filteredItems = allItems;
                    for (int j = 0; j < filterGroups.Count; j++)
                    {
                        List<JToken> filters = filterGroups[j].SelectToken("filters").Children().ToList();

                        List<T> filterGroup = filteredItems;
                        List<T> filterGroupResult = new List<T>();
                        for (int i = 0; i < filters.Count; i++)
                        {
                            string filterLabel = (string)filters[i].SelectToken("label");
                            string filterValue = (string)filters[i].SelectToken("value");
                            string filterDataField = (string)filters[i].SelectToken("field");
                            string filterCondition = (string)filters[i].SelectToken("condition");
                            string filterType = (string)filters[i].SelectToken("type");
                            string filterOperator = (string)filters[i].SelectToken("operator");

                            List<T> currentResult = new List<T>();

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
                        filteredItems = filterGroupResult;
                    }
                    allItems = filteredItems;

                }
            }
            count = allItems.Count;
            List<T> items = new List<T>();
            int startpageindex = pageNum * pageSize;

            int pagecount = (startpageindex + pageSize <= count) ? pageSize : count - startpageindex;
            items = allItems.GetRange(startpageindex, pagecount);
            return items;
        }
        #endregion

        [HttpGet]
        public ActionResult PopupTvulnManageCreate()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult PopupTvulnManageCreate(Tvuln tvuln)
        {
            return View();
        }

        [HttpGet]
        public IActionResult PopupTvulnManageEdit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tvuln = _context.Tvuln.Find(id);
            if (tvuln == null)
            {
                return NotFound();
            }

            //ViewData["GroupSeq"] = new SelectList(_context.TvulnGroup, "GroupSeq", "GroupType", tvuln.GroupSeq);

            return PartialView("PopupTvulnManageEdit", tvuln);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PopupTvulnManageEditAsync(long id, [Bind("VulnSeq,GroupSeq,ManualYn,AutoYn,ManageId,ClientStandardId,VulnName,SortOrder,RuleYn,Rate,Score,ApplyTime,Detail,DetailPath,Judgement,Effect,Remedy,RemedyPath,Refrrence,ParserContents,OrgParserContents,ApplyTarget,UseYn,ExceptCd,ExceptTermType,ExceptTermFr,ExceptTermTo,ExceptReason,ExceptUserId,ExceptDt,CreateUserId,CreateDt,UpdateUserId,UpdateDt,Vulgroup,Vulno,RemedyDetail,Overview,ManagementVulnYn")] Tvuln tvuln)
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
                return PartialView("PopupTvulnManageEdit", tvuln);
            }

            return PartialView("PopupTvulnManageEdit", tvuln);
        }

        private bool TvulnExists(long id)
        {
            return _context.Tvuln.Any(e => e.VulnSeq == id);
        }
    }
}