using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyLibrary.DataAccess;


namespace MyLibrary.DataAccess
{
    public class SettingDAO
    {
        private static SettingDAO instance = null;
        private static readonly object instanceLock = new object();

        public static SettingDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new SettingDAO();
                    }
                    return instance;
                }
            }
        }

        public IEnumerable<Setting> GetSettingList()
        {
            try
            {
                using (var context = new Coffee_ManagementContext())
                {
                    return context.Settings.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving setting list: " + ex.Message);
            }
        }

        public Setting GetSettingByID(int setting_id)
        {
            try
            {
                using (var context = new Coffee_ManagementContext())
                {
                    return context.Settings.FirstOrDefault(m => m.Id == setting_id);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving setting by ID: " + ex.Message);
            }
        }


        public void AddNew(Setting setting)
        {
            try
            {
                var existingSetting = GetSettingByID(setting.Id);
                if (existingSetting == null)
                {
                    using (var context = new Coffee_ManagementContext())
                    {
                        context.Settings.Add(setting);
                        context.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception("The setting already exists.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding new setting: " + ex.Message);
            }
        }

        public void Update(Setting setting)
        {
            try
            {
                var existingSetting = GetSettingByID(setting.Id);
                if (existingSetting != null)
                {
                    using (var context = new Coffee_ManagementContext())
                    {
                        context.Settings.Update(setting);
                        context.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception("The setting does not exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating setting: " + ex.Message);
            }
        }


        public void Remove(int settingId)
        {
            try
            {
                var settingToRemove = GetSettingByID(settingId);
                if (settingToRemove != null)
                {
                    using (var context = new Coffee_ManagementContext())
                    {
                        context.Settings.Remove(settingToRemove);
                        context.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception("The setting does not exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error removing setting: " + ex.Message);
            }
        }

        public void RemoveMultiple(List<int> settingIds)
        {
            try
            {
                using (var context = new Coffee_ManagementContext())
                {
                    foreach (var settingId in settingIds)
                    {
                        var settingToRemove = context.Users.FirstOrDefault(u => u.Id == settingId);
                        if (settingToRemove != null)
                        {
                            context.Users.Remove(settingToRemove);
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