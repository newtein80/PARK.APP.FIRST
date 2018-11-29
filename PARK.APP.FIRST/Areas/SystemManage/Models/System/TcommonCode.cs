using System;
using System.Collections.Generic;

namespace PARK.APP.FIRST.Areas.SystemManage.Models.System
{
    public partial class TcommonCode
    {
        public string CodeType { get; set; }
        public string CodeTypeName { get; set; }
        public string CodeId { get; set; }
        public string CodeName { get; set; }
        public int? CodeVal { get; set; }
        public int? SortOrder { get; set; }
        public string UseYn { get; set; }
        public string CodeComment { get; set; }
        public string CreateUserId { get; set; }
        public DateTime? CreateDt { get; set; }
    }
}
