using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PARK.APP.FIRST.Areas.SystemManage.Models.System;
using Microsoft.EntityFrameworkCore;

namespace PARK.APP.FIRST.Areas.SystemManage.Repositories
{
    public class SystemCodeRepository : ISystemCodeRepository
    {
        private readonly SystemDbContext _context;

        public SystemCodeRepository(SystemDbContext context)
        {
            _context = context;
        }

        public async Task<List<TcommonCode>> GetCommonCodeDropDownListAsync(string codeType, string use_yn)
        {
            //return await _context.TcommonCode.Where(r => r.CodeType == codeType).;
            return await _context.TcommonCode.Where(r => r.CodeType == codeType).ToListAsync();
        }
    }
}
