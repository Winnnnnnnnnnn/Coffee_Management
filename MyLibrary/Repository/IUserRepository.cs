using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyLibrary.DataAccess;

namespace MyLibrary.Repository
{
    public class UserRepository : IUserRepository
    {
        public User GetUserByID(int id) => UserDAO.Instance.GetUserByID(id);
        public IEnumerable<User> GetUsers() => UserDAO.Instance.GetUserList();
        public void InsertUser(User user) => UserDAO.Instance.AddNew(user);
        public void DeleteUser(int id) => UserDAO.Instance.Remove(id);
        public void UpdateUser(User user) => UserDAO.Instance.Update(user);
    }
}
