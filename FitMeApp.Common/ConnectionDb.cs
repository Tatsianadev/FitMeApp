using System;

namespace FitMeApp.Common
{
    public class ConnectionDb
    {
        public static string GetConnectionString()
        {
            string connectionString = @"Data Source=DESKTOP-R0KBKEQ\SQLEXPRESS;Initial catalog=FitMeDB;Integrated Security=True;TrustServerCertificate=True";
            return connectionString;
        }
    }
}
