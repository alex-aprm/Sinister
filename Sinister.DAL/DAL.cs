using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using RefactorThis.EFExtensions;
using Sinister.Models.Core;
using Sinister.Models.CRM;
using System.Reflection;

namespace Sinister.DAL
{
    public abstract class EntityRepository<T> where T : Entity
    {
        protected EntityRepository(Db db)
        {
            this.db = db;
        }
        protected Expression<Func<IUpdateConfiguration<T>, object>> UpdGraph { get; set; } 
        private Db db;
        public virtual List<T> GetAll()
        {
            return db.Set<T>().AsNoTracking().ToList();
        }
        public virtual T Get(Guid gid)
        {
            return db.Set<T>().AsNoTracking().FirstOrDefault(m=>m.Gid==gid);
        }
        public virtual void Save(T ent)
        {
            ent.Validate();
            T dbent = Get(ent.Gid);
            if (dbent == null)
                db.Set<T>().Add(ent);
            else
            {
                if (UpdGraph == null)
                    db.UpdateGraph(ent);
                else
                    db.UpdateGraph(ent, UpdGraph);
            }
            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                ValidationException E = new ValidationException(ex);
                foreach (DbEntityValidationResult r in ex.EntityValidationErrors)
                {
                    foreach (DbValidationError err in r.ValidationErrors)
                    {
                        E.FieldErrors.Add(new ValidationError(err.PropertyName, err.ErrorMessage));
                    }
                }
                throw E;
            }
            catch (Exception E)
            {
                throw E.InnerException ?? E;
            }
        }

        public virtual void Delete(T ent)
        {
            try
            {

                T dbent = db.Set<T>().Find(ent.Gid);
                db.Set<T>().Remove(dbent);
                db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                ValidationException E = new ValidationException(ex);
                foreach (DbEntityValidationResult r in ex.EntityValidationErrors)
                {
                    foreach (DbValidationError err in r.ValidationErrors)
                    {
                        E.FieldErrors.Add(new ValidationError(err.PropertyName, err.ErrorMessage));
                    }
                }
                throw E;
            }
            catch (Exception E)
            {
                throw E.InnerException ?? E;
            }

        }
    }

    public class Dictionaries : EntityRepository<Dictionary>
    {
        public Dictionaries(Db db)
            : base(db)
        {
            this.UpdGraph = map => map.OwnedCollection(w => w.Records);
        }
        public override Dictionary Get(Guid gid)
        {
            Dictionary d = base.Get(gid);
            if (d != null) d.Records = d.Records.OrderBy(r => r.OrderNumber).ToList();
            return d;
        }
    }
    public class Customers : EntityRepository<Customer>
    {
        public Customers(Db db)
            : base(db)
        {
        }
    }

    public class Repository
    {
        public Repository(string connectionString)
        {
            Db db = new Db(connectionString);
            foreach (PropertyInfo pp in this.GetType().GetProperties())
            {
                ConstructorInfo ci = pp.PropertyType.GetConstructor(new Type[] {typeof(Db)});
                if (ci != null)
                    pp.SetValue(this, ci.Invoke(new object[] {db}));
            }
        }
        public Dictionaries Dictionaries { get; set; }
        public Customers Customers { get; set; }
    }

}
