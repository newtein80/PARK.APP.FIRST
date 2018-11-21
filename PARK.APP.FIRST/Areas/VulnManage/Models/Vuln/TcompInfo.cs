using System;
using System.Collections.Generic;

namespace PARK.APP.FIRST.Areas.VulnManage.Models.Vuln
{
    public partial class TcompInfo
    {
        public TcompInfo()
        {
            TvulnGroup = new HashSet<TvulnGroup>();
        }

        public long CompSeq { get; set; }
        public string CompName { get; set; }
        public string CompDescription { get; set; }
        public string CompRef { get; set; }
        public string StandardYear { get; set; }
        public string DiagType { get; set; }
        public string ConfirmYn { get; set; }
        public string UseYn { get; set; }
        public string CreateUserId { get; set; }
        public DateTime? CreateDt { get; set; }
        public string UpdateUserId { get; set; }
        public DateTime? UpdateDt { get; set; }
        public string CompDetailGubun { get; set; }

        public ICollection<TvulnGroup> TvulnGroup { get; set; }
    }
}
