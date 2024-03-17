using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyLibrary.DataAccess;

namespace MyLibrary.Repository
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUsers();
        User GetUserByID(int userId);
        void InsertUser(User user);
        void DeleteUser(int userId);
        void DeleteUsers(List<int> userIds);
        void UpdateUser(User user);

        bool IsEmailExists(string email);
        bool IsPhoneExists(string phone);
        string GetRole(User user);

    }
}
