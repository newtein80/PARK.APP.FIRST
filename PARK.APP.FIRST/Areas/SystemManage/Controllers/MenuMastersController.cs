using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jQWidgets.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PARK.APP.FIRST.Areas.SystemManage.Models.Menu;
using PARK.APP.FIRST.Data;

namespace PARK.APP.FIRST.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class MenuMastersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MenuMastersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SystemManage/MenuMasters
        public async Task<IActionResult> Index()
        {
            return View(await _context.MenuMaster.ToListAsync());
        }

        #region+ ServerSide Paging, Filtering, Sorting
        // https://github.com/jqwidgets/Grid.AspNetCore.Mvc
        public class JSONData
        {
            public List<MenuMaster> MenuMasters
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

        // https://github.com/jqwidgets/Grid.AspNetCore.Mvc
        [HttpPost]
        public string GetPageData(string jsonData)
        {
            JToken token = JObject.Parse(jsonData);
            //JObject token = JObject.Parse(jsonData);

            List<JToken> filterGroups = token.SelectToken("filterGroups").Children().ToList();
            int pageSize = (int)token.SelectToken("pagesize");
            int pageNum = (int)token.SelectToken("pagenum");
            string sortField = (string)token.SelectToken("sortdatafield");
            string sortOrder = (string)token.SelectToken("sortorder");
            int count = 0;
            List<MenuMaster> employees = new List<MenuMaster>();
            List<MenuMaster> allEmployees = _context.MenuMaster.ToList();
            if (sortField != "")
            {
                if (sortOrder == "asc")
                {              
                    allEmployees = (from p in allEmployees
                                    orderby (p.GetType().GetProperty(sortField).GetValue(p))
                                 select p).ToList();

                }
                else if (sortOrder == "desc")
                {
                    allEmployees = (from p in allEmployees
                                    orderby (p.GetType().GetProperty(sortField).GetValue(p)) descending
                                    select p).ToList();
                }
            }
            if (filterGroups.Count > 0)
            {
                List<MenuMaster> filteredEmployees = allEmployees;
                for (int j = 0; j < filterGroups.Count; j++)
                {
                    List<JToken> filters = filterGroups[j].SelectToken("filters").Children().ToList();

                    List<MenuMaster> filterGroup = filteredEmployees;
                    List<MenuMaster> filterGroupResult = new List<MenuMaster>();
                    for (int i = 0; i < filters.Count; i++)
                    {
                        string filterLabel = (string)filters[i].SelectToken("label");
                        string filterValue = (string)filters[i].SelectToken("value");
                        string filterDataField = (string)filters[i].SelectToken("field");
                        string filterCondition = (string)filters[i].SelectToken("condition");
                        string filterType = (string)filters[i].SelectToken("type");
                        string filterOperator = (string)filters[i].SelectToken("operator");

                        List<MenuMaster> currentResult = new List<MenuMaster>();

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
                    filteredEmployees = filterGroupResult;
                }
                allEmployees = filteredEmployees;
             }
                 
            for (int i = pageNum * pageSize; i < allEmployees.Count && count < pageSize; i++)
            {
                employees.Add(allEmployees[i]);
                count++;
            }
            JSONData data = new JSONData();
            data.TotalRecords = allEmployees.Count;
            data.MenuMasters = employees;
            return JsonConvert.SerializeObject(data);
        }
        #endregion

        #region+ Export File
        [HttpPost]
        public FileContentResult ExportData(FormPostBack data)
        {
            return ExportHelper.ExportData(data);
        }
        #endregion

        #region+ Edit Cell (Not Working)
        [HttpPost]
        public bool CellEditData(string jsonData)
        {
            MenuMaster employee = (MenuMaster)JsonConvert.DeserializeObject(jsonData, typeof(MenuMaster));
            for (int i = 0; i < _context.MenuMaster.ToList().Count; i++)
            {
                MenuMaster currentEmployee = _context.MenuMaster.ToList()[i];
                if (currentEmployee.MenuIdentity == employee.MenuIdentity)
                {
                    currentEmployee.MenuName = employee.MenuName;
                    currentEmployee.CreatedDate = currentEmployee.CreatedDate;
                    currentEmployee.CssClass = currentEmployee.CssClass;
                    currentEmployee.MenuAction = currentEmployee.MenuAction;
                    currentEmployee.MenuArea = currentEmployee.MenuArea;
                    currentEmployee.MenuController = currentEmployee.MenuController;
                    currentEmployee.MenuId = currentEmployee.MenuId;
                    currentEmployee.Parent_MenuId = currentEmployee.Parent_MenuId;
                    currentEmployee.Sort_Order = currentEmployee.Sort_Order;
                    currentEmployee.User_Auth = currentEmployee.User_Auth;
                    currentEmployee.User_Duty = currentEmployee.User_Duty;
                    currentEmployee.User_Roll = currentEmployee.User_Roll;
                    currentEmployee.Use_Yn = currentEmployee.Use_Yn;

                    return true;
                }
            }
            return false;
        }
        #endregion

        // GET: SystemManage/MenuMasters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuMaster = await _context.MenuMaster
                .FirstOrDefaultAsync(m => m.MenuIdentity == id);
            if (menuMaster == null)
            {
                return NotFound();
            }

            return View(menuMaster);
        }

        // GET: SystemManage/MenuMasters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SystemManage/MenuMasters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MenuIdentity,MenuId,MenuName,Parent_MenuId,User_Roll,User_Auth,User_Duty,MenuArea,MenuController,MenuAction,Use_Yn,CssClass,Sort_Order,CreatedDate")] MenuMaster menuMaster)
        {
            if (ModelState.IsValid)
            {
                _context.Add(menuMaster);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(menuMaster);
        }

        // GET: SystemManage/MenuMasters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuMaster = await _context.MenuMaster.FindAsync(id);
            if (menuMaster == null)
            {
                return NotFound();
            }
            return View(menuMaster);
        }

        // POST: SystemManage/MenuMasters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MenuIdentity,MenuId,MenuName,Parent_MenuId,User_Roll,User_Auth,User_Duty,MenuArea,MenuController,MenuAction,Use_Yn,CssClass,Sort_Order,CreatedDate")] MenuMaster menuMaster)
        {
            if (id != menuMaster.MenuIdentity)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(menuMaster);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuMasterExists(menuMaster.MenuIdentity))
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
            return View(menuMaster);
        }

        // GET: SystemManage/MenuMasters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuMaster = await _context.MenuMaster
                .FirstOrDefaultAsync(m => m.MenuIdentity == id);
            if (menuMaster == null)
            {
                return NotFound();
            }

            return View(menuMaster);
        }

        // POST: SystemManage/MenuMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var menuMaster = await _context.MenuMaster.FindAsync(id);
            _context.MenuMaster.Remove(menuMaster);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MenuMasterExists(int id)
        {
            return _context.MenuMaster.Any(e => e.MenuIdentity == id);
        }
    }
}
