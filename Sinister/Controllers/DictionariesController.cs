using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sinister.DAL;
using Sinister.Models.Core;

namespace Sinister.Controllers
{
    public class DictionariesController : CRUDController<Dictionary,Dictionaries>
    {

        protected override Dictionary ProcessSubAction(Dictionary dictionary, string SubAction, Guid? SubGid)
        {
            ModelState.Clear();
            switch (SubAction)
            {
                case "AddRecord":
                    dictionary.Records.Add(new DictionaryRecord());
                    break;
                case "RemoveRecord":
                    DictionaryRecord RecordToRemove = dictionary.Records.First(s => s.Gid == (SubGid ?? Guid.Empty));
                    dictionary.Records.Remove(RecordToRemove);
                    break;
                case "MoveRecordUp":
                    DictionaryRecord RecordToMoveUp = dictionary.Records.First(s => s.Gid == (SubGid ?? Guid.Empty));
                    int IndexToMoveUp = dictionary.Records.IndexOf(RecordToMoveUp);
                    if (IndexToMoveUp > 0)
                    {
                        dictionary.Records[IndexToMoveUp] = dictionary.Records[IndexToMoveUp - 1];
                        dictionary.Records[IndexToMoveUp - 1] = RecordToMoveUp;
                    }
                    break;
                case "MoveRecordDown":
                    DictionaryRecord RecordToMoveDown = dictionary.Records.First(s => s.Gid == (SubGid ?? Guid.Empty));
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

    }
}
