using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyLibrary.DataAccess;

namespace MyLibrary.Repository
{
    public interface ITableRepository
    {
        IEnumerable<Table> GetTables();
        Table GetTableByID(int tableId);
        void InsertTable(Table table);
        void DeleteTable(int tableId);
        void DeleteTables(List<int> tableIds);
        void UpdateTable(Table table);
    }
}
