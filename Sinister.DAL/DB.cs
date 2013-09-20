using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using Sinister.DAL.Migrations;
using Sinister.Models.Core;
using Sinister.Models.CRM;

namespace Sinister.DAL
{
    public class Db:DbContext
    {
        public Db(string connectionString)
            : base(connectionString)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Db, Configuration>());
        }
        public Db()
            : base("Data Source=pc-alex;Initial Catalog=Sinister;Integrated Security=true") //Только для миграций из Студии
        {
            //throw new Exception(this.Database.Connection.ConnectionString.ToString());
        }

        public DbSet<Dictionary> Dictionaries { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DictionaryRecord>().HasRequired(e => e.Dictionary).WithMany(d => d.Records).WillCascadeOnDelete(true);

            base.OnModelCreating(modelBuilder);
        }
    }
}
