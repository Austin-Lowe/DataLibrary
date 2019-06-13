using PrimS.Telnet;
using Renci.SshNet;
using System;
using System.IO;
using System.Threading;

namespace DataLibrary.DataAccess
{
    class CISCO
    {
        /// <summary>
        /// Connects and executes commands on the Cisco IOS device for reseting Port-Security on the provided port.
        /// </summary>
        /// <param name="conn">A SSH Connection Object</param>
        /// <param name="port">A String of the port to be modified, on the network device.</param>
        public static void ResetPortSecurity(SshClient conn, String port)
        {
            conn.Connect();

            ShellStream ss = conn.CreateShellStream("cisco", 80, 24, 800, 600, 1024);

            System.Diagnostics.Debug.WriteLine(SendCommandLF("enable", ss));
            System.Diagnostics.Debug.WriteLine(SendCommand("cisco", ss));
            System.Diagnostics.Debug.WriteLine(SendCommandLF("clear port-security sticky interface " + port, ss));

            System.Diagnostics.Debug.WriteLine(SendCommandLF("configure terminal", ss));
            System.Diagnostics.Debug.WriteLine(SendCommandLF("interface " + port, ss));

            System.Diagnostics.Debug.WriteLine(SendCommandLF("no switchport port-security", ss));
            System.Diagnostics.Debug.WriteLine(SendCommandLF("switchport port-security", ss));
            System.Diagnostics.Debug.WriteLine(SendCommandLF("shutdown", ss));
            System.Diagnostics.Debug.WriteLine(SendCommandLF("no shutdown", ss));

            System.Diagnostics.Debug.WriteLine(SendCommandLF("end", ss));
            System.Diagnostics.Debug.WriteLine(SendCommandLF("copy running-config startup-config", ss));

            conn.Disconnect();
        }

        /// <summary>
        /// Connects and updates the maximum number of permitted MAC Addresses on the provided port.
        /// </summary>
        /// <param name="conn">A SSH connection object</param>
        /// <param name="port">A String of the port to be modified, on the network device.</param>
        /// <param name="count">A integer with the count of maximum number of MAC Adresses permitted.</param>
        public static void SetMaxMAC(SshClient conn, String port, int count)
        {
            conn.Connect();

            ShellStream ss = conn.CreateShellStream("cisco", 80, 24, 800, 600, 1024);

            System.Diagnostics.Debug.WriteLine(SendCommandLF("enable", ss));
            System.Diagnostics.Debug.WriteLine(SendCommand("cisco", ss));
            System.Diagnostics.Debug.WriteLine(SendCommandLF("configure terminal", ss));
            System.Diagnostics.Debug.WriteLine(SendCommandLF("interface " + port, ss));

            System.Diagnostics.Debug.WriteLine(SendCommandLF("switchport port-security maximum " + count.ToString(), ss));

            System.Diagnostics.Debug.WriteLine(SendCommandLF("end", ss));
            System.Diagnostics.Debug.WriteLine(SendCommandLF("copy running-config startup-config", ss));

            conn.Disconnect();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="secret"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static async System.Threading.Tasks.Task TelnetResetPortAsync(String ip, String username, String password, String secret, String port)
        {
            Client client = new Client(ip, 23, new System.Threading.CancellationToken());

            await client.WriteLine(username);
            await client.WriteLine(password);
            await client.WriteLine("enable");
            await client.WriteLine(secret);
            await client.WriteLine("clear port-security sticky interface " + port);

            await client.WriteLine("configure terminal");
            await client.WriteLine("interface " + port);

            await client.WriteLine("no switchport port-security");
            await client.WriteLine("switchport port-security");
            await client.WriteLine("shutdown");
            await client.WriteLine("no shutdown");

            await client.WriteLine("end");
            await client.WriteLine("copy running-config startup-config");

            String s = await client.ReadAsync(TimeSpan.FromMilliseconds(500));

            System.Diagnostics.Debug.WriteLine(s);
            client.Dispose();
        }

        public static async System.Threading.Tasks.Task TelnetSetMaxMAC(String ip, String username, String password, String secret, String port, int count)
        {
            Client client = new Client(ip, 23, new System.Threading.CancellationToken());

            await client.WriteLine(username);
            await client.WriteLine(password);
            await client.WriteLine("enable");
            await client.WriteLine(secret);

            await client.WriteLine("configure terminal");
            await client.WriteLine("interface " + port);

            await client.WriteLine("switchport port-security maximum " + count.ToString());

            await client.WriteLine("end");
            await client.WriteLine("copy running-config startup-config");

            String s = await client.ReadAsync(TimeSpan.FromMilliseconds(1000));

            System.Diagnostics.Debug.WriteLine(s);
            client.Dispose();
        }

        private static string SendCommandW(string cmd, ShellStream sStream)
        {
            StreamReader reader = new StreamReader(sStream);
            StreamWriter writer = new StreamWriter(sStream)
            {
                AutoFlush = true
            };
            writer.Write(cmd);
            while (sStream.Length == 0)
                Thread.Sleep(500);

            return reader.ReadToEnd();

        }

        private static string SendCommand(string cmd, ShellStream sStream)
        {
            StreamReader reader = new StreamReader(sStream);
            StreamWriter writer = new StreamWriter(sStream)
            {
                AutoFlush = true
            };
            writer.WriteLine(cmd);
            while (sStream.Length == 0)
                Thread.Sleep(500);

            return reader.ReadToEnd();
        }

        private static string SendCommandLF(string cmd, ShellStream sStream)
        {
            StreamReader reader = new StreamReader(sStream);
            StreamWriter writer = new StreamWriter(sStream)
            {
                AutoFlush = true
            };
            writer.Write(cmd + "\n");
            while (sStream.Length == 0)
                Thread.Sleep(500);

            return reader.ReadToEnd();

        }
    }
}
