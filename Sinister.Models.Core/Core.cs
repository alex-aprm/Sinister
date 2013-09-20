using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sinister.Models.Core
{
    public abstract class Entity
    {
        public Entity()
        {
        }

        [Key]
        public Guid Gid { get; set; }

        public virtual void Validate()
        {
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
        public string Name { get; set; }
        [MaxLength(200)]
        public string SID { get; set; }
        public virtual IList<DictionaryRecord> Records { get; set; }
        public int RecordsCount {get { return this.Records.Count(); }}
    }
    public class DictionaryRecord :Entity
    {
        public DictionaryRecord()
        {
            this.Gid = Guid.NewGuid();
        }
        public virtual Dictionary Dictionary { get; set; }
        public int OrderNumber { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string SID { get; set; }
    }
}
