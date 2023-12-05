using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using DemoNancy.Model;

namespace DemoNancy
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("MyDbConnection")
        {

        }

        public DbSet<Task> Tasks { get; set; }       
    }
}