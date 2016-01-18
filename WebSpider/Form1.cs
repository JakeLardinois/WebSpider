using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using System.IO;

using WebSpiderClassLib;



namespace WebSpider
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            //string strSource = SpiderDataGetter.getWebPageSourceCode(txtURL.Text);
            StringBuilder strbldrKeywords = new StringBuilder();
            StringBuilder strbldrSourceCode = new StringBuilder();
            DataTable objDataTable;
            

            //File.WriteAllText(Directory.GetCurrentDirectory() + "\\Keywords.txt", SpiderDataGetter.getKeywords(strSource));
            //File.WriteAllText(Directory.GetCurrentDirectory() + "\\WebSource.txt", strSource);


            objDataTable = SpiderDataGetter.getWebSites();

            foreach (DataRow objRow in objDataTable.Rows)
            {
                strbldrKeywords.Clear();
                strbldrSourceCode.Clear();

                if (!objRow["URL"].ToString().Equals(""))
                {
                    strbldrSourceCode.Append(SpiderDataGetter.getWebPageSourceCode(objRow["URL"].ToString()));
                    strbldrKeywords.Append(SpiderDataGetter.getKeywords(strbldrSourceCode.ToString()));
                    strbldrKeywords.Replace("'", "");

                    if (strbldrKeywords.Length > 3)
                    {
                        SpiderDataGetter.updateKeywords(objRow["ActivityID"].ToString(), strbldrKeywords.ToString());
                        txtMessages.Text = txtMessages.Text + "Updated " + objRow["URL"].ToString() + " keywords\r\n";
                        txtMessages.Refresh();
                    }

                }
                
            }

            //SpiderDataGetter.updateKeywords(1, "Farm is the farm");

            MessageBox.Show("Done");
        }

        
    }
}
