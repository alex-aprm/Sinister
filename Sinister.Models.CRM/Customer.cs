using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Sinister.Models.Core;

namespace Sinister.Models.CRM
{
    public class Customer : Entity
    {
        public Customer()
        {
            this.Gid = Guid.NewGuid();
            this.Identifies = new List<Identify>();
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
        public DateTime? BirthDate { get; set; }
        [XmlIgnore]
        [NotMapped]
        public bool BirthDateSpecified { get { return BirthDate.HasValue; } }
        public bool IsFullCustomer { get; set; }
        public virtual List<Identify> Identifies { get; set; }
        public override string Name
        {
            get
            {
                return this.LastName + " " + this.FirstName + " " + this.MiddleName;
            }
        }
        public Identify MainIdentify
        {
            get
            {
                return this.Identifies.FirstOrDefault(i => i.IsMain && i.IsValid);
            }
        }
    }

    public class Identify : Entity
    {
        public Identify()
        {
            this.Gid = Guid.NewGuid();
        }
        public virtual Customer Customer { get; set; }
        public virtual DictionaryRecord Type { get; set; }
        public Guid? TypeGid { get; set; }

        [MaxLength(50)]
        public string Series { get; set; }

        [MaxLength(50)]
        public string Number { get; set; }

        public DateTime? IssueDate { get; set; }
        public DateTime? EndDate { get; set; }

        [MaxLength(500)]
        public string Issuer { get; set; }

        [MaxLength(50)]
        public string IssuerCode { get; set; }

        public bool IsValid { get; set; }
        public bool IsMain { get; set; }
        public override string ToString()
        {
            string s = "";
            if (this.Type != null) s = this.Type.Name+": ";
            s += this.FullNumber;
            return s;
        }

        public string FullNumber
        {
            get
            {
                return (this.Series + " " + this.Number).Trim();
            }
        }

        public override string Name
        {
            get
            {
                return this.ToString();
            }
        }

    }

    public enum CustomerType
    {
        [Display(Name = "Физическое лицо")]
        Person,
        [Display(Name = "Индивидуальный предприниматель")]
        Individual,
        [Display(Name = "Юридическое лицо")]
        Corporate

    }
}
