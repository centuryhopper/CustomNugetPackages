
public class TestClass
{
    public int Id { get; set; }
    public string? Title { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public List<string>? Descriptions { get; set; } = new();

    public bool? CheckBoxTest {get;set;}
    public DateOnly? DateTest {get;set;}
    public string? DropdownTest { get; set; }
    public string? TextAreaTest { get; set; }
    public byte[]? ImageUpload { get; set; }
    public byte[]? PdfUpload { get; set; }


    public override string ToString()
    {
        return $"{Id}, {Title}, {Description}, {string.Join("|", Descriptions)}, {CheckBoxTest}, {DateTest}, {DropdownTest}, {TextAreaTest}";
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
        Map(m => m.CheckBoxTest).Name("CheckBoxTest");
        Map(m => m.DateTest).Name("DateTest");
        Map(m => m.DropdownTest).Name("DropdownTest");
        Map(m => m.TextAreaTest).Name("TextAreaTest");
    }
}