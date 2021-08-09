using StakeHolder.DAL.AbstractRepository;
using StakeHolder.DAL.ADOHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StakeHolder.DAL.Repository
{
    public class UserRepository : AbstractRepository<tblUser>
    {
        /// <summary>
        /// Get the Bid payment status with respect to the 
        /// </summary>
        /// <param name="dtFromdate"></param>
        /// <param name="dtTodate"></param>
        /// <param name="bidstatus"></param>
        /// <returns>DataSet</returns>
        public virtual DataSet GetUsersByFilter(int? type, string txt, int start, int end)
        {
            DataSet userDS = new DataSet();
            ADOConnection obj = new ADOConnection();
            try
            {
                SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@role", type),
                    new SqlParameter("@txt", txt),
                    new SqlParameter("@start", start),
                    new SqlParameter("@end", end)
                };
                userDS = obj.ExecuteDataSet("sp_GetUsersByFilter", parameters);
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }
            return userDS;
        }


    }
}
