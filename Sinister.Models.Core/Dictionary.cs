using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sinister.Models.Core
{
    public class Dictionary : Entity
    {
        public Dictionary()
        {
            this.Records = new List<DictionaryRecord>();
            this.Gid = Guid.NewGuid();
        }
        [MaxLength(200)]
        public string SID { get; set; }
        public virtual List<DictionaryRecord> Records { get; set; }
        [XmlIgnore]
        [NotMapped]
        public int RecordsCount { get { return this.Records.Count(); } }
        [XmlIgnore]
        [NotMapped]
        public List<DictionaryRecord> RecordsWithEmpty
        {
            get
            {
                List<DictionaryRecord> r = new List<DictionaryRecord>();
                r.AddRange(this.Records);
                r.Add(new DictionaryRecord { Gid=Guid.Empty, OrderNumber = -1 });
                return r;
            }
        }
    }
    public class DictionaryRecord : Entity
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
