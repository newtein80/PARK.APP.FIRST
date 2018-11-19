using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PARK.APP.FIRST.Areas.SystemManage.Models.Menu
{
    // https://www.codeproject.com/Articles/1237650/ASP-NET-Core-User-Role-Base-Dynamic-Menu-Managemen
    // warining : _(underbar)
    public partial class MenuMaster
    {
        [Key]
        public int MenuIdentity { get; set; }
        public string MenuId { get; set; }
        public string MenuName { get; set; }
        public string Parent_MenuId { get; set; }
        public string User_Roll { get; set; }
        public string MenuFileName { get; set; }
        public string MenuUrl { get; set; }
        public string Use_Yn { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
