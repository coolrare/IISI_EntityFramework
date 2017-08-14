using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IISIConsole.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public partial class ContosoUniversityEntities : DbContext
    {
        public override int SaveChanges()
        {
            var entities = this.ChangeTracker.Entries();

            foreach (var entity in entities)
            {
                if (entity.Entity is Course && 
                    entity.State == EntityState.Modified)
                {
                    entity.CurrentValues.SetValues(new {
                        DateModified = DateTime.Now
                    });
                }
            }

            return base.SaveChanges();
        }
    }
}
