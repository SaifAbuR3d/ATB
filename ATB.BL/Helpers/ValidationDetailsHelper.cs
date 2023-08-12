using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ATB.Helpers
{
    internal static class ValidationDetailsHelper
    {
        internal static void DisplayValidationDetails(Type EntityType)
        {
            var properties = EntityType.GetProperties();

            foreach (var property in properties)
            {
                Console.WriteLine($"- {property.Name}:");

                var attributes = property.GetCustomAttributes();

                foreach (var attribute in attributes)
                {
                    if (attribute is ValidationAttribute validationAttribute)
                    {
                        if (validationAttribute is DataTypeAttribute dataTypeAttribute)
                        {
                            Console.WriteLine($"    - {dataTypeAttribute.DataType.ToString()}");
                        }
                        else if (validationAttribute is RequiredAttribute)
                        {
                            Console.WriteLine($"    - Required");
                        }
                        else if (validationAttribute is RangeAttribute rangeAttribute)
                        {
                            Console.WriteLine($"    - Allowed Range: {rangeAttribute.Minimum} → {rangeAttribute.Maximum}");
                        }
                        else if (validationAttribute is StringLengthAttribute stringLengthAttribute)
                        {
                            Console.WriteLine($"    - Maximum Length: {stringLengthAttribute.MaximumLength}");
                        }
                    }
                }
            }
        }
    }
}