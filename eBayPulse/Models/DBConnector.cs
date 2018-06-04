using Microsoft.EntityFrameworkCore;
using Microsoft.Data;

namespace eBayPulse.Models
{
    public class DBConnector
    {
        public eBayPulseContext context { get; private set; }
        private static DBConnector connection;
        private DBConnector()
        {
            if (context == null)
            {
                context = new eBayPulseContext();
            }
        }
        public static DBConnector getConnection()
        {
            if (connection == null)
                connection = new DBConnector();
            return connection;
        }      
    }
}