using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLibrary.DataAccess
{
    public class TableDAO
    {
        private static TableDAO instance = null;
        private static readonly object instanceLock = new object();

        public static TableDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new TableDAO();
                    }
                    return instance;
                }
            }
        }

        public Table GetTableByID(int? table_id)
        {
            try
            {
                using (var context = new Coffee_ManagementContext())
                {
                    return context.Tables.FirstOrDefault(m => m.Id == table_id);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving table by ID: " + ex.Message);
            }
        }

    }
}