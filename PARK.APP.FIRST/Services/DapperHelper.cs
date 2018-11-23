using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PARK.APP.FIRST.Services
{
    public static class DapperHelper
    {
        public static IConfiguration Configuration;
        // Remember to add <remove name="LocalSqlServer" > in ConnectionStrings section if using this, as otherwise it would be the first one.
        //private static string connectionString = ConfigurationManager.ConnectionStrings[0].ConnectionString;
        private static string connectionString = Configuration.GetConnectionString("DefaultConnection");

        #region+ http://www.nullskull.com/a/10399923/sqlmapperhelper--a-helper-class-for-dapperdotnet.aspx
        /// <summary>
        /// Gets the open connection.
        /// </summary>
        /// <param name="name">The name of the connection string (optional).</param>
        /// <returns></returns>
        public static SqlConnection GetOpenConnection(string name = null)
        {
            string connString = "";
            //connString = name == null ? connString = ConfigurationManager.ConnectionStrings[0].ConnectionString : connString = ConfigurationManager.ConnectionStrings[name].ConnectionString;
            connString = name == null ? connString = Configuration.GetConnectionString("DefaultConnection") : connString = Configuration.GetConnectionString("DefaultConnection");
            var connection = new SqlConnection(connString);
            connection.Open();
            return connection;
        }


        public static int InsertMultiple<T>(string sql, IEnumerable<T> entities, string connectionName = null) where T : class, new()
        {
            using (SqlConnection cnn = GetOpenConnection(connectionName))
            {
                int records = 0;

                foreach (T entity in entities)
                {
                    records += cnn.Execute(sql, entity);
                }
                return records;
            }
        }

        public static DataTable ToDataTable<T>(this IList<T> list)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in list)
            {
                for (int i = 0; i < values.Length; i++)
                    values[i] = props[i].GetValue(item) ?? DBNull.Value;
                table.Rows.Add(values);
            }
            return table;
        }

        public static DynamicParameters GetParametersFromObject(object obj, string[] propertyNamesToIgnore)
        {
            if (propertyNamesToIgnore == null) propertyNamesToIgnore = new string[] { String.Empty };
            DynamicParameters p = new DynamicParameters();
            PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in properties)
            {
                if (!propertyNamesToIgnore.Contains(prop.Name))
                    p.Add("@" + prop.Name, prop.GetValue(obj, null));
            }
            return p;
        }

        public static void SetIdentity<T>(IDbConnection connection, Action<T> setId)
        {
            dynamic identity = connection.Query("SELECT @@IDENTITY AS Id").Single();
            T newId = (T)identity.Id;
            setId(newId);
        }


        public static object GetPropertyValue(object target, string propertyName)
        {
            PropertyInfo[] properties = target.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            object theValue = null;
            foreach (PropertyInfo prop in properties)
            {
                if (string.Compare(prop.Name, propertyName, true) == 0)
                {
                    theValue = prop.GetValue(target, null);
                }
            }
            return theValue;
        }

        public static void SetPropertyValue(object p, string propName, object value)
        {
            Type t = p.GetType();
            PropertyInfo info = t.GetProperty(propName);
            if (info == null)
                return;
            if (!info.CanWrite)
                return;
            info.SetValue(p, value, null);
        }

        /// <summary>
        /// Stored proc.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procname">The procname.</param>
        /// <param name="parms">The parms.</param>
        /// <returns></returns>
        public static List<T> StoredProcWithParams<T>(string procname, dynamic parms, string connectionName = null)
        {
            using (SqlConnection connection = GetOpenConnection(connectionName))
            {
                return connection.Query<T>(procname, (object)parms, commandType: CommandType.StoredProcedure).ToList();
            }

        }


        /// <summary>
        /// Stored proc with params returning dynamic.
        /// </summary>
        /// <param name="procname">The procname.</param>
        /// <param name="parms">The parms.</param>
        /// <param name="connectionName">Name of the connection.</param>
        /// <returns></returns>
        public static List<dynamic> StoredProcWithParamsDynamic(string procname, dynamic parms, string connectionName = null)
        {
            using (SqlConnection connection = GetOpenConnection(connectionName))
            {
                return connection.Query(procname, (object)parms, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        /// <summary>
        /// Stored proc insert with ID.
        /// </summary>
        /// <typeparam name="T">The type of object</typeparam>
        /// <typeparam name="U">The Type of the ID</typeparam>
        /// <param name="procName">Name of the proc.</param>
        /// <param name="parms">instance of DynamicParameters class. This should include a defined output parameter</param>
        /// <returns>U - the @@Identity value from output parameter</returns>
        public static U StoredProcInsertWithID<T, U>(string procName, DynamicParameters parms, string connectionName = null)
        {
            using (SqlConnection connection = DapperHelper.GetOpenConnection(connectionName))
            {
                var x = connection.Execute(procName, (object)parms, commandType: CommandType.StoredProcedure);
                return parms.Get<U>("@ID");
            }
        }


        /// <summary>
        /// SQL with params.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="parms">The parms.</param>
        /// <returns></returns>
        public static List<T> SqlWithParams<T>(string sql, dynamic parms, string connectionnName = null)
        {
            using (SqlConnection connection = GetOpenConnection(connectionnName))
            {
                return connection.Query<T>(sql, (object)parms).ToList();
            }
        }

        /// <summary>
        /// Insert update or delete SQL.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="parms">The parms.</param>
        /// <returns></returns>
        public static int InsertUpdateOrDeleteSql(string sql, dynamic parms, string connectionName = null)
        {
            using (SqlConnection connection = GetOpenConnection(connectionName))
            {
                return connection.Execute(sql, (object)parms);
            }
        }

        /// <summary>
        /// Insert update or delete stored proc.
        /// </summary>
        /// <param name="procName">Name of the proc.</param>
        /// <param name="parms">The parms.</param>
        /// <returns></returns>
        public static int InsertUpdateOrDeleteStoredProc(string procName, dynamic parms, string connectionName = null)
        {
            using (SqlConnection connection = GetOpenConnection(connectionName))
            {
                return connection.Execute(procName, (object)parms, commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// SQLs the with params single.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="parms">The parms.</param>
        /// <param name="connectionName">Name of the connection.</param>
        /// <returns></returns>
        public static T SqlWithParamsSingle<T>(string sql, dynamic parms, string connectionName = null)
        {
            using (SqlConnection connection = GetOpenConnection(connectionName))
            {
                return connection.Query<T>(sql, (object)parms).FirstOrDefault();
            }
        }

        /// <summary>
        ///  proc with params single returning Dynamic object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="parms">The parms.</param>
        /// <param name="connectionName">Name of the connection.</param>
        /// <returns></returns>
        public static System.Dynamic.DynamicObject DynamicProcWithParamsSingle<T>(string sql, dynamic parms, string connectionName = null)
        {
            using (SqlConnection connection = GetOpenConnection(connectionName))
            {
                return connection.Query(sql, (object)parms, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        /// <summary>
        /// proc with params returning Dynamic.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="parms">The parms.</param>
        /// <param name="connectionName">Name of the connection.</param>
        /// <returns></returns>
        public static IEnumerable<dynamic> DynamicProcWithParams<T>(string sql, dynamic parms, string connectionName = null)
        {
            using (SqlConnection connection = GetOpenConnection(connectionName))
            {
                return connection.Query(sql, (object)parms, commandType: CommandType.StoredProcedure);
            }
        }


        /// <summary>
        /// Stored proc with params returning single.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procname">The procname.</param>
        /// <param name="parms">The parms.</param>
        /// <param name="connectionName">Name of the connection.</param>
        /// <returns></returns>
        public static T StoredProcWithParamsSingle<T>(string procname, dynamic parms, string connectionName = null)
        {
            using (SqlConnection connection = GetOpenConnection(connectionName))
            {
                return connection.Query<T>(procname, (object)parms, commandType: CommandType.StoredProcedure).SingleOrDefault();
            }
        }
        #endregion

        #region+ https://m.blog.naver.com/PostView.nhn?blogId=wolfre&logNo=220597602977&proxyReferer=https%3A%2F%2Fwww.google.co.kr%2F
        // Select List
        public static IEnumerable<T> GetList<T>(string targetDB, string storedProcedure, DynamicParameters param = null) where T : class
        {
            using (SqlConnection connection = new SqlConnection(targetDB))
            {
                connection.Open();
                var output = connection.Query<T>(storedProcedure, param, commandType: CommandType.StoredProcedure);
                return output;
            }
        }

        // Multiple Select 1: N...
        public static Dictionary<Tmain, List<Tsub>> MultiPleGetList<Tmain, Tsub>(string targetDB, string storedProcedure, DynamicParameters param = null) where Tmain : class
        {
            using (SqlConnection connection = new SqlConnection(targetDB))
            {
                connection.Open();

                Dictionary<Tmain, List<Tsub>> listTable = new Dictionary<Tmain, List<Tsub>>();

                using (var output = connection.QueryMultiple(storedProcedure, param, commandType: CommandType.StoredProcedure))
                {
                    var main = output.Read<Tmain>().SingleOrDefault();
                    var sub = output.Read<Tsub>().ToList();

                    if (main != null && sub != null)
                    {
                        listTable.Add((Tmain)main, sub);
                    }

                    return listTable;
                }
            }
        }

        // Top 1
        public static T Top1<T>(string targetDB, string storedProcedure, DynamicParameters param = null) where T : class
        {
            using (SqlConnection connection = new SqlConnection(targetDB))
            {
                connection.Open();
                var output = connection.Query<T>(storedProcedure, param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                return output;
            }
        }

        // Insert, Update, Delete
        public static void Process<T>(string targetDB, string storedProcedure, DynamicParameters param = null) where T : class
        {
            using (SqlConnection connection = new SqlConnection(targetDB))
            {
                connection.Open();
                connection.Execute(storedProcedure, param, commandType: CommandType.StoredProcedure);
            }
        }

        // 메인 서브 트랜잭션
        //public int DeleteProduct(Product product)
        //{
        //    using (IDbConnection connection = OpenConnection())
        //    {
        //        const string deleteImageQuery = "DELETE FROM Production.ProductProductPhoto " +
        //                                        "WHERE ProductID = @ProductID";
        //        const string deleteProductQuery = "DELETE FROM Production.Product " +
        //                                          "WHERE ProductID = @ProductID";
        //        IDbTransaction transaction = connection.BeginTransaction();
        //        int rowsAffected = connection.Execute(deleteImageQuery,new { ProductID = product.ProductID }, transaction);
        //        rowsAffected += connection.Execute(deleteProductQuery,new { ProductID = product.ProductID }, transaction);
        //        transaction.Commit();
        //        return rowsAffected;
        //    }
        //}
        #endregion
    }
}
