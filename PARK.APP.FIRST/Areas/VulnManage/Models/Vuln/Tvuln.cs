using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PARK.APP.FIRST.Areas.VulnManage.Models.Vuln
{
    // https://docs.microsoft.com/en-us/dotnet/api/system.data.linq.mapping.columnattribute?redirectedfrom=MSDN&view=netframework-4.7.2
    // https://www.tektutorialshub.com/model-validation-in-asp-net-core-mvc/
    public partial class Tvuln
    {
        [Key]
        public long VulnSeq { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the Manage ID")]
        public long GroupSeq { get; set; }

        [Display(Name = "수동점검 여부")]
        public string ManualYn { get; set; }

        [Display(Name = "자동점검 여부")]
        public string AutoYn { get; set; }

        [Display(Name = "관리ID")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the Manage ID")]
        public string ManageId { get; set; }

        [Display(Name = "고객관리 항목ID")]
        public string ClientStandardId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the Vuln Name")]
        [StringLength(maximumLength: 25, MinimumLength = 3, ErrorMessage = "Length must be between 10 to 25")]
        [Display(Name = "항목명")]
        public string VulnName { get; set; }

        [Display(Name = "순차번호")]
        public int? SortOrder { get; set; }

        [Display(Name = "룰 관계 여부(웹취약점에 해당)")]
        public string RuleYn { get; set; }

        [Display(Name = "위험도")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the Rate")]
        public string Rate { get; set; }

        [Display(Name = "위험수준")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the Score")]
        public string Score { get; set; }

        [Display(Name = "적용시기")]
        public string ApplyTime { get; set; }

        [Display(Name = "상세설명")]
        public string Detail { get; set; }

        [Display(Name = "상세설명 첨부파일")]
        public string DetailPath { get; set; }

        [Display(Name = "판단기준")]
        public string Judgement { get; set; }

        [Display(Name = "영향도")]
        public string Effect { get; set; }

        [Display(Name = "조치방법")]
        public string Remedy { get; set; }

        [Display(Name = "조치방법 첨부파일")]
        public string RemedyPath { get; set; }

        [Display(Name = "참조")]
        public string Refrrence { get; set; }

        [Display(Name = "PARSER CONTENTS")]
        public string ParserContents { get; set; }

        [Display(Name = "ORIGINAL PARSER CONTENTS")]
        public string OrgParserContents { get; set; }

        [Display(Name = "적용대상")]
        public string ApplyTarget { get; set; }

        [Display(Name = "사용여부")]
        public string UseYn { get; set; }

        [Display(Name = "예외/제외 여부")]
        public string ExceptCd { get; set; }

        [Display(Name = "예외/제외 상세")]
        public string ExceptTermType { get; set; }

        [Display(Name = "예외/제외 적용시기")]
        public string ExceptTermFr { get; set; }

        [Display(Name = "예외/제외 적용시기")]
        public string ExceptTermTo { get; set; }

        [Display(Name = "예외/제외 적용 사유")]
        public string ExceptReason { get; set; }

        [Display(Name = "예외/제외 적용 사용자")]
        public string ExceptUserId { get; set; }

        [Display(Name = "예외/제외 기간")]
        //[DataType(DataType.Date)]
        public DateTime? ExceptDt { get; set; }

        [Display(Name = "등록 사용자")]
        public string CreateUserId { get; set; }

        [Display(Name = "등록일")]
        //[DataType(DataType.Date)]
        public DateTime? CreateDt { get; set; }

        [Display(Name = "수정 사용자")]
        public string UpdateUserId { get; set; }

        [Display(Name = "수정일")]
        //[DataType(DataType.Date)]
        public DateTime? UpdateDt { get; set; }

        [Display(Name = "NVISC - VULGROUP No.")]
        public long? Vulgroup { get; set; }

        [Display(Name = "NVISC - VULNO No.")]
        public string Vulno { get; set; }

        [Display(Name = "조치방법 상세설명")]
        public string RemedyDetail { get; set; }

        [Display(Name = "Overview")]
        public string Overview { get; set; }

        [Display(Name = "관리대상 항목 여부")]
        public string ManagementVulnYn { get; set; }

        public TvulnGroup GroupSeqNavigation { get; set; }
    }
}
