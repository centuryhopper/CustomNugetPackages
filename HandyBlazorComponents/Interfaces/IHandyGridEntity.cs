
namespace HandyBlazorComponents.Interfaces;

public interface IHandyGridEntity
{
    int GetPrimaryKey();
    void SetPrimaryKey(int id);
    // method to set properties dynamically
    void SetProperties(Dictionary<string, object> properties);
    void ParsePropertiesFromCSV(Dictionary<string, object> properties);
    object? DisplayPropertyInGrid(string propertyName);

}
