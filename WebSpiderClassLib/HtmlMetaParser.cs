﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using System.IO;


namespace WebSpiderClassLib
{
    public class HtmlMetaParser
    {
        public enum RobotHtmlMeta
        {
            None = 0, NoIndex, NoFollow, NoIndexNoFollow
        };

        public static List<HtmlMeta> Parse(string htmldata)
        {
            Regex metaregex =
                new Regex(@"<meta\s*(?:(?:\b(\w|-)+\b\s*(?:=\s*(?:""[^""]*""|'" +
                          @"[^']*'|[^""'<> ]+)\s*)?)*)/?\s*>",
                          RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);

            List<HtmlMeta> MetaList = new List<HtmlMeta>();
            foreach (Match metamatch in metaregex.Matches(htmldata))
            {
                HtmlMeta mymeta = new HtmlMeta();

                Regex submetaregex =
                    new Regex(@"(?<name>\b(\w|-)+\b)\" +
                              @"s*=\s*(""(?<value>" +
                              @"[^""]*)""|'(?<value>[^']*)'" +
                              @"|(?<value>[^""'<> ]+)\s*)+",
                              RegexOptions.IgnoreCase |
                              RegexOptions.ExplicitCapture);

                foreach (Match submetamatch in submetaregex.Matches(metamatch.Value.ToString()))
                {
                    //System.Windows.Forms.MessageBox.Show(submetamatch.Groups["name"].ToString());
                    //System.Windows.Forms.MessageBox.Show(submetamatch.Groups["name"].ToString() + "\r\n" + submetamatch.Groups["value"].ToString());
                    /*if (submetamatch.Groups["name"].ToString().ToLower() == "content")
                    {
                        System.Windows.Forms.MessageBox.Show(submetamatch.Groups["value"].ToString().ToLower());
                        //mymeta.HttpEquiv = submetamatch.Groups["value"].ToString();
                        //File.WriteAllText(Directory.GetCurrentDirectory() + "\\WebSource.txt", submetamatch.Groups["value"].ToString());
                    }*/

                    if (submetamatch.Groups["value"].ToString().ToLower() == "keywords")
                    {
                        System.Windows.Forms.MessageBox.Show(submetamatch.NextMatch().Groups["value"].ToString().ToLower());
                        //System.Windows.Forms.MessageBox.Show(submetamatch.Groups["value"].ToString().ToLower());
                        //File.WriteAllText(Directory.GetCurrentDirectory() + "\\WebSource.txt", submetamatch.Groups["value"].ToString());
                    }
                        
                    /*if ("http-equiv" == submetamatch.Groups["name"].ToString().ToLower())
                        mymeta.HttpEquiv = submetamatch.Groups["value"].ToString();

                    if (("name" == submetamatch.Groups["name"].ToString().ToLower()) && (mymeta.HttpEquiv == String.Empty))
                        mymeta.Name = submetamatch.Groups["value"].ToString();

                    if ("scheme" == submetamatch.Groups["name"].ToString().ToLower())
                        mymeta.Scheme = submetamatch.Groups["value"].ToString();

                    if ("content" == submetamatch.Groups["name"].ToString().ToLower())
                    {
                        mymeta.Content = submetamatch.Groups["value"].ToString();
                        MetaList.Add(mymeta);
                    }*/
                }
            }
            return MetaList;
        }

        public static RobotHtmlMeta ParseRobotMetaTags(string htmldata)
        {
            List<HtmlMeta> MetaList = HtmlMetaParser.Parse(htmldata);

            RobotHtmlMeta result = RobotHtmlMeta.None;
            foreach (HtmlMeta meta in MetaList)
            {
                if (meta.Name.ToLower().IndexOf("robots") != -1 ||
                        meta.Name.ToLower().IndexOf("robot") != -1)
                {
                    string content = meta.Content.ToLower();
                    if (content.IndexOf("noindex") != -1 &&
                        content.IndexOf("nofollow") != -1)
                    {
                        result = RobotHtmlMeta.NoIndexNoFollow;
                        break;
                    }
                    if (content.IndexOf("noindex") != -1)
                    {
                        result = RobotHtmlMeta.NoIndex;
                        break;
                    }
                    if (content.IndexOf("nofollow") != -1)
                    {
                        result = RobotHtmlMeta.NoFollow;
                        break;
                    }
                }
            }
            return result;
        }

    }
}
