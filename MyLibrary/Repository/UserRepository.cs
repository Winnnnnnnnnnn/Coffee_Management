using System;
using System.Collections.Generic;
using System.Linq;
using MyLibrary.DataAccess;

namespace MyLibrary.Repository
{
    public class UserRepository : IUserRepository
    {
        public IEnumerable<User> GetUsers() => UserDAO.Instance.GetUserList();
        public User GetUserByID(int userId) => UserDAO.Instance.GetUserByID(userId);
        public void InsertUser(User user) => UserDAO.Instance.AddNew(user);
        public void DeleteUser(int userId) => UserDAO.Instance.Remove(userId);
        public void DeleteUsers(List<int> idsToDelete) => UserDAO.Instance.RemoveMultiple(idsToDelete);
        public void UpdateUser(User user) => UserDAO.Instance.Update(user);

        public string GetRole(User user)
        {
            var role = "";
            switch (user.Role)
            {
                case 1:
                    role = "Admin";
                    break;
                case 2:
                    role = "Chủ quán";
                    break;
                case 3:
                    role = "Thu ngân";
                    break;
                default:
                    role = "Nhân viên";
                    break;

            }
            return role;
        }
    }
}
