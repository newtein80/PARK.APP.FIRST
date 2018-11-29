using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PARK.APP.FIRST.Filters
{
    public class GetGridFilterItems
    {
        public List<T> GetFilterItems<T>(string jsonData, List<T> allItems, ref int count)
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
    }
}
