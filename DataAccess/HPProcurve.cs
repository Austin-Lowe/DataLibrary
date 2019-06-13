using PrimS.Telnet;
using System;

namespace DataLibrary.DataAccess
{
    class HPProcurve
    {
        public static async System.Threading.Tasks.Task TelnetResetPortAsync(String ip, String username, String password, String port)
        {
            Client client = new Client(ip, 23, new System.Threading.CancellationToken());
            String output;

            await client.Write("?");
            output = await client.TerminatedReadAsync("#", TimeSpan.FromMilliseconds(500));
            Console.WriteLine(output);
            await client.WriteLine("configure terminal");
            await client.WriteLine("no port-security " + port);
            await client.WriteLine("port-security " + port + " learn-mode static address-limit 1 action send-disable clear-intrusion-flag");
            await client.WriteLine("interface " + port + "enable");

            string s = await client.ReadAsync(TimeSpan.FromMilliseconds(500));

            System.Diagnostics.Debug.WriteLine(output);
            System.Diagnostics.Debug.WriteLine(s);
            client.Dispose();
        }

        public static async System.Threading.Tasks.Task TelnetSetMaxMAC(String ip, String username, String password, String port, int count)
        {
            Client client = new Client(ip, 23, new System.Threading.CancellationToken());

            await client.Write("?");
            await client.WriteLine("configure terminal");
            await client.WriteLine("port-security " + port + " address-limit " + count.ToString());

            String s = await client.ReadAsync(TimeSpan.FromMilliseconds(500));

            System.Diagnostics.Debug.WriteLine(s);
            client.Dispose();
        }
    }
}
