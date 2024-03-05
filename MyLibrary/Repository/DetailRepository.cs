using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyLibrary.DataAccess;


namespace MyLibrary.Repository
{
    public class DetailRepository : IDetailRepository
    {
        public IEnumerable<Detail> GetDetails() => DetailDAO.Instance.GetDetailList();
        public Detail GetDetailByID(int DetailId) => DetailDAO.Instance.GetDetailByID(DetailId);
        public void InsertDetail(Detail Detail) => DetailDAO.Instance.AddNew(Detail);
        public void DeleteDetail(int DetailId) => DetailDAO.Instance.Remove(DetailId);
        public void DeleteDetails(List<int> idsToDelete) => DetailDAO.Instance.RemoveMultiple(idsToDelete);
        public void UpdateDetail(Detail Detail) => DetailDAO.Instance.Update(Detail);
    }
}