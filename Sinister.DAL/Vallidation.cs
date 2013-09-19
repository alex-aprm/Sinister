using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sinister.DAL
{
    public class ValidationError
    {
        public ValidationError(string Entity, string Field, string Error)
            : this(Field, Error)
        {
            this.Entity = Entity;
        }
        public ValidationError(string Field, string Error)
        {
            this.Field = Field;
            this.Error = Error;
        }
        public string Entity { get; set; }
        public string Field { get; set; }
        public string Error { get; set; }
    }

    public class ValidationException : Exception
    {
        public ValidationException(Exception innerException)
            : this(null, innerException)
        {
            FieldErrors = new List<ValidationError>();
        }
        public ValidationException(List<ValidationError> FieldErrors)
            : this(FieldErrors, null)
        {
        }
        public ValidationException(List<ValidationError> FieldErrors, Exception innerException)
            : base("Ошибка валидации данных", innerException)
        {
            this.FieldErrors = FieldErrors;
        }
        public List<ValidationError> FieldErrors { get; set; }

    }
}
