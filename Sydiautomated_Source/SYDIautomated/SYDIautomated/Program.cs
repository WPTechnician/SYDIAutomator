using System;
using System.Diagnostics;
using System.IO;
using System.Configuration;

namespace SYDIautomated
{
    class Program
    {
        static void Main(string[] args)
        {
            //Clear the results folder
            if (!Directory.Exists(@"c:\scripts"))
            {
                Console.WriteLine("ERROR: Please make sure the c:\\scripts exists.");
                return;
            }
            DirectoryInfo di = new DirectoryInfo(@"c:\scripts\results");
            foreach (var fi in di.GetFiles())
            {
                fi.Delete();
            }
            try
            {
                runcmd("cscript", @"c:\scripts\tools\sydi-wrapper.vbs -a""" + ConfigurationManager.AppSettings["OU"] + "\"");
                runcmd("cscript", @"c:\scripts\tools\sydi-wrapper.vbs -a""" + ConfigurationManager.AppSettings["DCOU"] + "\"");
                runcmd("cmd", "/c forfiles /p c:\\scripts\\results /c \"cmd /c cscript -nologo c:\\scripts\\tools\\sydi-transform.vbs -x@file -sc:\\scripts\\xml\\serverhtml.xsl -o@file.htm\"");
            }
            catch(Exception ex)
            {
                EventLog.WriteEntry("SYDI_Automator", ex.Message, EventLogEntryType.Error, 60000);
            }
            try
            {
                copyfiles();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("SYDI_Automator", ex.Message, EventLogEntryType.Error, 60000);
            }
        }

        private static void copyfiles()
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(@"c:\scripts\results");
                string web_path = ConfigurationManager.AppSettings["Web_Path"];
                foreach (var file in di.GetFiles("*.htm"))
                {
                    string newName = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + file.Name;
                    file.MoveTo(web_path + "\\" + newName);
                    //File.Copy(file.FullName, );
                }
            }
            catch(Exception ex)
            {
                EventLog.WriteEntry("SYDI_Automator", ex.Message, EventLogEntryType.Error, 60000);
                return;
            }
        }
        
        private static void runcmd(string cmd, string arg)
        {
            try
            {
                Process SYDIWrap = new Process();
                SYDIWrap.StartInfo.CreateNoWindow = false;
                SYDIWrap.StartInfo.FileName = cmd;
                SYDIWrap.StartInfo.UseShellExecute = false;
                SYDIWrap.StartInfo.WorkingDirectory = ConfigurationManager.AppSettings["Scripts_Path"];
                SYDIWrap.StartInfo.Arguments = arg;
                SYDIWrap.Start();
                SYDIWrap.WaitForExit();
                SYDIWrap.Close();
            }
            catch(Exception ex)
            {
                EventLog.WriteEntry("SYDI_Automator", ex.Message, EventLogEntryType.Error, 60000);
            }
        }
    }
}
