using System.Collections.Generic;
using DataLibrary.Models;
using DataLibrary.DataAccess;

namespace DataLibrary.Logic
{
    class SwitchProcessor
    {
        public static List<SwitchModel> LoadSwitches()
        {
            string sql = @"select HOSTNAME, DEVICE_TYPE, IP, USERNAME, PASSWORD, SECRET
                            from dbo.SWITCH;";

            return SQLDataAccess.LoadData<SwitchModel>(sql);
        }
    }
}
