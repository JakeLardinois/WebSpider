using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;
using System.Net;

using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.IO;

using System.Data.SqlClient;
using System.Data;

namespace WebSpiderClassLib
{

    public static class SpiderDataGetter
    {

        public static string getWebPageSourceCode(string strURL)
        {
            WebClient objWebClient = new WebClient();


            return objWebClient.DownloadString(strURL);
        }

        public static string getKeywords(string strHTMLCode)
        {
            Regex metaregex = new Regex(@"<meta\s*(?:(?:\b(\w|-)+\b\s*(?:=\s*(?:""[^""]*""|'" +
                          @"[^']*'|[^""'<> ]+)\s*)?)*)/?\s*>", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
            List<HtmlMeta> MetaList = new List<HtmlMeta>();


            foreach (Match metamatch in metaregex.Matches(strHTMLCode))
            {
                HtmlMeta mymeta = new HtmlMeta();
                Regex submetaregex = new Regex(@"(?<name>\b(\w|-)+\b)\" +
                              @"s*=\s*(""(?<value>" +
                              @"[^""]*)""|'(?<value>[^']*)'" +
                              @"|(?<value>[^""'<> ]+)\s*)+",
                              RegexOptions.IgnoreCase |
                              RegexOptions.ExplicitCapture);


                foreach (Match submetamatch in submetaregex.Matches(metamatch.Value.ToString()))
                {
                    if (submetamatch.Groups["value"].ToString().ToLower() == "keywords")
                    {
                        //System.Windows.Forms.MessageBox.Show(submetamatch.NextMatch().Groups["value"].ToString().ToLower());
                        //System.Windows.Forms.MessageBox.Show(submetamatch.Groups["value"].ToString().ToLower());
                        //File.WriteAllText(Directory.GetCurrentDirectory() + "\\WebSource.txt", submetamatch.Groups["value"].ToString());
                        return submetamatch.NextMatch().Groups["value"].ToString();
                    }
                }
            }
            return "";
        }

        public static void updateKeywords(string strActivityID, string strKeywords)
        {
            SqlConnection objConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["dcendeavdbConnectionString"].ConnectionString);
            SqlCommand objCommand = objConnection.CreateCommand();

            objConnection.Open();

            objCommand.CommandText = "UPDATE [dcendeavdb].[dcendeavadmin].[Activities] SET [Keywords] = '" + strKeywords + "' WHERE [ActivityID] = " + strActivityID;
            objCommand.ExecuteNonQuery();

            objCommand.Dispose();
            objConnection.Close();
        }

        public static void addKeywords(string strActivityID, string strKeywords)
        {
            SqlConnection objConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["dcendeavdbConnectionString"].ConnectionString);
            SqlCommand objCommand = objConnection.CreateCommand();

            objConnection.Open();

            objCommand.CommandText = "UPDATE [dcendeavdb].[dcendeavadmin].[Activities] SET [Keywords] = Cast([Keywords]as nvarchar(max))  + '" + strKeywords + "' WHERE [ActivityID] = " + strActivityID;
            objCommand.ExecuteNonQuery();

            objCommand.Dispose();
            objConnection.Close();
        }

        public static DataTable getWebSites()
        {
            SqlConnection objConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["dcendeavdbConnectionString"].ConnectionString);
            SqlDataAdapter objDataAdapter;
            string strSQLSelect = "SELECT [ActivityID],[URL],[Keywords] FROM [dcendeavdb].[dcendeavadmin].[Activities]";
            DataSet objDataset = new DataSet();

            objConnection.Open();
            objDataAdapter = new SqlDataAdapter(strSQLSelect, objConnection);
            objDataAdapter.Fill(objDataset);
            objConnection.Close();
            objDataAdapter.Dispose();

            return objDataset.Tables[0];
        }
    }
}
