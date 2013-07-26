using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using AHome.Models;
using AHome.Models.Base;

namespace AHome.DAL
{
    public abstract class BaseDAL
    {
        public RogerContext db = new RogerContext();

        public int Update<T>(T entity, int id, RogerContext dbParm = null) where T : IEntity
        {
            if (dbParm == null)
            {
                dbParm = db;
            }
            var entry = dbParm.Entry<T>(entity);

            if (entry.State == EntityState.Detached)
            {
                var set = dbParm.Set<T>();
                T attachedEntity = set.Find(id);  // You need to have access to key

                if (attachedEntity != null)
                {
                    var attachedEntry = dbParm.Entry(attachedEntity);
                    attachedEntry.CurrentValues.SetValues(entity);
                }
                else
                {
                    entry.State = EntityState.Modified; // This should attach entity
                }
            }
            return dbParm.SaveChanges();
        }
    }
}