using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using Sinister.DAL;
using Sinister.Models.Core;
using AutoMapper;


namespace Sinister.Controllers
{
    public class DictionariesController : CRUDController<Dictionary, Dictionaries>
    {

        protected override Dictionary ProcessSubAction(Dictionary dictionary, string SubAction, Guid? SubGid, int? SubId)
        {
            switch (SubAction)
            {
                case "AddRecord":
                    dictionary.Records.Add(new DictionaryRecord());
                    ModelState.Clear();
                    break;
                case "RemoveRecord":
                    DictionaryRecord RecordToRemove = dictionary.Records.First(s => s.Gid == (SubGid ?? Guid.Empty));
                    dictionary.Records.Remove(RecordToRemove);
                    ModelState.Clear();
                    break;
                case "MoveRecordUp":
                    DictionaryRecord RecordToMoveUp = dictionary.Records.First(s => s.Gid == (SubGid ?? Guid.Empty));
                    int IndexToMoveUp = dictionary.Records.IndexOf(RecordToMoveUp);
                    if (IndexToMoveUp > 0)
                    {
                        dictionary.Records[IndexToMoveUp] = dictionary.Records[IndexToMoveUp - 1];
                        dictionary.Records[IndexToMoveUp - 1] = RecordToMoveUp;
                    }
                    ModelState.Clear();
                    break;
                case "MoveRecordDown":
                    DictionaryRecord RecordToMoveDown = dictionary.Records.First(s => s.Gid == (SubGid ?? Guid.Empty));
                    int IndexToMoveDown = dictionary.Records.IndexOf(RecordToMoveDown);
                    if (IndexToMoveDown < dictionary.Records.Count() - 1)
                    {
                        dictionary.Records[IndexToMoveDown] = dictionary.Records[IndexToMoveDown + 1];
                        dictionary.Records[IndexToMoveDown + 1] = RecordToMoveDown;
                    }
                    ModelState.Clear();
                    break;
                case "SortByName":
                    dictionary.Records = dictionary.Records.OrderBy(r => r.Name).ToList();
                    ModelState.Clear();
                    break;
                case "MoveRecord":
                    DictionaryRecord RecordToMove = dictionary.Records.First(s => s.Gid == (SubGid ?? Guid.Empty));
                    int IndexToMove = dictionary.Records.IndexOf(RecordToMove);
                    int NewId = SubId ?? 0+1;
                    if (NewId>IndexToMove)
                    {
                        dictionary.Records.Insert(NewId+1, RecordToMove);
                        dictionary.Records.RemoveAt(IndexToMove);
                    }
                    else
                    {
                        dictionary.Records.Insert(NewId, RecordToMove);
                        dictionary.Records.RemoveAt(IndexToMove+1);
                    }
                    ModelState.Clear();
                    break;
            }
            foreach (DictionaryRecord s in dictionary.Records) //Выправим нумерацию
            {
                s.OrderNumber = dictionary.Records.IndexOf(s) + 1;
            }

            return dictionary;
        }

    }
}
