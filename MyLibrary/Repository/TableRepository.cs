using System;
using System.Collections.Generic;
using System.Linq;
using MyLibrary.DataAccess;

namespace MyLibrary.Repository
{
    public class TableRepository : ITableRepository
    {
        public IEnumerable<Table> GetTables() => TableDAO.Instance.GetTableList();
        public Table GetTableByID(int tableId) => TableDAO.Instance.GetTableByID(tableId);
        public void InsertTable(Table table) => TableDAO.Instance.AddNew(table);
        public void DeleteTable(int tableId) => TableDAO.Instance.Remove(tableId);
        public void DeleteTables(List<int> idsToDelete) => TableDAO.Instance.RemoveMultiple(idsToDelete);
        public void UpdateTable(Table table) => TableDAO.Instance.Update(table);
    }
}
