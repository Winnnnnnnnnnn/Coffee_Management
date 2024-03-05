using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyLibrary.DataAccess;

namespace MyLibrary.Repository
{
    public interface IDetailRepository
    {
        IEnumerable<Detail> GetDetails();
        Detail GetDetailByID(int DetailId);
        void InsertDetail(Detail Detail);
        void DeleteDetail(int DetailId);
        void DeleteDetails(List<int> DetailIds);
        void UpdateDetail(Detail Detail);
    }
}