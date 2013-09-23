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
        [XmlAttribute("Gid")]
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
            foreach (PropertyInfo pp in t.GetProperties().Where(p=>p.CanWrite && !(p.GetCustomAttributes(false).Any(a=>a is XmlIgnoreAttribute || a is NotMappedAttribute))))
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
                        IList srclist = (IList)pp.GetValue(this);
                        foreach (Entity ce in srclist)
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

    public class GuidNotEmptyAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Guid g = ((Guid?)value)?? Guid.Empty;
            string message = FormatErrorMessage(validationContext.DisplayName);
            if (g == Guid.Empty)
            { return new ValidationResult(String.Format(message!=""?message:"Необходимо выбрать значение")); }
            return ValidationResult.Success;
        }
    }

}
