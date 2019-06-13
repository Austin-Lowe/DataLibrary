using DataLibrary.DataAccess;
using DataLibrary.Models;
using System.Collections.Generic;

namespace DataLibrary.Logic
{
    public class PortProcessor
    {
        public static List<PortModel> LoadPorts()
        {
            string sql = @"select port, name, status, Vlan, duplex, speed, type, host
                            from dbo.PORTS;";

            return SQLDataAccess.LoadData<PortModel>(sql);
        }
    }
}
