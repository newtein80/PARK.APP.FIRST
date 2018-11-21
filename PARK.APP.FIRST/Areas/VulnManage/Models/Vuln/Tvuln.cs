using System;
using System.Collections.Generic;

namespace PARK.APP.FIRST.Areas.VulnManage.Models.Vuln
{
    public partial class Tvuln
    {
        public long VulnSeq { get; set; }
        public long GroupSeq { get; set; }
        public string ManualYn { get; set; }
        public string AutoYn { get; set; }
        public string ManageId { get; set; }
        public string ClientStandardId { get; set; }
        public string VulnName { get; set; }
        public int? SortOrder { get; set; }
        public string RuleYn { get; set; }
        public string Rate { get; set; }
        public string Score { get; set; }
        public string ApplyTime { get; set; }
        public string Detail { get; set; }
        public string DetailPath { get; set; }
        public string Judgement { get; set; }
        public string Effect { get; set; }
        public string Remedy { get; set; }
        public string RemedyPath { get; set; }
        public string Refrrence { get; set; }
        public string ParserContents { get; set; }
        public string OrgParserContents { get; set; }
        public string ApplyTarget { get; set; }
        public string UseYn { get; set; }
        public string ExceptCd { get; set; }
        public string ExceptTermType { get; set; }
        public string ExceptTermFr { get; set; }
        public string ExceptTermTo { get; set; }
        public string ExceptReason { get; set; }
        public string ExceptUserId { get; set; }
        public DateTime? ExceptDt { get; set; }
        public string CreateUserId { get; set; }
        public DateTime? CreateDt { get; set; }
        public string UpdateUserId { get; set; }
        public DateTime? UpdateDt { get; set; }
        public long? Vulgroup { get; set; }
        public string Vulno { get; set; }
        public string RemedyDetail { get; set; }
        public string Overview { get; set; }
        public string ManagementVulnYn { get; set; }

        public TvulnGroup GroupSeqNavigation { get; set; }
    }
}
