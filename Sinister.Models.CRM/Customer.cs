using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Sinister.Models.Core;

namespace Sinister.Models.CRM
{
    public class Customer : Entity
    {
        public Customer()
        {
            this.Gid = Guid.NewGuid();
        }
        [MaxLength(100)]
        public string Code { get; set; }
        [MaxLength(500)]
        public string LastName { get; set; }
        [MaxLength(500)]
        public string FirstName { get; set; }
        [MaxLength(500)]
        public string MiddleName { get; set; }
        [MaxLength(20)]
        public string INN { get; set; }
        public bool IsFullCustomer { get; set; }
        public IList<Identify> Idenities { get; set; }
    }

    public class Identify : Entity
    {
        public Identify()
        {
            this.Gid = Guid.NewGuid();
        }
        public virtual Customer Customer { get; set; }
        public virtual DictionaryRecord IdentifyType { get; set; }

        [MaxLength(50)]
        public string Series { get; set; }

        [MaxLength(50)]
        public string Number { get; set; }

        public DateTime IssueDate { get; set; }
        public DateTime? EndDate { get; set; }

        [MaxLength(500)]
        public string Issuer { get; set; }

        [MaxLength(50)]
        public string IssuerCode { get; set; }

        public bool IsValid { get; set; }
        public bool IsMain { get; set; }
    }

}
