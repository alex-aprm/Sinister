using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Xml;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Globalization;

namespace Sinister.Global
{
    public class DecimalModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext,
            ModelBindingContext bindingContext)
        {
            ValueProviderResult valueResult = bindingContext.ValueProvider
                .GetValue(bindingContext.ModelName);
            ModelState modelState = new ModelState { Value = valueResult };
            object actualValue = null;
            try
            {
                //Check if this is a nullable decimal and a null or empty string has been passed
                var isNullableAndNull = (bindingContext.ModelMetadata.IsNullableValueType &&
                                         string.IsNullOrEmpty(valueResult.AttemptedValue));
                //If not nullable and null then we should try and parse the decimal
                if (!isNullableAndNull)
                {
                    actualValue = decimal.Parse(valueResult.AttemptedValue, NumberStyles.Any, CultureInfo.CurrentCulture);
                }
            }
            catch (FormatException e)
            {
                modelState.Errors.Add(e);
            }
            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            return actualValue;
        }
    }

    public class DateTimeBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            ValueProviderResult valueResult = bindingContext.ValueProvider
    .GetValue(bindingContext.ModelName);
            ModelState modelState = new ModelState { Value = valueResult };

            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            object date = null;
            try
            {
                if (value!=null)
                date = value.ConvertTo(typeof(DateTime), CultureInfo.CurrentCulture);
            }
            catch (Exception e)
            {
                modelState.Errors.Add(e);
            }
            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            return date;
        }
    }
   
    public class ClientNumberValidatorProvider : ClientDataTypeModelValidatorProvider
    {
        public override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata,
                                                               ControllerContext context)
        {
            bool isNumericField = base.GetValidators(metadata, context).Any();
            if (isNumericField)
                yield return new ClientSideNumberValidator(metadata, context);
        }
    }

    public class ClientSideNumberValidator : ModelValidator
    {
        public ClientSideNumberValidator(ModelMetadata metadata,
            ControllerContext controllerContext)
            : base(metadata, controllerContext) { }

        public override IEnumerable<ModelValidationResult> Validate(object container)
        {
            yield break; // Do nothing for server-side validation 
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            yield return new ModelClientValidationRule
            {
                ValidationType = "number",
                ErrorMessage = "Некорректное значение"
            };
            yield return new ModelClientValidationRule
            {
                ValidationType = "date",
                ErrorMessage = "Некорректное значение"
            };
        }
    }

    public static class God
    {

        public static string GetConnectionString(string Name)
        {
            string r = "";

            r = ConfigurationManager.ConnectionStrings[Name].ConnectionString;
            return r;
        }
        public static string GetConnectionString(string Name, string User, string Password)
        {
            SqlConnectionStringBuilder b = new SqlConnectionStringBuilder(GetConnectionString(Name));
            if (User != null)
                b.UserID = User;
            if (Password != null)
                b.Password = Password;
            return b.ConnectionString;

        }

        public static string MigrationConnectionString { get; set; }
        public static void GiveMeMaps()
        {

        }


    }

    public static class EnumHelper<T>
    {
        public static IList<T> GetValues(Enum value)
        {
            var enumValues = new List<T>();

            foreach (FieldInfo fi in value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                enumValues.Add((T)Enum.Parse(value.GetType(), fi.Name, false));
            }
            return enumValues;
        }

        public static T Parse(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static IList<string> GetNames(Enum value)
        {
            return value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public).Select(fi => fi.Name).ToList();
        }

        public static IList<string> GetDisplayValues(Enum value)
        {
            return GetNames(value).Select(obj => GetDisplayValue(Parse(obj))).ToList();
        }

        public static string GetDisplayValue(T value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var descriptionAttributes = fieldInfo.GetCustomAttributes(
                typeof(DisplayAttribute), false) as DisplayAttribute[];

            if (descriptionAttributes == null) return string.Empty;
            return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Name : value.ToString();
        }
    }

    public static class XMLHelpers
    {
        public static string ToIndentedString(this XmlDocument doc)
        {
            var stringWriter = new StringWriter(new StringBuilder());
            var xmlTextWriter = new XmlTextWriter(stringWriter) { Formatting = Formatting.Indented };
            doc.Save(xmlTextWriter);
            return stringWriter.ToString();
        }
    }

    public static class Extensions
    {
        public static string GetDisplayName(this PropertyInfo property)
        {
            var atts = property.GetCustomAttributes(
                typeof(DisplayNameAttribute), true);
            if (atts.Length == 0)
                return "";
            return (atts[0] as DisplayNameAttribute).DisplayName;
        }

        public static bool In<T>(this T obj, params T[] args)
        {
            return args.Contains(obj);
        }

        public static string UpperFirst(this string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }

    }
}