using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PARK.APP.FIRST.Areas.SystemManage.Models.System;
using PARK.APP.FIRST.Areas.SystemManage.Repositories;
using PARK.APP.FIRST.Areas.VulnManage.Models.Vuln;
using PARK.APP.FIRST.Services;

namespace PARK.APP.FIRST.Areas.VulnManage.Controllers
{
    [Area("VulnManage")]
    public class VulnManageController : Controller
    {
        private readonly VulnDbContext vulnDbContext;
        private readonly ISystemCodeRepository systemCodeRepository;

        public VulnManageController(VulnDbContext vulnDbContext, ISystemCodeRepository systemCodeRepository)
        {
            this.vulnDbContext = vulnDbContext;
            this.systemCodeRepository = systemCodeRepository;
        }

        public class PageDefault
        {
            public string Theme { get; set; }
            public string Setting { get; set; }
            public string Title { get; set; }

            public PageDefault()
            {
                Theme = "metro";
                Setting = "Controller Setting";
                Title = "VulnManage";
            }
        }

        public class VulnModel : Tvuln
        {

        }

        public class VulnSearchModel
        {
            [Display(Name = "점검 구분")]
            public string Diag_type { get; set; }

            public List<TcommonCode> Diag_types { get; set; }

            [Display(Name = "점검 유형")]
            public string Diag_kind { get; set; }

            [Display(Name = "컴플라이언스명")]
            public string Comp_name { get; set; }
            public Int64 Comp_seq { get; set; }

            [Display(Name = "항목그룹명")]
            public string Group_name { get; set; }
            public Int64 Group_seq { get; set; }

            [Display(Name = "항목명")]
            public string Vuln_name { get; set; }

            [Display(Name = "관리ID")]
            public string Manage_id { get; set; }

            [Display(Name = "위험도")]
            public string Rate { get; set; }

            [Display(Name = "위험수준")]
            public string Score { get; set; }

            [Display(Name = "예외여부")]
            public string Exception_yn { get; set; }

            public VulnSearchModel()
            {
                Diag_type = "";
                Diag_kind = "";
                Comp_name = "";
                Comp_seq = 0;
                Group_name = "";
                Group_seq = 0;
                Vuln_name = "";
                Manage_id = "";
                Rate = "";
                Score = "";
                Exception_yn = "";
                Diag_types = new List<TcommonCode>();
            }
        }

        public class VulnViewModel
        {
            public List<VulnModel> vulnModels { get; set; }
            public VulnSearchModel vulnSearchModel { get; set; }
            public PageDefault pageDefault { get; set; }
            public int vulnModesTotalCount { get; set; }
        }

        [HttpGet]
        public IActionResult VulnList()
        {
            var _ddlDiagType = systemCodeRepository.GetCommonCodeDropDownList("DIAG_TYPE", "");

            var vulnSearchModel = new VulnSearchModel();

            vulnSearchModel.Diag_types = _ddlDiagType;

            var initVulnModels = new List<VulnModel>
            {
                new VulnModel()
            };

            VulnViewModel vulnViewModel = new VulnViewModel
            {
                pageDefault = new PageDefault(),
                vulnModels = initVulnModels,
                vulnModesTotalCount = 0,
                vulnSearchModel = vulnSearchModel
            };

            return View(vulnViewModel);
        }

        [HttpPost]
        public IActionResult VulnList(VulnViewModel vulnViewModel)
        {
            var filter_vulnModels = new List<VulnModel>();

            DbParameter outputParam = null;
            vulnDbContext.LoadStoredProc("dbo.SP_VULN_LIST_02")
                .WithSqlParam("gubun", "")
                .WithSqlParam("diag_type", "")
                .WithSqlParam("diag_kind", "")
                .WithSqlParam("comp_seq", 0)
                .WithSqlParam("comp_name", vulnViewModel.vulnSearchModel.Comp_name ?? "")
                .WithSqlParam("group_seq", 0)
                .WithSqlParam("group_name", vulnViewModel.vulnSearchModel.Group_name ?? "")
                .WithSqlParam("vuln_seq", 0)
                .WithSqlParam("vuln_name", vulnViewModel.vulnSearchModel.Vuln_name ?? "")
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
                    filter_vulnModels = handler.ReadToList<VulnModel>().ToList();

                });

            int outputParamValue = (int)outputParam?.Value;

            if (1 > outputParamValue)
            {
                filter_vulnModels.Add(new VulnModel());
            }

            vulnViewModel.vulnSearchModel.Diag_types = systemCodeRepository.GetCommonCodeDropDownList("DIAG_TYPE", "");

            VulnViewModel rtn_vulnViewModel = new VulnViewModel
            {
                pageDefault = new PageDefault(),
                vulnModels = filter_vulnModels,
                vulnModesTotalCount = outputParamValue,
                vulnSearchModel = vulnViewModel.vulnSearchModel
            };

            return View(rtn_vulnViewModel);
        }

        [HttpPost]
        public string GetGridVulnData(string jsonData, VulnSearchModel vulnSearchModel)
        {
            var filter_vulnModels = new List<VulnModel>();

            DbParameter outputParam = null;
            vulnDbContext.LoadStoredProc("dbo.SP_VULN_LIST_02")
                .WithSqlParam("gubun", "")
                .WithSqlParam("diag_type", "")
                .WithSqlParam("diag_kind", "")
                .WithSqlParam("comp_seq", 0)
                .WithSqlParam("comp_name", vulnSearchModel.Comp_name ?? "")
                .WithSqlParam("group_seq", 0)
                .WithSqlParam("group_name", vulnSearchModel.Group_name ?? "")
                .WithSqlParam("vuln_seq", 0)
                .WithSqlParam("vuln_name", vulnSearchModel.Vuln_name ?? "")
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
                    filter_vulnModels = handler.ReadToList<VulnModel>().ToList();

                });

            int outputParamValue = (int)outputParam?.Value;

            VulnViewModel rtn_vulnViewModel = new VulnViewModel
            {
                pageDefault = new PageDefault(),
                vulnModels = FilterItems(jsonData, filter_vulnModels, ref outputParamValue),
                vulnModesTotalCount = outputParamValue,
                vulnSearchModel = vulnSearchModel
            };

            return JsonConvert.SerializeObject(rtn_vulnViewModel);
        }

        [HttpPost]
        public string GetGridVulnData_Dapper(string jsonData, VulnSearchModel vulnSearchModel)
        {
            var param = new DynamicParameters();
            param.Add("@gubun", "");
            param.Add("@diag_type", "");
            param.Add("@diag_kind", "");
            param.Add("@comp_seq", 0);
            param.Add("@comp_name", vulnSearchModel.Comp_name ?? "");
            param.Add("@group_seq", 0);
            param.Add("@group_name", vulnSearchModel.Group_name ?? "");
            param.Add("@vuln_seq", 0);
            param.Add("@vuln_name", vulnSearchModel.Vuln_name ?? "");
            param.Add("@manage_id", "");
            param.Add("@rate", 0);
            param.Add("@score", 0);
            param.Add("@use_yn", "");
            param.Add("@exception_yn", "");
            param.Add("@user_id", "");
            param.Add("@sort_field", "");
            param.Add("@is_desc", 1);
            param.Add("@pagesize", 1000000);
            param.Add("@pageindex", 1);
            //p.Add("@UserID", userId, DbType.String, null, 100);
            param.Add("@allCount", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 50);

            // 테이블 한개 리스트
            var listTable = DapperHelper.GetList<VulnModel>("SP_VULN_LIST_02", param).ToList();

            // 메인 + 서브 모델
            //var Table = SqlHelper.MultiPleGetList<Schema_User, Address>(SqlHelper.localDB.ToString(), "sp_GetUser_List", param);

            int resultParam = param.Get<int>("@allCount");

            VulnViewModel rtn_vulnViewModel = new VulnViewModel
            {
                pageDefault = new PageDefault(),
                vulnModels = FilterItems(jsonData, listTable, ref resultParam),
                vulnModesTotalCount = resultParam,
                vulnSearchModel = vulnSearchModel
            };

            return JsonConvert.SerializeObject(rtn_vulnViewModel);
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

        public class VulnCreateModel
        {
            public List<TcommonCode> tCommonCodes { get; set; }
            public List<TcompInfo> tCompInfos { get; set; }
            public VulnCreateInfo tVulnCreateInfo { get; set; }
        }

        public class VulnCreateInfo : Tvuln
        {
            public long CompSeq { get; set; }
            public string DiagType { get; set; }
            public string DiagKind { get; set; }
        }

        [HttpGet]
        public IActionResult VulnCreate()
        {
            string m_UseYn = "Y";
            string[] arrSearchCommonCodes = { "RATE", "SCORE", "EXCEPT_CD", "USE_YN", "DIAG_TYPE" };
            List<TcommonCode> _ddlCommonCodes = systemCodeRepository.GetCommonCodeByArray(arrSearchCommonCodes, m_UseYn);

            var vulnCreateModel = new VulnCreateModel
            {
                tCommonCodes = _ddlCommonCodes,
                tCompInfos = vulnDbContext.TcompInfo.ToList(),
                tVulnCreateInfo = new VulnCreateInfo()
            };

            return View(vulnCreateModel);
        }

        [HttpPost]
        public IActionResult VulnCreate(VulnCreateModel vulnCreateModel)
        {
            return RedirectToAction("VulnCreate");
        }

        public JsonResult GetCompByDiagType(string i_Diagtype)
        {
            List<TcompInfo> tcompInfos_by_diagtype = new List<TcompInfo>();

            tcompInfos_by_diagtype = vulnDbContext.TcompInfo.Where(x => x.DiagType == i_Diagtype).ToList();

            tcompInfos_by_diagtype.Insert(0, new TcompInfo { CompSeq = 0, CompName = "--select !! --" });

            return Json(new SelectList(tcompInfos_by_diagtype, "CompSeq", "CompName"));
        }

        public JsonResult GetDiagKindByComp(int i_CompSeq)
        {
            List<TvulnGroup> tdiagkind_by_compseq = new List<TvulnGroup>();

            tdiagkind_by_compseq = vulnDbContext.TvulnGroup.Where(x => x.CompSeq == i_CompSeq && x.GroupType == "K").Distinct().ToList();

            tdiagkind_by_compseq.Insert(0, new TvulnGroup { GroupSeq = 0, GroupName = "--select !! --" });

            return Json(new SelectList(tdiagkind_by_compseq, "GroupSeq", "GroupName"));
        }

        public JsonResult GetVulnGroupByDiagKind(int i_GroupSeq)
        {
            List<TvulnGroup> tvulngroups_by_compseq = new List<TvulnGroup>();

            tvulngroups_by_compseq = vulnDbContext.TvulnGroup.Where(x => x.UpperSeq == i_GroupSeq && x.GroupType == "G").ToList();

            tvulngroups_by_compseq.Insert(0, new TvulnGroup { GroupSeq = 0, GroupName = "--select !! --" });

            return Json(new SelectList(tvulngroups_by_compseq, "GroupSeq", "GroupName"));
        }

        
    }
}