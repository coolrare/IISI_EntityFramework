using IISIConsole.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
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
                //db.Configuration.ProxyCreationEnabled = false;

                //db.Database.Log = Console.WriteLine;

                var one = db.Course.Find(2);

                one.Title += "1";
                //one.DateModified = DateTime.Now;

                db.SaveChanges();
            }



            //DemoEntityState(db);
            //DemoManyToMany(db);
            //DemoQueryCount(db);
            //DemoInsert(db);
            //DemoUpdate(db);
            //DemoDelete(db);
            //DemoQuery(db);
        }

        private static void DemoEntityState(ContosoUniversityEntities db)
        {
            var one = db.Course.Find(2);
            Console.WriteLine(db.Entry(one).State);

            one.Credits++;
            Console.WriteLine(db.Entry(one).State);

            db.Entry(one).State = EntityState.Added;

            db.SaveChanges();
        }

        private static void DemoManyToMany(ContosoUniversityEntities db)
        {
            var c = db.Course.Find(2);
            var p = db.Person.Find(3);
            //p = new Person() { FirstName = "Will", LastName = "Huang", HireDate = DateTime.Now, Discriminator = "OK" };
            c.Person.Add(p);
            db.SaveChanges();
        }

        private static void DemoQueryCount(ContosoUniversityEntities db)
        {
            var data = db.Department.ToList();

            foreach (var item in data)
            {
                Console.WriteLine(item.Name + item.Course.Count());
            }
        }

        private static void DemoDelete(ContosoUniversityEntities db)
        {
            var dd = db.Department.Find(14);
            db.Department.Remove(dd);
            db.SaveChanges();
        }

        private static void DemoUpdate(ContosoUniversityEntities db)
        {
            var d1 = db.Department.Find(1);
            d1.Budget = d1.Budget + 1;

            d1.ModifiedOn = DateTime.Now;

            db.SaveChanges();
        }

        private static void DemoInsert(ContosoUniversityEntities db)
        {
            var d = new Department()
            {
                DepartmentID = 0,
                Name = "My Dept",
                Budget = 100,
                StartDate = DateTime.Now,
                ModifiedOn = DateTime.Now
            };

            db.Department.Add(d);
            db.SaveChanges();

            Console.WriteLine("InsertedID = " + d.DepartmentID);
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
