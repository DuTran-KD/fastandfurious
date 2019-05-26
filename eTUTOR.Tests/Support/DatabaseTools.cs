using eTUTOR.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTUTOR.Tests.Support
{
    public static class DatabaseTools
    {
        [AssemblyInitialize]
        public static void CleanDatabase(TestContext context)
        {
            using(var db = new DatabaseDbContext())
            {
                //db.students.RemoveRange(db.students);
                //db.parents.RemoveRange(db.parents);
                //db.tutors.RemoveRange(db.tutors);
                db.SaveChanges();
            }
        }
    }
}
