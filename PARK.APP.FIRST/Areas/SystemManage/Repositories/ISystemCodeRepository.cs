using PARK.APP.FIRST.Areas.SystemManage.Models.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PARK.APP.FIRST.Areas.SystemManage.Repositories
{
    public interface ISystemCodeRepository
    {
        Task<List<TcommonCode>> GetCommonCodeDropDownListAsync(string codeType, string use_yn);
        List<TcommonCode> GetCommonCodeDropDownList(string codeType, string use_yn);
    }
}
