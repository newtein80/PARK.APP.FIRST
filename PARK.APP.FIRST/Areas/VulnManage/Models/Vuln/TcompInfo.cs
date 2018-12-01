using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PARK.APP.FIRST.Areas.VulnManage.Models.Vuln
{
    public partial class TcompInfo
    {
        public TcompInfo()
        {
            TvulnGroup = new HashSet<TvulnGroup>();
        }

        public long CompSeq { get; set; }
        [Display(Name = "컴플라이언스 명")]
        public string CompName { get; set; }
        [Display(Name = "컴플라이언스 설명")]
        public string CompDescription { get; set; }
        public string CompRef { get; set; }
        [Display(Name = "기준년도")]
        public string StandardYear { get; set; }
        [Display(Name = "점검 구분")]
        public string DiagType { get; set; }
        [Display(Name = "확정 여부")]
        public string ConfirmYn { get; set; }
        [Display(Name = "사용 여부")]
        public string UseYn { get; set; }
        public string CreateUserId { get; set; }
        public DateTime? CreateDt { get; set; }
        public string UpdateUserId { get; set; }
        public DateTime? UpdateDt { get; set; }
        public string CompDetailGubun { get; set; }

        public ICollection<TvulnGroup> TvulnGroup { get; set; }
    }
}
