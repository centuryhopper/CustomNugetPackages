



public class TestClass
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Descriptions { get; set; } = new();

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