
using HandyBlazorComponents.Abstracts;


namespace Client.Models;

public class HandyGridEntity : HandyGridEntityAbstract<TestClass>
{

    public HandyGridEntity() : base()
    {
    }

    public HandyGridEntity(TestClass Object) : base(Object)
    {
    }

    public override object? DisplayPropertyInGrid(string propertyName)
    {
        switch (propertyName)
        {
            case nameof(Object.Id):
                return Object.Id;
            case nameof(Object.Title):
                return Object.Title;
            case nameof(Object.Description):
                return Object.Description;
            case nameof(Object.Descriptions):
                return string.Join(",", Object.Descriptions);
            case nameof(Object.CheckBoxTest):
                return Object.CheckBoxTest.ToString();
            case nameof(Object.DateTest):
                return Object.DateTest.ToString();
            case nameof(Object.DropdownTest):
                return Object.DropdownTest;
            case nameof(Object.TextAreaTest):
                return Object.TextAreaTest;
            case nameof(Object.ImageUpload):
                return Object.ImageUpload;
            case nameof(Object.PdfUpload):
                return Object.PdfUpload;
            default:
                throw new Exception("Invalid property name");
        }
    }

    public override int GetPrimaryKey()
    {
        return Object.Id;
    }

    public override void SetPrimaryKey(int id)
    {
        Object.Id = id;
    }

    public override void ParsePropertiesFromCSV(Dictionary<string, object> properties)
    {
        base.ParsePropertiesFromCSV(properties);
    }

    public override void SetProperties(Dictionary<string, object> properties)
    {
        base.SetProperties(properties);
    }
}

