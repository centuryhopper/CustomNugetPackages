

using HandyBlazorComponents.Interfaces;

public class TestClass : IHandyGridEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Descriptions { get; set; } = new();

    public object? DisplayPropertyInGrid(string propertyName)
    {
        switch (propertyName)
        {
            case nameof(Id):
                return Id;
            case nameof(Title):
                return Title;
            case nameof(Description):
                return Description;
            case nameof(Descriptions):
                return string.Join(",", Descriptions);
            default:
                throw new Exception("Invalid property name");
        }
    }

    public int GetPrimaryKey()
    {
        return Id;
    }

    public void SetPrimaryKey(int id)
    {
        Id = id;
    }

    public void ParsePropertiesFromCSV(Dictionary<string, object> properties)
    {
        foreach (var property in properties)
        {
            // Use reflection to set the property if it exists and is writable
            var propInfo = this.GetType().GetProperty(property.Key);
            if (propInfo != null && propInfo.CanWrite)
            {
                var propType = propInfo.PropertyType;

                // Check if the property is a List<string> and the value is a delimited string
                if (typeof(IEnumerable<string>).IsAssignableFrom(propType))
                {
                    var stringValue = ((IEnumerable<string>) property.Value).First();
                    // Convert the delimited string to a List<string>
                    var listValue = stringValue.Split('+').ToList();
                    propInfo.SetValue(this, listValue);
                }
                else
                {
                    // Set the property directly if it's not a List<string> or delimited string
                    propInfo.SetValue(this, property.Value);
                }
            }
        }
    }

    public void SetProperties(Dictionary<string, object> properties)
    {
        foreach (var property in properties)
        {
            // Use reflection to set the property if it exists and is writable
            var propInfo = this.GetType().GetProperty(property.Key);
            if (propInfo != null && propInfo.CanWrite)
            {
                propInfo.SetValue(this, property.Value);
            }
        }
    }

    public override string ToString()
    {
        return $"{Id}, {Title}, {Description}";
    }
}

public class TestClassMapper : ClassMap<TestClass>
{
    public TestClassMapper()
    {
        Map(m => m.Id).Name("Id");
        Map(m => m.Title).Name("Title");
        Map(m => m.Description).Name("Description");
        Map(m => m.Descriptions).Name("Descriptions");
    }
}