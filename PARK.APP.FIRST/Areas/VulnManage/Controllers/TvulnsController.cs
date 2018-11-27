using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PARK.APP.FIRST.Areas.VulnManage.Models.Vuln;
using PARK.APP.FIRST.Areas.VulnManage.Repositories;
using PARK.APP.FIRST.Data;
using PARK.APP.FIRST.Services;

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

        public async Task<IActionResult> Index_01()
        {
            //query.Skip(skip).Take(pageSize).ToList();
            var vulnDbContext = _context.Tvuln.Include(t => t.GroupSeqNavigation).Skip(1).Take(10);
            return View(await vulnDbContext.ToListAsync());
        }

        // https://stackoverflow.com/questions/23536299/mvc-5-multiple-models-in-a-single-view
        // https://stackoverflow.com/questions/4764011/multiple-models-in-a-view
        public class ViewModels
        {
            public List<PageVulns> PageVulns { get; set; }
            public PageDefault PageDefault { get; set; }
            public int PageListTotalCount { get; set; }
        }

        public class PageVulns
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

        public class PageDefault
        {
            public string Theme { get; set; }
            public string Setting { get; set; }

            public PageDefault()
            {
                Theme = "metro";
                Setting = "Controller Setting";
            }
        }

        public IActionResult Index_02()
        {
            // https://exceptionnotfound.net/using-dapper-asynchronously-in-asp-net-core-2-1/
            var pageVulns = new List<PageVulns>();

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
                    // Column 명이 맞지않는 컬럼은 보이지 않는다.
                    //vulns = handler.ReadToList<Tvuln>().ToList();

                    //pageVulns = handler.ReadToList<PageVulns>().Select(u => new PageVulns
                    //{
                    //    GROUP_SEQ = u.GROUP_SEQ,
                    //    VULN_SEQ = u.VULN_SEQ,
                    //    CREATE_USER_ID = u.CREATE_USER_ID,
                    //    MANAGE_ID = u.MANAGE_ID,
                    //    SORT_ORDER = u.SORT_ORDER,
                    //    UPDATE_DT = u.UPDATE_DT,
                    //    VULGROUP = u.VULGROUP,
                    //    VULNO = u.VULNO,
                    //    VULN_NAME = u.VULN_NAME
                    //}).ToList();

                    pageVulns = handler.ReadToList<PageVulns>().ToList();
                    
                });

            int outputParamValue = (int)outputParam?.Value;
            ViewModels viewModels = new ViewModels
            {
                PageVulns = pageVulns,
                PageDefault = new PageDefault(),
                PageListTotalCount = outputParamValue
            };

            return View(viewModels);
        }

        // google : asp.net core jqxgrid dapper
        // https://stackoverflow.com/questions/49872246/understanding-async-await-using-dapper-repositories-in-asp-net-core
        [HttpGet]
        public IActionResult Index_04(string id)
        {
            // https://exceptionnotfound.net/using-dapper-asynchronously-in-asp-net-core-2-1/
            var pageVulns = new List<PageVulns>();

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
                    pageVulns = handler.ReadToList<PageVulns>().ToList();

                });

            int outputParamValue = (int)outputParam?.Value;
            ViewModels viewModels = new ViewModels
            {
                PageVulns = pageVulns,
                PageDefault = new PageDefault(),
                PageListTotalCount = outputParamValue
            };

            return View(viewModels);
        }

        [HttpPost]
        public string Index_04(string id, string jsonData)
        {
            JToken token = JObject.Parse(jsonData);

            List<JToken> filterGroups = token.SelectToken("filterGroups").Children().ToList();
            int pageSize = (int)token.SelectToken("pagesize");
            int pageNum = (int)token.SelectToken("pagenum");
            string sortField = (string)token.SelectToken("sortdatafield");
            string sortOrder = (string)token.SelectToken("sortorder");
            int count = 0;

            //List<Tvuln> vulns = new List<Tvuln>();
            List<PageVulns> vulns = new List<PageVulns>();

            // 일단 Entity Framework 로 가져오는 부분을 바꿔야함!!
            // List<Tvuln> allVulns = _context.Tvuln.ToList();
            List<PageVulns> allVulns = _context.Tvuln.Select(r => new PageVulns
            {
                GROUP_SEQ = r.GroupSeq,
                VULN_SEQ = r.VulnSeq,
                CREATE_USER_ID = r.CreateUserId,
                MANAGE_ID = r.ManageId,
                SORT_ORDER = (long)r.SortOrder,
                UPDATE_DT = Convert.ToDateTime(r.UpdateDt),
                VULGROUP = 998,
                VULNO = "a",
                VULN_NAME = r.VulnName
            }).ToList();

            #region+ Sorting, Searching, Paging..--> 이부분을 Procedure 사용으로 바꿔서 Index_05 생성
            if (sortField != "")
            {
                if (sortOrder == "asc")
                {
                    allVulns = (from p in allVulns
                                orderby (p.GetType().GetProperty(sortField).GetValue(p))
                                select p).ToList();

                }
                else if (sortOrder == "desc")
                {
                    allVulns = (from p in allVulns
                                orderby (p.GetType().GetProperty(sortField).GetValue(p)) descending
                                select p).ToList();
                }
            }
            if (filterGroups.Count > 0)
            {
                //List<Tvuln> filteredVulns = allVulns;
                List<PageVulns> filteredVulns = allVulns;

                for (int j = 0; j < filterGroups.Count; j++)
                {
                    List<JToken> filters = filterGroups[j].SelectToken("filters").Children().ToList();

                    //List<Tvuln> filterGroup = filteredVulns;
                    //List<Tvuln> filterGroupResult = new List<Tvuln>();
                    List<PageVulns> filterGroup = filteredVulns;
                    List<PageVulns> filterGroupResult = new List<PageVulns>();

                    for (int i = 0; i < filters.Count; i++)
                    {
                        string filterLabel = (string)filters[i].SelectToken("label");
                        string filterValue = (string)filters[i].SelectToken("value");
                        string filterDataField = (string)filters[i].SelectToken("field");
                        string filterCondition = (string)filters[i].SelectToken("condition");
                        string filterType = (string)filters[i].SelectToken("type");
                        string filterOperator = (string)filters[i].SelectToken("operator");

                        //List<Tvuln> currentResult = new List<Tvuln>();
                        List<PageVulns> currentResult = new List<PageVulns>();

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
                    filteredVulns = filterGroupResult;
                }
                allVulns = filteredVulns;
            }

            for (int i = pageNum * pageSize; i < allVulns.Count && count < pageSize; i++)
            {
                vulns.Add(allVulns[i]);
                count++;
            }
            #endregion

            //PagedResults<Tvuln> data = new PagedResults<Tvuln>
            //{
            //    TotalCount = allVulns.Count,
            //    Items = vulns
            //};

            ViewModels viewModels = new ViewModels
            {
                PageVulns = vulns,
                PageDefault = new PageDefault(),
                PageListTotalCount = allVulns.Count
            };
            return JsonConvert.SerializeObject(viewModels);
        }















        public class ViewModels05
        {
            public List<PageVuln05> PageVuln05s { get; set; }
            public PageDefault05 PageDefault05s { get; set; }
            public int PageListTotalCount { get; set; }
        }

        public class PageVuln05
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

        public class PageDefault05
        {
            public string Theme { get; set; }
            public string Setting { get; set; }

            public PageDefault05()
            {
                Theme = "metro";
                Setting = "Controller Setting";
            }
        }


        // google : asp.net core jqxgrid dapper
        // https://stackoverflow.com/questions/49872246/understanding-async-await-using-dapper-repositories-in-asp-net-core
        [HttpGet]
        public IActionResult Index_05(string id)
        {
            // https://exceptionnotfound.net/using-dapper-asynchronously-in-asp-net-core-2-1/
            var pageVulns = new List<PageVuln05>();

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
                    pageVulns = handler.ReadToList<PageVuln05>().ToList();

                });

            int outputParamValue = (int)outputParam?.Value;
            ViewModels05 viewModel05s = new ViewModels05
            {
                PageVuln05s = pageVulns,
                PageDefault05s = new PageDefault05(),
                PageListTotalCount = outputParamValue
            };

            return View(viewModel05s);
        }

        [HttpPost]
        public string Index_05(string id, string jsonData)
        {
            JToken token = JObject.Parse(jsonData);

            List<JToken> filterGroups = token.SelectToken("filterGroups").Children().ToList();
            int pageSize = (int)token.SelectToken("pagesize");
            int pageNum = (int)token.SelectToken("pagenum");
            string sortField = (string)token.SelectToken("sortdatafield");
            string sortOrder = (string)token.SelectToken("sortorder");
            int count = 0;

            //List<Tvuln> vulns = new List<Tvuln>();
            List<PageVuln05> vulns = new List<PageVuln05>();

            // 일단 Entity Framework 로 가져오는 부분을 바꿔야함!!
            // List<Tvuln> allVulns = _context.Tvuln.ToList();
            List<PageVuln05> allVulns = _context.Tvuln.Select(r => new PageVuln05
            {
                GROUP_SEQ = r.GroupSeq,
                VULN_SEQ = r.VulnSeq,
                CREATE_USER_ID = r.CreateUserId,
                MANAGE_ID = r.ManageId,
                SORT_ORDER = (long)r.SortOrder,
                UPDATE_DT = Convert.ToDateTime(r.UpdateDt),
                VULGROUP = 998,
                VULNO = "a",
                VULN_NAME = r.VulnName
            }).ToList();

            #region+ Sorting, Searching, Paging..--> 이부분을 Procedure 사용으로 바꿔서 Index_05 생성
            if (sortField != "")
            {
                if (sortOrder == "asc")
                {
                    allVulns = (from p in allVulns
                                orderby (p.GetType().GetProperty(sortField).GetValue(p))
                                select p).ToList();

                }
                else if (sortOrder == "desc")
                {
                    allVulns = (from p in allVulns
                                orderby (p.GetType().GetProperty(sortField).GetValue(p)) descending
                                select p).ToList();
                }
            }
            if (filterGroups.Count > 0)
            {
                //List<Tvuln> filteredVulns = allVulns;
                List<PageVuln05> filteredVulns = allVulns;

                for (int j = 0; j < filterGroups.Count; j++)
                {
                    List<JToken> filters = filterGroups[j].SelectToken("filters").Children().ToList();

                    //List<Tvuln> filterGroup = filteredVulns;
                    //List<Tvuln> filterGroupResult = new List<Tvuln>();
                    List<PageVuln05> filterGroup = filteredVulns;
                    List<PageVuln05> filterGroupResult = new List<PageVuln05>();

                    for (int i = 0; i < filters.Count; i++)
                    {
                        string filterLabel = (string)filters[i].SelectToken("label");
                        string filterValue = (string)filters[i].SelectToken("value");
                        string filterDataField = (string)filters[i].SelectToken("field");
                        string filterCondition = (string)filters[i].SelectToken("condition");
                        string filterType = (string)filters[i].SelectToken("type");
                        string filterOperator = (string)filters[i].SelectToken("operator");

                        //List<Tvuln> currentResult = new List<Tvuln>();
                        List<PageVuln05> currentResult = new List<PageVuln05>();

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
                    filteredVulns = filterGroupResult;
                }
                allVulns = filteredVulns;
            }

            for (int i = pageNum * pageSize; i < allVulns.Count && count < pageSize; i++)
            {
                vulns.Add(allVulns[i]);
                count++;
            }
            #endregion

            //PagedResults<Tvuln> data = new PagedResults<Tvuln>
            //{
            //    TotalCount = allVulns.Count,
            //    Items = vulns
            //};

            ViewModels05 viewModel05s = new ViewModels05
            {
                PageVuln05s = vulns,
                PageDefault05s = new PageDefault05(),
                PageListTotalCount = allVulns.Count
            };
            return JsonConvert.SerializeObject(viewModel05s);
        }














































        [HttpGet]
        public async Task<IActionResult> Index_03()
        {
            var vulnDbContext = _context.Tvuln.Include(t => t.GroupSeqNavigation).Skip(1).Take(10);
            return View(await vulnDbContext.ToListAsync());
        }

        //An unhandled exception occurred while processing the request.
        //ArgumentNullException: Value cannot be null.
        //Parameter name: s
        // -> HttpGet 없을경우..
        [HttpPost]
        public string Index_03(string jsonData)
        {
            JToken token = JObject.Parse(jsonData);

            List<JToken> filterGroups = token.SelectToken("filterGroups").Children().ToList();
            int pageSize = (int)token.SelectToken("pagesize");
            int pageNum = (int)token.SelectToken("pagenum");
            string sortField = (string)token.SelectToken("sortdatafield");
            string sortOrder = (string)token.SelectToken("sortorder");
            int count = 0;
            List<Tvuln> vulns = new List<Tvuln>();
            List<Tvuln> allVulns = _context.Tvuln.ToList();
            if (sortField != "")
            {
                if (sortOrder == "asc")
                {
                    allVulns = (from p in allVulns
                                    orderby (p.GetType().GetProperty(sortField).GetValue(p))
                                    select p).ToList();

                }
                else if (sortOrder == "desc")
                {
                    allVulns = (from p in allVulns
                                    orderby (p.GetType().GetProperty(sortField).GetValue(p)) descending
                                    select p).ToList();
                }
            }
            if (filterGroups.Count > 0)
            {
                List<Tvuln> filteredVulns = allVulns;
                for (int j = 0; j < filterGroups.Count; j++)
                {
                    List<JToken> filters = filterGroups[j].SelectToken("filters").Children().ToList();

                    List<Tvuln> filterGroup = filteredVulns;
                    List<Tvuln> filterGroupResult = new List<Tvuln>();
                    for (int i = 0; i < filters.Count; i++)
                    {
                        string filterLabel = (string)filters[i].SelectToken("label");
                        string filterValue = (string)filters[i].SelectToken("value");
                        string filterDataField = (string)filters[i].SelectToken("field");
                        string filterCondition = (string)filters[i].SelectToken("condition");
                        string filterType = (string)filters[i].SelectToken("type");
                        string filterOperator = (string)filters[i].SelectToken("operator");

                        List<Tvuln> currentResult = new List<Tvuln>();

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
                    filteredVulns = filterGroupResult;
                }
                allVulns = filteredVulns;
            }

            for (int i = pageNum * pageSize; i < allVulns.Count && count < pageSize; i++)
            {
                vulns.Add(allVulns[i]);
                count++;
            }
            PagedResults<Tvuln> data = new PagedResults<Tvuln>
            {
                TotalCount = allVulns.Count,
                Items = vulns
            };
            return JsonConvert.SerializeObject(data);
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
