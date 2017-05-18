using HtmlAgilityPackTest.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlAgilityPackTest.DAL
{
    class MissingCaseDataContext : DbContext
    {
        public MissingCaseDataContext()
            : base("name=MissingCaseDataContext")
        {
            Database.SetInitializer<MissingCaseDataContext>(null);
        }
        public virtual DbSet<Threads> Threads { get; set; }
        public virtual DbSet<MissingCaseModel> MissingCaseModels { get; set; }
        public virtual DbSet<Recipient> Recipients { get; set; }
        public virtual DbSet<MainLandForum> MainLandForums { get; set; }
    }
}
