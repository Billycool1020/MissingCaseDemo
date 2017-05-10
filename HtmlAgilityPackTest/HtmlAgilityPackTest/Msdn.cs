using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlAgilityPackTest
{
    class Msdn
    {
        public void MsdnRoot()
        {
            
            List<string> forums = new List<string>();
            forums.Add("wcf");
            foreach (var f in forums)
            {
                for(var i = 1; i < 15; i++)
                {
                    MsdnSub(f, i);
                }

            }
        }
        public void MsdnSub(string Forum, int page)
        {
            PostedTime pt = new PostedTime();
            DateTime PostDate;
            string time;
            var msdnroot = "https://social.msdn.microsoft.com/Forums/en-US/home?forum=";
            var fliterstring = "&filter=alltypes&sort=lastpostdesc&brandIgnore=true&page=";
            var url = msdnroot + Forum + fliterstring + page.ToString();
            url = "https://social.msdn.microsoft.com/Forums/en-US/home?filter=alltypes&sort=lastpostdesc&brandIgnore=true&page="+page;
            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load(url);
            HtmlNode[] nodes = document.DocumentNode.SelectNodes("//a[@data-block='main']").ToArray();
            foreach (HtmlNode item in nodes)
            {
                HtmlNode timenode = item.ParentNode.ParentNode.SelectSingleNode("//span[@class='timeago smallgreytext'][2]");
                if (timenode.InnerText.Split(',').Length > 1)
                {
                    time = timenode.InnerText.Split(',')[1];
                }else
                {
                    time = timenode.InnerText;
                }
                PostDate=pt.Caltime(time);
                Console.WriteLine(item.InnerText);
                Console.WriteLine(PostDate);
                Console.WriteLine();


            }
        }
    }
}
