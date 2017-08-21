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
                //var c = db.Course.Find(9);
                //c.Credits = Credits.極品 | Credits.優質;
                //db.SaveChanges();

                db.Database.Log = Console.WriteLine;

                //var data = db.Course.SqlQuery("SELECT * FROM dbo.Course WHERE Credits > @p0", Credits.普通);

                var data = db.Database.SqlQuery<MyCourse>(
                    "SELECT CourseID as ID, Title as Name FROM dbo.Course");

                foreach (var item in data)
                {
                    Console.WriteLine(item.ID + "\t" + item.Name);
                }

                Console.WriteLine(data.Count());

                //Demo延遲載入資料並自定載入邏輯(db);
                //Demo預先載入資料(db);


                //var data = from p in db.Course
                //           where (p.Credits & (Credits.極品 | Credits.普通)) != Credits.無
                //           select p;

                //var data = from p in db.Course
                //           where 
                //               p.Credits.HasFlag(Credits.極品) ||
                //               p.Credits.HasFlag(Credits.普通)
                //           select p;

                //var data = from p in db.Course
                //           where p.Credits >= Credits.優質
                //           select p;

                //foreach (var c in data)
                //{
                //    Console.WriteLine(c.CourseID + "\t" + c.Title + "\t" + c.Credits);
                //}
            }

            //DemoCallSP(db);
            //DemoConcurrencyCheck(db);
            //DemoHackDeleteMethod();
            //DemoEntityStateBetweenDbContext();
            //DemoEntityState(db);
            //DemoManyToMany(db);
            //DemoQueryCount(db);
            //DemoInsert(db);
            //DemoUpdate(db);
            //DemoDelete(db);
            //DemoQuery(db);
        }

        private static void Demo延遲載入資料並自定載入邏輯(ContosoUniversityEntities db)
        {
            db.Configuration.LazyLoadingEnabled = false;
            
            var data = db.Department;
            foreach (var item in data)
            {
                if (item.IsDeleted == false)
                {
                    db.Entry(item).Collection(p => p.Course).Load();
                }
                Console.WriteLine(item.Name + "\t課程數" + item.Course.Count());
            }
        }

        private static void Demo預先載入資料(ContosoUniversityEntities db)
        {
            var data = db.Department.Include(p => p.Course);

            foreach (var item in data)
            {
                var a = item.Course.Count(p => p.Credits > Credits.普通);
            }
        }

        private static void DemoCallSP(ContosoUniversityEntities db)
        {
            var data = db.GetCourses();
            foreach (var item in data)
            {
                Console.WriteLine(item.Title + " " + item.Name);
            }
        }

        private static void DemoConcurrencyCheck(ContosoUniversityEntities db)
        {
            db.Database.Log = Console.WriteLine;
            var d = db.Department.Find(1);
            d.Budget = d.Budget + 1;
            Console.ReadLine();
            db.SaveChanges();
        }

        private static void DemoHackDeleteMethod()
        {
            using (var db = new ContosoUniversityEntities())
            {
                //var c = db.Course.Find(19);
                //db.Course.Remove(c);

                db.Entry(new Course() { CourseID = 19 }).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }

        private static void DemoEntityStateBetweenDbContext()
        {
            Course c;

            using (var db = new ContosoUniversityEntities())
            {
                c = db.Course.Find(2);
                Console.WriteLine(db.Entry(c).State);
                c.Credits++;
                Console.WriteLine(db.Entry(c).State);
            }

            using (var db = new ContosoUniversityEntities())
            {
                Console.WriteLine(db.Entry(c).State);
                db.Course.Attach(c);
                Console.WriteLine(db.Entry(c).State);
                db.Entry(c).State = EntityState.Modified;
                Console.WriteLine(db.Entry(c).State);
            }
        }

        private static void DemoDateModified(ContosoUniversityEntities db)
        {
            var one = db.Course.Find(2);

            one.Title += "1";
            //one.DateModified = DateTime.Now;

            db.SaveChanges();
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
