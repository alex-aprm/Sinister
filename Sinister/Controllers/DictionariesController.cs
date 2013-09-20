using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sinister.DAL;
using Sinister.Models.Core;

namespace Sinister.Controllers
{
    public class DictionariesController : BaseController
    {
        //
        // GET: /Dictionaries/

        public ActionResult Index()
        {
            List<Dictionary> l =repository.Dictionaries.GetAll();
            return View(l);
        }

       
        public ActionResult Create()
        {
            return View(new Dictionary());
        }

        [HttpPost]
        public ActionResult Create(Dictionary dictionary, string SubAction, Guid? RecordGid)
        {
            dictionary = ProcessSubAction(dictionary, SubAction, RecordGid);
            if (SubAction == "")
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        repository.Dictionaries.Save(dictionary);
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
            return View(dictionary);
        }

        //
        // GET: /Dictionaries/Edit/5

        public ActionResult Edit(Guid gid)
        {
            Dictionary d = repository.Dictionaries.Get(gid);
            return View(d);
        }

        //
        // POST: /Dictionaries/Edit/5

        [HttpPost]
        public ActionResult Edit(Dictionary dictionary, string SubAction, Guid? RecordGid)
        {
            dictionary = ProcessSubAction(dictionary, SubAction, RecordGid);
            if (SubAction == "")
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        repository.Dictionaries.Save(dictionary);
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
            return View(dictionary);
        }

        //
        // GET: /Dictionaries/Delete/5

        public ActionResult Delete(Guid gid)
        {
            Dictionary d = repository.Dictionaries.Get(gid);
            return View(d);
        }

        //
        // POST: /Dictionaries/Delete/5

        private Dictionary ProcessSubAction(Dictionary dictionary, string SubAction, Guid? RecordGid)
        {
            ModelState.Clear();
            switch (SubAction)
            {
                case "AddRecord":
                    dictionary.Records.Add(new DictionaryRecord());
                    break;
                case "RemoveRecord":
                    DictionaryRecord RecordToRemove = dictionary.Records.First(s => s.Gid == (RecordGid??Guid.Empty));
                    dictionary.Records.Remove(RecordToRemove);
                    break;
                case "MoveRecordUp":
                    DictionaryRecord RecordToMoveUp = dictionary.Records.First(s => s.Gid == (RecordGid ?? Guid.Empty));
                    int IndexToMoveUp = dictionary.Records.IndexOf(RecordToMoveUp);
                    if (IndexToMoveUp > 0)
                    {
                        dictionary.Records[IndexToMoveUp] = dictionary.Records[IndexToMoveUp - 1];
                        dictionary.Records[IndexToMoveUp - 1] = RecordToMoveUp;
                    }
                    break;
                case "MoveRecordDown":
                    DictionaryRecord RecordToMoveDown = dictionary.Records.First(s => s.Gid == (RecordGid ?? Guid.Empty));
                    int IndexToMoveDown = dictionary.Records.IndexOf(RecordToMoveDown);
                    if (IndexToMoveDown < dictionary.Records.Count() - 1)
                    {
                        dictionary.Records[IndexToMoveDown] = dictionary.Records[IndexToMoveDown + 1];
                        dictionary.Records[IndexToMoveDown + 1] = RecordToMoveDown;
                    }
                    break;
                case "SortByName":
                    dictionary.Records = dictionary.Records.OrderBy(r => r.Name).ToList();
                    break;
            }
            foreach (DictionaryRecord s in dictionary.Records) //Выправим нумерацию
            {
                s.OrderNumber = dictionary.Records.IndexOf(s) + 1;
            }

            return dictionary;
        }


        [HttpPost]
        public ActionResult Delete(Dictionary dictionary)
        {
            try
            {
                repository.Dictionaries.Delete(dictionary);
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
            return View(dictionary);
        }
    }
}
