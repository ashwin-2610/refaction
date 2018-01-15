using System.Data.SqlClient;
using System.Web;
using refactor_me.Repositories;

namespace refactor_me.Helpers
{
    public class Helper
    {
        private const string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={DataDirectory}\Database.mdf;Integrated Security=True";

        public static SqlConnection NewConnection()
        {
            var connstr = ConnectionString.Replace("{DataDirectory}", HttpContext.Current.Server.MapPath("~/App_Data"));
            return new SqlConnection(connstr);
        }

        public static ProductRepository GetProductRepository()
        {
            return ProductRepository.Instance;
        }
    }
}