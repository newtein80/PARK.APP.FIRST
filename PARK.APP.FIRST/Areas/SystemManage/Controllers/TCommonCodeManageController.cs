using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PARK.APP.FIRST.Areas.SystemManage.Models.System;
using PARK.APP.FIRST.Services;

namespace PARK.APP.FIRST.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class TCommonCodeManageController : Controller
    {
        private readonly SystemDbContext _systemDbContext;

        public TCommonCodeManageController(SystemDbContext systemDbContext)
        {
            _systemDbContext = systemDbContext;
        }

        public class PageDefaultModel
        {
            public string Theme { get; set; }
            public string Setting { get; set; }

            public PageDefaultModel()
            {
                Theme = "metro";
                Setting = "Controller Setting";
            }
        }

        public class PageCommonCodeModel
        {
            public string CodeType { get; set; }
            public string CodeTypeName { get; set; }
            public string CodeId { get; set; }
            public string CodeName { get; set; }
            public int? CodeVal { get; set; }
            public string UseYn { get; set; }
            public string CodeComment { get; set; }
            public Int64 TEMP_SEQ { get; set; }
        }

        public class PageSearchMdoel
        {
            [Display(Name = "CODE TYPE")]
            public string CodeType { get; set; }
            [Display(Name = "CODE TYPE NAME")]
            public string CodeTypeName { get; set; }
            [Display(Name = "CODE ID")]
            public string CodeId { get; set; }
            [Display(Name = "CODE NAME")]
            public string CodeName { get; set; }
        }

        public class PageViewModel_CommonCode
        {
            public List<PageCommonCodeModel> _TcommonCodes { get; set; }
            public PageSearchMdoel _PageSearchMdoel { get; set; }
            public PageDefaultModel _PageDefault { get; set; }
            public int _PageListTotalCoutn { get; set; }
        }

        [HttpGet]
        public IActionResult CommonCodeIndex()
        {
            //var pageCommonCodes = new List<PageCommonCodeModel>();

            //pageCommonCodes.Add(new PageCommonCodeModel());

            var pageCommonCodes = new List<PageCommonCodeModel>
            {
                new PageCommonCodeModel()
            };

            PageViewModel_CommonCode pageViewModel_CommonCode = new PageViewModel_CommonCode
            {
                _PageDefault = new PageDefaultModel(),
                _PageListTotalCoutn = 0,
                _PageSearchMdoel = new PageSearchMdoel(),
                _TcommonCodes = pageCommonCodes
            };

            return View(pageViewModel_CommonCode);
        }

        [HttpPost]
        public IActionResult CommonCodeIndex(PageViewModel_CommonCode pageViewModel_CommonCode)
        {
            var pageCommonCodes = new List<PageCommonCodeModel>();

            DbParameter outputParam = null;

            _systemDbContext.LoadStoredProc("dbo.SP_COMMON_CODE_LIST")
                .WithSqlParam("gubun", "")
                .WithSqlParam("code_type", pageViewModel_CommonCode._PageSearchMdoel.CodeType ?? "")
                .WithSqlParam("code_type_name", pageViewModel_CommonCode._PageSearchMdoel.CodeTypeName ?? "")
                .WithSqlParam("code_id", pageViewModel_CommonCode._PageSearchMdoel.CodeId)
                .WithSqlParam("code_name", pageViewModel_CommonCode._PageSearchMdoel.CodeName ?? "")
                .WithSqlParam("code_val", "")
                .WithSqlParam("use_yn", "")
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
                    pageCommonCodes = handler.ReadToList<PageCommonCodeModel>().ToList();

                });

            int outputParamValue = (int)outputParam?.Value;

            if(1> outputParamValue)
            {
                pageCommonCodes.Add(new PageCommonCodeModel());
            }

            PageViewModel_CommonCode rtn_pageVieModel_CommonCode = new PageViewModel_CommonCode
            {
                _TcommonCodes = pageCommonCodes,
                _PageSearchMdoel = pageViewModel_CommonCode._PageSearchMdoel,
                _PageDefault = new PageDefaultModel(),
                _PageListTotalCoutn = outputParamValue
            };

            return View(rtn_pageVieModel_CommonCode);
        }

        [HttpPost]
        public string GetGridData_CommonCode(string jsonData, PageSearchMdoel searchModel)
        {
            var compModels = new List<PageCommonCodeModel>();

            DbParameter outputParam = null;

            _systemDbContext.LoadStoredProc("dbo.SP_COMMON_CODE_LIST")
                .WithSqlParam("gubun", "")
                .WithSqlParam("code_type", searchModel.CodeType ?? "")
                .WithSqlParam("code_type_name", searchModel.CodeTypeName ?? "")
                .WithSqlParam("code_id", searchModel.CodeId)
                .WithSqlParam("code_name", searchModel.CodeName ?? "")
                .WithSqlParam("code_val", "")
                .WithSqlParam("use_yn", "")
                .WithSqlParam("sort_field", "")
                .WithSqlParam("is_desc", 1)
                .WithSqlParam("pagesize", 100000)
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
                    compModels = handler.ReadToList<PageCommonCodeModel>().ToList();

                });

            int outputParamValue = (int)outputParam?.Value;

            PageViewModel_CommonCode rtn_pageViewModel = new PageViewModel_CommonCode
            {
                _TcommonCodes = FilterItems(jsonData, compModels, ref outputParamValue),//compModels,//
                _PageSearchMdoel = searchModel,
                _PageDefault = new PageDefaultModel(),
                _PageListTotalCoutn = outputParamValue
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


        public IActionResult CommonCodeCreate()
        {
            return View();
        }
    }
}