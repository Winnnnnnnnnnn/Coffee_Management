using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyLibrary.DataAccess;


namespace MyLibrary.DataAccess
{
    public class UserDAO
    {
        private static UserDAO instance = null;
        private static readonly object instanceLock = new object();

        public static UserDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new UserDAO();
                    }
                    return instance;
                }
            }
        }

        public IEnumerable<User> GetUserList()
        {
            try
            {
                using (var context = new Coffee_ManagementContext())
                {
                    return context.Users.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving user list: " + ex.Message);
            }
        }

        public User GetUserByID(int userId)
        {
            try
            {
                using (var context = new Coffee_ManagementContext())
                {
                    return context.Users.FirstOrDefault(m => m.Id == userId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving user by ID: " + ex.Message);
            }
        }


        public void AddNew(User user)
        {
            try
            {
                var existingUser = GetUserByID(user.Id);
                if (existingUser == null)
                {
                    using (var context = new Coffee_ManagementContext())
                    {
                        context.Users.Add(user);
                        context.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception("The user already exists.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding new user: " + ex.Message);
            }
        }

        public void Update(User user)
        {
            try
            {
                var existingUser = GetUserByID(user.Id);
                if (existingUser != null)
                {
                    using (var context = new Coffee_ManagementContext())
                    {
                        context.Users.Update(user);
                        context.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception("The user does not exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating user: " + ex.Message);
            }
        }


        public void Remove(int userId)
        {
            try
            {
                var userToRemove = GetUserByID(userId);
                if (userToRemove != null)
                {
                    using (var context = new Coffee_ManagementContext())
                    {
                        context.Users.Remove(userToRemove);
                        context.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception("The user does not exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error removing user: " + ex.Message);
            }
        }

        public void RemoveMultiple(List<int> userIds)
        {
            try
            {
                using (var context = new Coffee_ManagementContext())
                {
                    foreach (var userId in userIds)
                    {
                        var userToRemove = context.Users.FirstOrDefault(u => u.Id == userId);
                        if (userToRemove != null)
                        {
                            context.Users.Remove(userToRemove);
                        }
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error removing users: " + ex.Message);
            }
        }



    }
}