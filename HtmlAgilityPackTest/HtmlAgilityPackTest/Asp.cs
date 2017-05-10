using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlAgilityPackTest
{
    class Asp
    {
        public void AspRoot()
        {
            List<Threads> list = new List<Threads>();
            var asproot = "https://forums.asp.net/";
            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load(asproot);
            HtmlNode[] nodes = document.DocumentNode.SelectNodes("//td[@class='col1']").ToArray();
            foreach (HtmlNode item in nodes)
            {
                if (item.ChildNodes.Count > 1)
                {
                    var subhref = item.ChildNodes[0].FirstChild.Attributes["href"].Value;
                    var subhref1 = subhref.Split('?')[0];
                    subhref1 = subhref1.Remove(subhref1.Length - 1);
                    var subhref2 = subhref.Split('?')[1];
                    for (var i = 1; i < 2; i++)
                    {
                        var url = asproot + subhref1 + i.ToString() + "?";
                        AspSub(url, subhref2);
                    }

                }
            }
        }
        public void AspSub(string url, string subhref)
        {
            PostedTime pt = new PostedTime();
            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load(url + subhref);
            HtmlNode[] nodes = document.DocumentNode.SelectNodes("//a[@title]").ToArray();
            foreach (HtmlNode item in nodes)
            {

                if (item.Attributes["title"].Value.Contains("[Unanswered]") || item.Attributes["title"].Value.Contains("[Answered]"))
                {
                   
                    DateTime PostDate;
                    string Title = item.InnerHtml;
                    Console.WriteLine(Title.Replace("\r\n                ", ""));
                    string Time = item.ParentNode.NextSibling.NextSibling.ChildNodes[4].InnerText;
                    Time = Time.Remove(0, 1);
                    PostDate = pt.Caltime(Time);

                    string Link = "https://forums.asp.net";
                    Link = Link + item.Attributes["href"].Value;
                    Console.WriteLine(Link);
                    Console.WriteLine(PostDate);
                    Console.WriteLine();
                }
            }
        }
    }
}

