using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sinister.DAL;
using Sinister.Global;
using Sinister.Models.Core;
using System.Reflection;

namespace Sinister.Controllers
{
    public class BaseController : Controller
    {
        public BaseController() : base()
        {
            string cstr = God.GetConnectionString("SinisterConnection");
            this.repository = new Repository(cstr);
        }
        protected Repository repository { get; set; }
    }

    public class CRUDController<E,R> : BaseController where E : Entity, new() where R : EntityRepository<E>
    {
        protected R entityRepository;
        public CRUDController():base()
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
            entity = ProcessSubAction(entity, SubAction, SubGid);
            if (SubAction == "")
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        this.entityRepository.Save(entity);
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
            return View(entity);
        }

        //
        // GET: /Dictionaries/Edit/5

        public ActionResult Edit(Guid gid)
        {
            E e = this.entityRepository.Get(gid);
            return View(e);
        }

        //
        // POST: /Dictionaries/Edit/5

        [HttpPost]
        public virtual ActionResult Edit(E entity, string SubAction, Guid? SubGid)
        {
            entity = ProcessSubAction(entity, SubAction, SubGid);
            if (SubAction == "")
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        this.entityRepository.Save(entity);
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
            return View(entity);
        }

        //
        // GET: /Dictionaries/Delete/5

        public virtual ActionResult Delete(Guid gid)
        {
            E e = this.entityRepository.Get(gid);
            return View(e);
        }

        //
        // POST: /Dictionaries/Delete/5

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
    }
}
