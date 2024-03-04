using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyLibrary.DataAccess;

namespace MyLibrary.Repository
{
    public interface ISettingRepository
    {
        IEnumerable<Setting> GetSettings();
        Setting GetSettingByID(int setting_id);
        void InsertSetting(Setting setting);
        void DeleteSetting(int setting_id);
        void DeleteSettings(List<int> setting_ids);
        void UpdateSetting(Setting setting);
    }
}
