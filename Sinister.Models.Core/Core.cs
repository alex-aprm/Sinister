using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using AutoMapper;
using System.Reflection;
using System.Data;

namespace Sinister.Models.Core
{
    public abstract class Entity
    {
        protected Entity()
        {

        }

        [Key]
        public Guid Gid { get; set; }
        [MaxLength(200)]
        public virtual string Name { get; set; }

        public virtual void Validate()
        {
        }

        public Entity GetRidOfProxies()
        {
            Type t = this.GetType().BaseType;
            if (t.Name == "Entity")
                t = this.GetType();
            Mapper.CreateMap(t,t);
            foreach (PropertyInfo pp in t.GetProperties().Where(p=>p.CanWrite && !(p.GetCustomAttributes(false).Any(a=>a is XmlIgnoreAttribute))))
            {
                if (pp.PropertyType.BaseType == typeof(Entity))
                {
                    Entity ce = (Entity)pp.GetValue(this);
                    ce = ce.GetRidOfProxies();
                    pp.SetValue(this, ce);
                }
                if (pp.PropertyType.IsGenericType)
                {
                    if (pp.PropertyType.GenericTypeArguments.FirstOrDefault(g => g.BaseType == typeof (Entity)) != null)
                    {
                        ConstructorInfo ci = pp.PropertyType.GetConstructor(new Type[] { });
                        IList dstlist = (IList)ci.Invoke(new object[] { });
                        foreach (Entity ce in (IList)pp.GetValue(this))
                        {
                            Entity cec = ce.GetRidOfProxies();

                            dstlist.Add(cec);
                        }
                        pp.SetValue(this, dstlist);
                    }
                }
            }
            Entity e = (Entity)Mapper.Map(this,t, t);
            return e;
        }
    }
    public class Dictionary :Entity 
    {
        public Dictionary()
        {
            this.Records = new List<DictionaryRecord>();
            this.Gid = Guid.NewGuid();
        }
        [MaxLength(200)]
        public string SID { get; set; }
        public virtual List<DictionaryRecord> Records { get; set; }
        public int RecordsCount {get { return this.Records.Count(); }}
        [NotMapped]
        public List<DictionaryRecord> RecordsWithEmpty
        {
            get
            {
                List<DictionaryRecord> r = this.Records;
                r.Add(new DictionaryRecord {OrderNumber = -1});
                return r;
            }
        }
    }
    public class DictionaryRecord :Entity
    {
        public DictionaryRecord()
        {
            this.Gid = Guid.NewGuid();
        }
        [XmlIgnore]
        public virtual Dictionary Dictionary { get; set; }
        [XmlIgnore]
        public Guid DictionaryGid { get; set; }
        public int OrderNumber { get; set; }
        [MaxLength(200)]
        public string SID { get; set; }
    }
}
