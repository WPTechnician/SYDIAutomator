using System;
using System.Linq;
using System.IO;
using System.Collections;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using PdfSharp.Pdf;
using System.Configuration;

namespace ServerDocs
{
    public partial class Main : System.Web.UI.Page
    {
        protected internal bool archive;
        ArrayList dates = new ArrayList();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(Directory.Exists(ConfigurationManager.AppSettings["File_Path"])))
            {
                LblError.Text = "Configured file path doesn't exist!";
                LblError.Visible = true;
                return;
            }
            LblError.Visible = false;
            DirectoryInfo di = new DirectoryInfo(ConfigurationManager.AppSettings["File_Path"]);

            foreach (var file in di.GetFiles("*.htm"))
            {
                if (!dates.Contains(file.LastWriteTime.ToString("yyyy/MM/dd")))
                {
                    dates.Add(file.LastWriteTime.ToString("yyyy/MM/dd"));
                }
            }
            dates.Sort();
            dates.Reverse();
            if (DDL_Date.Items.Count == 0)
            {
                DDL_Date.Items.Add(new System.Web.UI.WebControls.ListItem("Select a date..."));

                foreach (var date in dates)
                {
                    DDL_Date.Items.Add(new System.Web.UI.WebControls.ListItem(date.ToString()));
                }
            }
        }
        
        protected void DDL_Date_SelectedIndexChanged(object sender, EventArgs e)
        {
            DDL_Server.Items.Clear();
            DDL_Server.Items.Add(new System.Web.UI.WebControls.ListItem("Select a server... "));

            DirectoryInfo di = new DirectoryInfo(ConfigurationManager.AppSettings["File_Path"]);

            foreach (var file in di.GetFiles("*.htm"))
            {
                if (file.LastWriteTime.ToString("yyyy/MM/dd") == DDL_Date.SelectedValue)
                {
                    LblArchNum.Text = new string(file.Name.TakeWhile(c => !Char.IsLetter(c)).ToArray());
                    if (LblArchNum.Text.Length > 1)
                    {
                        string newname = file.Name.Replace(LblArchNum.Text, "");
                        DDL_Server.Items.Add(new System.Web.UI.WebControls.ListItem(newname.Replace(".xml.htm", "")));
                        archive = true;
                    }
                    else
                    {
                        DDL_Server.Items.Add(new System.Web.UI.WebControls.ListItem(file.Name.Replace(".xml.htm", "")));
                        archive = false;
                    }
                }
            }
        }
        protected void DDL_Server_SelectedIndexChanged(object sender, EventArgs e)
        {
            string content,filename;
            DirectoryInfo di = new DirectoryInfo(ConfigurationManager.AppSettings["File_Path"]);

            foreach (var file in di.GetFiles("*.htm"))
            {
                if(archive)
                {
                    filename = LblArchNum.Text + file.Name;
                }
                else
                {
                    filename = file.Name;
                }
                if (file.LastWriteTime.ToString("yyyy/MM/dd") == DDL_Date.SelectedValue && filename.Replace(".xml.htm", "") == LblArchNum.Text+DDL_Server.SelectedValue)
                {
                    content = File.ReadAllText(file.FullName);
                    contents.InnerHtml = content;
                    break;
                }
            }
        }
        private string GenerateNumber()
        {
            Random random = new Random();
            string r = "";
            int i;
            for (i = 1; i < 11; i++)
            {
                r += random.Next(0, 9).ToString();
            }
            return r;
        }
        protected void btnPDFExport_Click(object sender, EventArgs e)
        {
            if (!DDL_Server.SelectedValue.Contains("Data is currently") && contents.InnerHtml.Length != 0)
            {
                try
                {
                    string tempname = GenerateNumber();
                    PdfDocument Doc = PdfGenerator.GeneratePdf(contents.InnerHtml, PdfSharp.PageSize.Letter);
                    
                    if (!(Directory.Exists(ConfigurationManager.AppSettings["Temp_Dir"])))
                    {
                        LblError.Text = "Configured Temp file path doesn't exist!";
                        LblError.Visible = true;
                        return;
                    }
                    else
                    {
                        Doc.Save(ConfigurationManager.AppSettings["Temp_Dir"] + "\\" + tempname + ".pdf");
                        string url;
                        if (ConfigurationManager.AppSettings["Page_URL"].StartsWith("http://") || ConfigurationManager.AppSettings["Page_URL"].StartsWith("https://"))
                        {
                            url = ConfigurationManager.AppSettings["Page_URL"];
                        }
                        else
                        {
                            if (ConfigurationManager.AppSettings["SSL_Enabled"].ToLower() == "true")
                            {
                                url = "https://" + ConfigurationManager.AppSettings["Page_URL"];
                            }
                            else
                            {
                                url = "http://" + ConfigurationManager.AppSettings["Page_URL"];
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LblError.Text = "Error: "+ ex.Message.ToString();
                    LblError.Visible = true;
                    return;
                }
            }
        }
    }
}