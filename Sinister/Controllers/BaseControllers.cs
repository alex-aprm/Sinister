using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using Sinister.DAL;
using Sinister;
using Sinister.Models.Core;
using System.Reflection;

namespace Sinister.Controllers
{
    public abstract class BaseController : Controller
    {
        public BaseController() : base()
        {
            string cstr = Sinister.God.GetConnectionString("SinisterConnection");
            this.repository = new Repository(cstr);
        }
        protected Repository repository { get; set; }
    }

    public abstract class CRUDController<E,R> : BaseController where E : Entity, new() where R : EntityRepository<E>
    {
        protected R entityRepository;
        protected CRUDController():base()
        {
            foreach (PropertyInfo pp in repository.GetType().GetProperties())
            {
                if (pp.PropertyType.BaseType == typeof(EntityRepository<E>))
                {
                    entityRepository = (pp.GetValue(repository) as R);
                }
            }
        }
        public virtual ActionResult Index()
        {
            List<E> l = entityRepository.GetAll();
            return View(l);
        }

        public virtual ActionResult Create()
        {
            return View(new E());
        }

        [HttpPost]
        public virtual ActionResult Create(E entity, string SubAction, Guid? SubGid)
        {
            entity = (E)ReadDictionaryProps(entity);
            if ((SubAction??"") == "")
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        this.entityRepository.Save((E)entity.GetRidOfProxies());
                        return RedirectToAction("Index");
                    }
                    catch (ValidationException ex)
                    {
                        foreach (ValidationError err in ex.FieldErrors)
                        {
                            ModelState.AddModelError(err.Field, err.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
                }
            }
            else entity = ProcessSubAction(entity, SubAction, SubGid);
            return View(entity);
        }

        public ActionResult Edit(Guid gid)
        {
            E e = this.entityRepository.Get(gid);
            return View(e);
        }

        [HttpPost]
        public virtual ActionResult Edit(E entity, string SubAction, Guid? SubGid)
        {
            entity = (E) ReadDictionaryProps(entity);
            if ((SubAction??"") == "")
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        this.entityRepository.Save((E) entity.GetRidOfProxies());
                        return RedirectToAction("Index");
                    }
                    catch (ValidationException ex)
                    {
                        foreach (ValidationError err in ex.FieldErrors)
                        {
                            ModelState.AddModelError(err.Field, err.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        string InnerMessage = "";
                        if (ex.InnerException != null) InnerMessage = " (" + ex.InnerException.Message + ")";
                        ModelState.AddModelError("", ex.Message + InnerMessage);
                    }
                }
            } else entity = ProcessSubAction(entity, SubAction, SubGid);
            return View(entity);
        }

        protected object ReadDictionaryProps(object entity)
        {
            if (entity == null) return null;
            if (entity.GetType().FullName == "Sinister.Models.Core.Dictionary") return entity;
            foreach (PropertyInfo pp in entity.GetType().GetProperties())
            {
                if (pp.PropertyType.FullName == "Sinister.Models.Core.DictionaryRecord")
                {
                    PropertyInfo ppg = entity.GetType().GetProperty(pp.Name + "Gid");
                    DictionaryRecord d = (DictionaryRecord)pp.GetValue(entity);
                    if (d != null)
                        d = (DictionaryRecord) repository.Dictionaries.GetRecord(d.Gid);
                    else
                    {
                        d = (DictionaryRecord)repository.Dictionaries.GetRecord(((Guid?)ppg.GetValue(entity))??Guid.Empty);
                    }
                    if (d != null) d = (DictionaryRecord)d.GetRidOfProxies();
                    pp.SetValue(entity, d);
                } else
                if (pp.PropertyType.BaseType!=null && pp.CanWrite)
                    if (pp.PropertyType.BaseType.FullName == "Sinister.Models.Core.Entity")
                    { 
                        Entity e = (Entity)pp.GetValue(entity);
                        e = (Entity)ReadDictionaryProps(e);
                        pp.SetValue(entity, e);
                    }
                if (pp.PropertyType.IsGenericType)
                {
                    if (pp.PropertyType.GenericTypeArguments.FirstOrDefault(g => g.BaseType == typeof(Entity)) != null)
                    {
                        ConstructorInfo ci = pp.PropertyType.GetConstructor(new Type[] { });
                        IList dstlist = (IList)ci.Invoke(new object[] { });
                        foreach (Entity ce in (IList)pp.GetValue(entity))
                        {
                            Entity cec = (Entity)ReadDictionaryProps(ce);
                            dstlist.Add(cec);
                        }
                        pp.SetValue(entity, dstlist);
                    }
                }

            }
            return entity;
        }

        public virtual ActionResult Delete(Guid gid)
        {
            E e = this.entityRepository.Get(gid);
            return View(e);
        }

        protected virtual E ProcessSubAction(E entity, string SubAction, Guid? SubGid)
        {
            return entity;
        }


        [HttpPost]
        public ActionResult Delete(E entity)
        {
            try
            {
                this.entityRepository.Delete(entity);
                return RedirectToAction("Index");
            }
            catch (ValidationException ex)
            {
                foreach (ValidationError err in ex.FieldErrors)
                {
                    ModelState.AddModelError(err.Field, err.Error);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(entity);
        }

        public FileContentResult Xml(Guid gid)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");


            E d = (E)entityRepository.Get(gid).GetRidOfProxies();
            XmlSerializer serializer = new XmlSerializer(typeof(E));

            MemoryStream str = new MemoryStream();
            serializer.Serialize(str, d,ns);
            byte[] content = new byte[str.Length];
            str.Position = 0;
            str.Read(content, 0, (int)str.Length);
            str.Close();
            return File(content, "Xml", d.Name + ".xml");
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.repository = this.repository;
            base.OnActionExecuting(filterContext);
        }
    }
}
