using System;
using System.Linq;
using System.Diagnostics;
using System.IO;
using EnterpriseDT.Net.Ftp;

namespace SYDIautomated
{
    class Program
    {
        static void Main(string[] args)
        {
            //Clear the results folder
            if(!Directory.Exists(ou.Default.Scripts_Path))
            {
                Console.WriteLine("ERROR: Please unzip scripts.zip to a path and update SYDIautomated.exe.config with a valid Script Path");
                return;
            }
            DirectoryInfo di = new DirectoryInfo(ou.Default.Scripts_Path + "\\results");
            foreach (var fi in di.GetFiles())
            {
                fi.Delete();
            }
            //Make sure the values you set in ou.settings are accurate.
            try
            {
                runcmd("cscript", ou.Default.Scripts_Path + "\\tools\\sydi-wrapper.vbs -a\"" + ou.Default.OU + "\"");
                runcmd("cscript", ou.Default.Scripts_Path + "\\tools\\sydi-wrapper.vbs -a\"" + ou.Default.DCOU + "\"");
                runcmd("cmd", "/c forfiles /p " + ou.Default.Scripts_Path + "\\results /c \"cmd /c cscript -nologo " + ou.Default.Scripts_Path + "\\tools\\sydi-transform.vbs -x@file -s" + ou.Default.Scripts_Path + "\\xml\\serverhtml.xsl -o@file.htm\"");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            try
            {
                ftpfiles(args);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private static void ftpfiles(string[] args)
        {
            try
            {
                FTPConnection ftp = new FTPConnection();
                ftp.ServerAddress = ou.Default.FTP_Address;
                ftp.UserName = ou.Default.FTP_UserName;
                ftp.Password = ou.Default.FTP_Password;
                ftp.ConnectMode = FTPConnectMode.PASV;
                ftp.Connect();
                FTPFile[] files = ftp.GetFileInfos("/SYDI-Docs/Files");

                if (files.Count() > 0)
                {
                    foreach (var fi in files)
                    {
                        if (fi.Size != 0)
                        {
                            string newName = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + fi.Name;
                            ftp.RenameFile("/SYDI-Docs/files/" + fi.Name, "/SYDI-Docs/files/" + newName);
                        }
                    }
                }
                try
                {
                    DirectoryInfo di = new DirectoryInfo(ou.Default.Scripts_Path + "\\results");
                    foreach (var file in di.GetFiles("*.htm"))
                    {
                        ftp.UploadFile(file.FullName, "/SYDI-Docs/files/" + file.Name);
                    }
                    ftp.Close();
                }
                catch
                {
                    Console.WriteLine("There was a problem with the FTP server!");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
                SYDIWrap.StartInfo.WorkingDirectory = @"C:\scripts\";
                SYDIWrap.StartInfo.Arguments = arg;
                SYDIWrap.Start();
                SYDIWrap.WaitForExit();
                SYDIWrap.Close();
            }
            catch
            {
                Console.WriteLine("Failed running the SYDI script command!");
            }
        }
    }
}
