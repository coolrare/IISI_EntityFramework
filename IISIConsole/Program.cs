using IISIConsole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IISIConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new ContosoUniversityEntities())
            {
                //db.Configuration.LazyLoadingEnabled = false;

                //db.Database.Log = (string msg) =>
                //{
                //    Console.WriteLine(msg);
                //};

                var d = new Department()
                {
                    DepartmentID = 0,
                    Name = "My Dept",
                    Budget = 100,
                    StartDate = DateTime.Now
                };

                db.Department.Add(d);
                db.SaveChanges();

                Console.WriteLine("InsertedID = " + d.DepartmentID);

                var d1 = db.Department.Find(1);
                d1.Budget = d1.Budget + 1;
                db.SaveChanges();

                var dd = db.Department.Find(14);
                db.Department.Remove(dd);
                db.SaveChanges();
                
                //DemoQuery(db);
            }
        }

        private static void DemoQuery(ContosoUniversityEntities db)
        {
            foreach (var dept in db.Department)
            {
                Console.WriteLine(dept.Name);

                foreach (var course in dept.Course)
                {
                    Console.WriteLine("\t" + course.Title);
                }
            }
        }

        private static IEnumerable<string> GetData()
        {
            for (int i = 0; i < 10; i++)
            {
                yield return i.ToString();
            }
        }
    }
}
