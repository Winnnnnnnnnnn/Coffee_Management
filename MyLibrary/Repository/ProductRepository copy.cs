using System;
using System.Collections.Generic;
using System.Linq;
using MyLibrary.DataAccess;

namespace MyLibrary.Repository
{
    public class SettingRepository : ISettingRepository
    {
        public IEnumerable<Setting> GetSettings() => SettingDAO.Instance.GetSettingList();
        public Setting GetSettingByID(int setting_id) => SettingDAO.Instance.GetSettingByID(setting_id);
        public void InsertSetting(Setting setting) => SettingDAO.Instance.AddNew(setting);
        public void DeleteSetting(int setting_id) => SettingDAO.Instance.Remove(setting_id);
        public void DeleteSettings(List<int> setting_ids) => SettingDAO.Instance.RemoveMultiple(setting_ids);
        public void UpdateSetting(Setting setting) => SettingDAO.Instance.Update(setting);
    }
}
