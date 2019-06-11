using DataLibrary.DataAccess;
using DataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
