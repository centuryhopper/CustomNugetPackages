
namespace HandyBlazorComponents.Abstracts;

public abstract class HandyGridEntityAbstract<T> where T : class, new()
{
    public T Object { get; set; }
    public List<string> Columns { get; set; }

    protected HandyGridEntityAbstract()
    {
        Object = new();
        Columns = typeof(T).GetProperties().Select(prop => prop.Name).ToList();
    }
    protected HandyGridEntityAbstract(T Object)
    {
        this.Object = Object;
        Columns = typeof(T).GetProperties().Select(prop => prop.Name).ToList();
    }

    public T DeepCopy()
    {
        T copy = new();
        foreach (var columnName in Columns)
        {
            var originalValue = GetPropertyValue(columnName);
            SetPropertyValue(copy, columnName, originalValue);
        }

        return copy;
    }

    public object? GetPropertyValue(string propertyName)
    {
        object? value = typeof(T).GetProperty(propertyName)?.GetValue(Object);
        return value;
    }

    public void SetPropertyValue(T item, string propertyName, object? value)
    {
        var property = typeof(T).GetProperty(propertyName);
        if (property != null && property.CanWrite)
        {
            var propertyType = property.PropertyType;

            // Null handling for nullable types
            if (value == null)
            {
                if (Nullable.GetUnderlyingType(propertyType) != null)
                {
                    property.SetValue(item, null);
                }
                return;
            }

            try
            {
                // Special handling for DateTime and Nullable<DateTime>
                if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
                {
                    if (value is string stringValue && DateTime.TryParse(stringValue, out DateTime parsedDate))
                    {
                        property.SetValue(item, parsedDate);
                    }
                    else if (value is DateTime dateTimeValue)
                    {
                        property.SetValue(item, dateTimeValue);
                    }
                    else if (propertyType == typeof(DateTime?))
                    {
                        property.SetValue(item, null); // Set nullable DateTime to null if invalid
                    }
                }
                // Special handling for DateOnly and Nullable<DateOnly>
                else if (propertyType == typeof(DateOnly) || propertyType == typeof(DateOnly?))
                {
                    if (value is string stringValue && DateOnly.TryParse(stringValue, out DateOnly parsedDateOnly))
                    {
                        property.SetValue(item, parsedDateOnly);
                    }
                    else if (value is DateOnly dateOnlyValue)
                    {
                        property.SetValue(item, dateOnlyValue);
                    }
                    else if (value is DateTime dateTimeValue)
                    {
                        property.SetValue(item, DateOnly.FromDateTime(dateTimeValue)); // Convert DateTime to DateOnly
                    }
                    else if (propertyType == typeof(DateOnly?))
                    {
                        property.SetValue(item, null); // Set nullable DateOnly to null if invalid
                    }
                }
                // Handle integer types
                else if (propertyType == typeof(int) || propertyType == typeof(int?))
                {
                    property.SetValue(item, Convert.ToInt32(value));
                }
                // Handle float types
                else if (propertyType == typeof(float) || propertyType == typeof(float?))
                {
                    property.SetValue(item, Convert.ToSingle(value));
                }
                // Handle double types
                else if (propertyType == typeof(double) || propertyType == typeof(double?))
                {
                    property.SetValue(item, Convert.ToDouble(value));
                }
                // Handle decimal types
                else if (propertyType == typeof(decimal) || propertyType == typeof(decimal?))
                {
                    property.SetValue(item, Convert.ToDecimal(value));
                }
                // Handle boolean types
                else if (propertyType == typeof(bool) || propertyType == typeof(bool?))
                {
                    property.SetValue(item, Convert.ToBoolean(value));
                }
                // Handle enums
                else if (propertyType.IsEnum)
                {
                    if (value is string enumString && Enum.TryParse(propertyType, enumString, true, out var enumValue))
                    {
                        property.SetValue(item, enumValue);
                    }
                    else if (value.GetType().IsEnum && Enum.IsDefined(propertyType, value))
                    {
                        property.SetValue(item, value);
                    }
                }
                // Handle nullable enums
                else if (Nullable.GetUnderlyingType(propertyType)?.IsEnum == true)
                {
                    var underlyingType = Nullable.GetUnderlyingType(propertyType);
                    if (value is string nullableEnumString && Enum.TryParse(underlyingType, nullableEnumString, true, out var
                    nullableEnumValue))
                    {
                        property.SetValue(item, nullableEnumValue);
                    }
                }
                // Handle List<string> specifically
                else if (propertyType == typeof(List<string>))
                {
                    if (value is string str)
                    {
                        // Convert comma-separated string to List<string>
                        var list = str.Split(',').Select(s => s.Trim()).Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                        property.SetValue(item, list);
                    }
                    else if (value is IEnumerable<string> stringEnumerable)
                    {
                        // If value is already an IEnumerable<string>, convert to List<string>
                        property.SetValue(item, stringEnumerable.ToList());
                    }
                }
                // Handle other IEnumerable<T> types
                else if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    var elementType = propertyType.GetGenericArguments()[0];
                    var method = typeof(Enumerable).GetMethod("Cast")?.MakeGenericMethod(elementType);
                    var toListMethod = typeof(Enumerable).GetMethod("ToList")?.MakeGenericMethod(elementType);

                    if (method != null && toListMethod != null && value is IEnumerable<object> enumerable)
                    {
                        var castedEnumerable = method.Invoke(null, new[] { enumerable });
                        var list = toListMethod.Invoke(null, new[] { castedEnumerable });
                        property.SetValue(item, list);
                    }
                }
                // Handle other directly convertible types
                else
                {
                    property.SetValue(item, Convert.ChangeType(value, propertyType));
                }
            }
            catch (Exception ex)
            {
                // Handle or log exception if needed
                Console.WriteLine($"Failed to set property {propertyName}: {ex.Message}");
            }
        }
    }

    public void SetPropertyValue(string propertyName, object? value)
    {
        var property = typeof(T).GetProperty(propertyName);
        if (property != null && property.CanWrite)
        {
            var propertyType = property.PropertyType;

            // Null handling for nullable types
            if (value == null)
            {
                if (Nullable.GetUnderlyingType(propertyType) != null)
                {
                    property.SetValue(Object, null);
                }
                return;
            }

            try
            {
                // Special handling for DateTime and Nullable<DateTime>
                if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
                {
                    if (value is string stringValue && DateTime.TryParse(stringValue, out DateTime parsedDate))
                    {
                        property.SetValue(Object, parsedDate);
                    }
                    else if (value is DateTime dateTimeValue)
                    {
                        property.SetValue(Object, dateTimeValue);
                    }
                    else if (propertyType == typeof(DateTime?))
                    {
                        property.SetValue(Object, null); // Set nullable DateTime to null if invalid
                    }
                }
                // Special handling for DateOnly and Nullable<DateOnly>
                else if (propertyType == typeof(DateOnly) || propertyType == typeof(DateOnly?))
                {
                    if (value is string stringValue && DateOnly.TryParse(stringValue, out DateOnly parsedDateOnly))
                    {
                        property.SetValue(Object, parsedDateOnly);
                    }
                    else if (value is DateOnly dateOnlyValue)
                    {
                        property.SetValue(Object, dateOnlyValue);
                    }
                    else if (value is DateTime dateTimeValue)
                    {
                        property.SetValue(Object, DateOnly.FromDateTime(dateTimeValue)); // Convert DateTime to DateOnly
                    }
                    else if (propertyType == typeof(DateOnly?))
                    {
                        property.SetValue(Object, null); // Set nullable DateOnly to null if invalid
                    }
                }
                // Handle integer types
                else if (propertyType == typeof(int) || propertyType == typeof(int?))
                {
                    property.SetValue(Object, Convert.ToInt32(value));
                }
                // Handle float types
                else if (propertyType == typeof(float) || propertyType == typeof(float?))
                {
                    property.SetValue(Object, Convert.ToSingle(value));
                }
                // Handle double types
                else if (propertyType == typeof(double) || propertyType == typeof(double?))
                {
                    property.SetValue(Object, Convert.ToDouble(value));
                }
                // Handle decimal types
                else if (propertyType == typeof(decimal) || propertyType == typeof(decimal?))
                {
                    property.SetValue(Object, Convert.ToDecimal(value));
                }
                // Handle boolean types
                else if (propertyType == typeof(bool) || propertyType == typeof(bool?))
                {
                    property.SetValue(Object, Convert.ToBoolean(value));
                }
                // Handle enums
                else if (propertyType.IsEnum)
                {
                    if (value is string enumString && Enum.TryParse(propertyType, enumString, true, out var enumValue))
                    {
                        property.SetValue(Object, enumValue);
                    }
                    else if (value.GetType().IsEnum && Enum.IsDefined(propertyType, value))
                    {
                        property.SetValue(Object, value);
                    }
                }
                // Handle nullable enums
                else if (Nullable.GetUnderlyingType(propertyType)?.IsEnum == true)
                {
                    var underlyingType = Nullable.GetUnderlyingType(propertyType);
                    if (value is string nullableEnumString && Enum.TryParse(underlyingType, nullableEnumString, true, out var
                    nullableEnumValue))
                    {
                        property.SetValue(Object, nullableEnumValue);
                    }
                }
                // Handle List<string> specifically
                else if (propertyType == typeof(List<string>))
                {
                    if (value is string str)
                    {
                        // Convert comma-separated string to List<string>
                        var list = str.Split(',').Select(s => s.Trim()).Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                        property.SetValue(Object, list);
                    }
                    else if (value is IEnumerable<string> stringEnumerable)
                    {
                        // If value is already an IEnumerable<string>, convert to List<string>
                        property.SetValue(Object, stringEnumerable.ToList());
                    }
                }
                // Handle other IEnumerable<T> types
                else if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    var elementType = propertyType.GetGenericArguments()[0];
                    var method = typeof(Enumerable).GetMethod("Cast")?.MakeGenericMethod(elementType);
                    var toListMethod = typeof(Enumerable).GetMethod("ToList")?.MakeGenericMethod(elementType);

                    if (method != null && toListMethod != null && value is IEnumerable<object> enumerable)
                    {
                        var castedEnumerable = method.Invoke(null, new[] { enumerable });
                        var list = toListMethod.Invoke(null, new[] { castedEnumerable });
                        property.SetValue(Object, list);
                    }
                }
                // Handle other directly convertible types
                else
                {
                    property.SetValue(Object, Convert.ChangeType(value, propertyType));
                }
            }
            catch (Exception ex)
            {
                // Handle or log exception if needed
                Console.WriteLine($"Failed to set property {propertyName}: {ex.Message}");
            }
        }
    }

    public abstract int GetPrimaryKey();
    public abstract void SetPrimaryKey(int id);
    // method to set properties dynamically
    public virtual void SetProperties(Dictionary<string, object> properties)
    {
        foreach (var property in properties)
        {
            // Use reflection to set the property if it exists and is writable
            var propInfo = Object.GetType().GetProperty(property.Key);
            if (propInfo != null && propInfo.CanWrite)
            {
                propInfo.SetValue(Object, property.Value);
            }
        }
    }

    public virtual void ParsePropertiesFromCSV(Dictionary<string, object> properties)
    {
        foreach (var property in properties)
        {
            // Use reflection to set the property if it exists and is writable
            var propInfo = Object.GetType().GetProperty(property.Key);
            if (propInfo != null && propInfo.CanWrite)
            {
                var propType = propInfo.PropertyType;

                // Check if the property is a List<string> and the value is a delimited string
                if (typeof(IEnumerable<string>).IsAssignableFrom(propType))
                {
                    var stringValue = ((IEnumerable<string>) property.Value).First();
                    // Convert the delimited string to a List<string>
                    var listValue = stringValue.Split('+').ToList();
                    propInfo.SetValue(Object, listValue);
                }
                else
                {
                    // Set the property directly if it's not a List<string> or delimited string
                    propInfo.SetValue(Object, property.Value);
                }
            }
        }
    }
    public abstract object? DisplayPropertyInGrid(string propertyName);
}
