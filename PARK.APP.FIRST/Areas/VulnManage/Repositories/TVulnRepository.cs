using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PARK.APP.FIRST.Areas.VulnManage.Models.Vuln;

using Dapper;

namespace PARK.APP.FIRST.Areas.VulnManage.Repositories
{
    public class TVulnRepository : ITVulnRepository
    {
        private readonly IConfiguration _configuration;

        public TVulnRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection DbConnection
        {
            get
            {
                return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            }
        }

        public async Task<List<Tvuln>> GetByVulnGroupSeq(int vulnGroupSeq)
        {
            using (IDbConnection connection = DbConnection)
            {
                string strQuery = @"select top 10 * from dbo.TVuln where GROUP_SEQ = @GROUP_SEQ";
                DbConnection.Open();
                var result = await DbConnection.QueryAsync<Tvuln>(strQuery, new { GROUP_SEQ = vulnGroupSeq });
                return result.ToList();
            }
        }
    }
}
