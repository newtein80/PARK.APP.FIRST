using System;
using System.Collections.Generic;

namespace PARK.APP.FIRST.Areas.VulnManage.Models.Vuln
{
    public partial class TvulnGroup
    {
        public TvulnGroup()
        {
            Tvuln = new HashSet<Tvuln>();
        }

        public long GroupSeq { get; set; }
        public long UpperSeq { get; set; }
        public int Level { get; set; }
        public string GroupType { get; set; }
        public long CompSeq { get; set; }
        public string DiagKind { get; set; }
        public string DiagTerm { get; set; }
        public string GroupId { get; set; }
        public string GroupName { get; set; }
        public string DiagTool { get; set; }
        public int? SortOrder { get; set; }
        public string CreateUserId { get; set; }
        public DateTime? CreateDt { get; set; }
        public string UpdateUserId { get; set; }
        public DateTime? UpdateDt { get; set; }

        public TcompInfo CompSeqNavigation { get; set; }
        public ICollection<Tvuln> Tvuln { get; set; }
    }
}
