using HtmlAgilityPack;
using HtmlAgilityPackTest.DAL;
using HtmlAgilityPackTest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlAgilityPackTest.BLL
{
    class MainLandMsdn
    {

        public List<Threads> MainLandMsdnRoot()
        {
            MissingCaseDataContext db = new MissingCaseDataContext();
            List<Threads> list = new List<Threads>();
            var forums = db.MainLandForums.ToList();

            foreach (var f in forums)
            {
                for (var i = 1; i < 3; i++)
                {
                    list.AddRange(MainLandMsdnSub(f.Url, f.Name, i));
                }

            }
            return list;
        }
        public List<Threads> MainLandMsdnSub(string Url, string Name, int page)
        {
            List<Threads> list = new List<Threads>();
            PostedTime pt = new PostedTime();
            string time;
            var fliterstring = "&brandIgnore=true&page=";
            var url = Url + fliterstring + page.ToString();
            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load(url);
            HtmlNode[] nodes = document.DocumentNode.SelectNodes("//a[@data-block='main']").ToArray();
            foreach (HtmlNode item in nodes)
            {
                HtmlNode timenode = item.ParentNode.ParentNode.SelectSingleNode("div[3]/span[7]/span[2]");
                if (timenode.InnerText.Split(',').Length > 1)
                {
                    time = timenode.InnerText.Split(',')[1];
                }
                else
                {
                    time = timenode.InnerText;
                }
                if (pt.RecentPostM(time))
                {
                    Threads thread = new Threads();
                    thread.Link = item.Attributes["href"].Value;
                    thread.PostDate = pt.CaltimeM(time);
                    thread.Product = Name;
                    thread.ThreadId = item.Attributes["data-threadId"].Value;
                    thread.Title = item.InnerText;
                    list.Add(thread);
                    Console.WriteLine(item.InnerText);
                    Console.WriteLine(pt.CaltimeM(time));
                    Console.WriteLine();
                }
            }
            return list;
        }

    }
}
