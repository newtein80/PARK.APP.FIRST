using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PARK.APP.FIRST.Areas.VulnManage.Models.Vuln;
using PARK.APP.FIRST.Services;

namespace PARK.APP.FIRST.Areas.VulnManage.Controllers
{
    [Area("VulnManage")]
    public class TcompManageController : Controller
    {
        private readonly VulnDbContext vulnDbContext;

        public TcompManageController(VulnDbContext vulnDbContext)
        {
            this.vulnDbContext = vulnDbContext;
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

        public class PageCompModel
        {
            public Int64 COMP_SEQ { get; set; }
            public string COMP_NAME { get; set; }
            public string DIAG_TYPE { get; set; }
            public string COMP_DESCRIPTION { get; set; }
            public string CONFIRM_YN { get; set; }
            public string USE_YN { get; set; }
            public string CREATE_USER_ID { get; set; }
            public DateTime CREATE_DT { get; set; }
            public string COMP_DETAIL_GUBUN { get; set; }
        }

        public class PageCompSearchModel
        {
            [Display(Name = "COMPLIANCE NAME")]
            public string Comp_name { get; set; }
            [Display(Name = "DIAG. TYPE")]
            public string Diag_type { get; set; }
            [Display(Name = "Usable")]
            public string Use_yn { get; set; }
            [Display(Name = "CREATE USER")]
            public string Create_user_id { get; set; }

            public PageCompSearchModel()
            {
                Comp_name = "";
                Diag_type = "";
                Use_yn = "";
                Create_user_id = "";
            }
        }

        public class PageViewModel
        {
            public List<PageCompModel> _PageCompModels { get; set; }
            public PageCompSearchModel _PageCompSearch { get; set; }
            public PageDefault _PageDefault { get; set; }
            public int _PageListTotalCount { get; set; }
        }

        [HttpGet]
        public IActionResult Index_21()
        {
            #region+ 페이지 최초 진입이기 때문에 필요없음. search 데이터 또는 기타 데이터 초기값 설정만 하면 됨. grid 는 나중에 jqx에서 가져옴
            //var pageComps = new List<PageCompModel>();
            //DbParameter outputParam = null;

            //vulnDbContext.LoadStoredProc("dbo.SP_COMP_LIST")
            //    .WithSqlParam("gubun", "")
            //    .WithSqlParam("diag_type", "")
            //    .WithSqlParam("comp_name", "")
            //    .WithSqlParam("standard_year", "")
            //    .WithSqlParam("use_yn", "")
            //    .WithSqlParam("create_user_id", "")
            //    .WithSqlParam("sort_field", "")
            //    .WithSqlParam("is_desc", 1)
            //    .WithSqlParam("pagesize", 10)
            //    .WithSqlParam("pageindex", 1)
            //    .WithSqlParam("allCount", (dbParam) =>
            //    {
            //        dbParam.Direction = System.Data.ParameterDirection.Output;
            //        dbParam.DbType = System.Data.DbType.Int32;
            //        outputParam = dbParam;
            //    })
            //    .ExecuteStoredProc((handler) =>
            //    {
            //        pageComps = handler.ReadToList<PageCompModel>().ToList();

            //    });

            //int outputParamValue = (int)outputParam?.Value;

            //if(1 > outputParamValue)
            //{
            //    pageComps.Add(new PageCompModel());
            //}

            //PageViewModel pageViewModel = new PageViewModel
            //{
            //    _PageCompModels = pageComps,//new List<PageCompModel>(),//pageComps,
            //    _PageCompSearch = new PageCompSearchModel(),
            //    _PageDefault = new PageDefault(),
            //    _PageListTotalCount = outputParamValue
            //};

            //return View(pageViewModel);
            #endregion

            var pageComps = new List<PageCompModel>();

            pageComps.Add(new PageCompModel());

            PageViewModel pageViewModel = new PageViewModel
            {
                _PageCompModels = pageComps,//new List<PageCompModel>(),//pageComps,
                _PageCompSearch = new PageCompSearchModel(),
                _PageDefault = new PageDefault(),
                _PageListTotalCount = 0
            };

            return View(pageViewModel);
        }

        [HttpPost]
        public IActionResult Index_21(PageViewModel pageViewModel)
        {
            var pageComps = new List<PageCompModel>();

            DbParameter outputParam = null;

            vulnDbContext.LoadStoredProc("dbo.SP_COMP_LIST")
                .WithSqlParam("gubun", "")
                .WithSqlParam("diag_type", pageViewModel._PageCompSearch.Diag_type ?? "")
                .WithSqlParam("comp_name", pageViewModel._PageCompSearch.Comp_name ?? "")
                .WithSqlParam("standard_year", "")
                .WithSqlParam("use_yn", pageViewModel._PageCompSearch.Use_yn ?? "")
                .WithSqlParam("create_user_id", pageViewModel._PageCompSearch.Create_user_id ?? "")
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
                    // 지정한 모델클래스의 각 명칭과 프로시저결과의 각 컬럼명이 같아야 한다.
                    pageComps = handler.ReadToList<PageCompModel>().ToList();

                });

            int outputParamValue = (int)outputParam?.Value;

            if (1 > outputParamValue)
            {
                pageComps.Add(new PageCompModel());
            }

            PageViewModel rtn_pageViewModel = new PageViewModel
            {
                _PageCompModels = pageComps,
                _PageCompSearch = pageViewModel._PageCompSearch,
                _PageDefault = new PageDefault(),
                _PageListTotalCount = outputParamValue
            };

            return View(rtn_pageViewModel);
        }

        [HttpPost]
        public string GetGridCompData(string jsonData, PageCompSearchModel searchModel)
        {
            var compModels = new List<PageCompModel>();

            DbParameter outputParam = null;

            vulnDbContext.LoadStoredProc("dbo.SP_COMP_LIST")
                .WithSqlParam("gubun", "")
                .WithSqlParam("diag_type", searchModel.Diag_type ?? "")
                .WithSqlParam("comp_name", searchModel.Comp_name ?? "")
                .WithSqlParam("standard_year", "")
                .WithSqlParam("use_yn", searchModel.Use_yn ?? "")
                .WithSqlParam("create_user_id", searchModel.Create_user_id ?? "")
                .WithSqlParam("sort_field", "")
                .WithSqlParam("is_desc", 1)
                .WithSqlParam("pagesize", 10000)
                .WithSqlParam("pageindex", 1)
                .WithSqlParam("allCount", (dbParam) =>
                {
                    dbParam.Direction = System.Data.ParameterDirection.Output;
                    dbParam.DbType = System.Data.DbType.Int32;
                    outputParam = dbParam;
                })
                .ExecuteStoredProc((handler) =>
                {
                    // 지정한 모델클래스의 각 명칭과 프로시저결과의 각 컬럼명이 같아야 한다.
                    compModels = handler.ReadToList<PageCompModel>().ToList();

                });

            int outputParamValue = (int)outputParam?.Value;

            PageViewModel rtn_pageViewModel = new PageViewModel
            {
                _PageCompModels = FilterItems(jsonData, compModels, ref outputParamValue),//compModels,//
                _PageCompSearch = searchModel,
                _PageDefault = new PageDefault(),
                _PageListTotalCount = outputParamValue
            };

            return JsonConvert.SerializeObject(rtn_pageViewModel);
        }

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
    }
}