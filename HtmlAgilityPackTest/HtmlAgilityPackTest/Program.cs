using HtmlAgilityPackTest.BLL;
using HtmlAgilityPackTest.DAL;
using HtmlAgilityPackTest.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;


namespace HtmlAgilityPackTest
{
    class Program
    {
        static MissingCaseDataContext db = new MissingCaseDataContext();
        public static int flag = 0;
        static void Main(string[] args)
        {
            PostedTime.StartTime = DateTime.Now.ToUniversalTime();
            Timer aTimer = new Timer(Start, null, 0, 600000);
            while (true)
            {

            }
        }
        static void Start(object state)
        {

            // Email mail = new Email();

            // mail.Sendmail();
            // Console.WriteLine("Email send " + DateTime.Now.ToUniversalTime());
            //getNewCaseList();
            //if (DateTime.Now.Hour == 9 && flag == 0)
            //{
            MissingCase mc = new MissingCase();
            int a =mc.CheckIsAnswer();
            Console.WriteLine(a.ToString());


            // SaveMissingCase();
            //    flag++;
            //}
            //if (DateTime.Now.Hour == 10 && flag == 1)
            //{
            //    flag--;
            //}

        }


        static void SaveMissingCase()
        {
            Console.WriteLine(DateTime.Now.ToUniversalTime());
            List<MissingCaseModel> MissingList = new List<MissingCaseModel>();
            MissingCase mc = new MissingCase();
            mc.recheck();
            MissingList = mc.getMissingCase();
            MissingList = mc.CheckMissingCase(MissingList);
            Database data = new Database();
            data.Save(MissingList);
            Console.WriteLine(DateTime.Now.ToUniversalTime());
        }

        static void getNewCaseList()
        {

            List<Threads> List = new List<Threads>();
            Console.WriteLine(DateTime.Now.ToUniversalTime());
            //Msdn msdn = new Msdn();
            //List.AddRange(msdn.MsdnRoot());

            MainLandMsdn MainLandmsdn = new MainLandMsdn();
            List.AddRange(MainLandmsdn.MainLandMsdnRoot());

            //Asp asp = new Asp();
            //List.AddRange(asp.AspRoot());

            //IIS iis = new IIS();
            //List.AddRange(iis.IISRoot());

            Console.WriteLine(DateTime.Now.ToUniversalTime());
            Email mail = new Email();

           // mail.Sendmail(List);
            Console.WriteLine("Email send "+DateTime.Now.ToUniversalTime());
        }
    }
}
