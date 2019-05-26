using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace eTUTOR.Models
{
    public class DatabaseDbContext: DbContext
    {
        public DatabaseDbContext()
            : base(@"data source= capstone2019; initial catalog = eTUITORModel; Integrated Security = True ")
        {

        }

        public DbSet<student> tutors { get; set; }
        public DbSet<parent> parents { get; set; }
    }
}