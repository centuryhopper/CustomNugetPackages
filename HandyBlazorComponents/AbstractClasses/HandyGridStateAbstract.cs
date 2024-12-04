using System.Security.Cryptography.X509Certificates;
using HandyBlazorComponents.Abstracts;
using Microsoft.AspNetCore.Components;
using static HandyBlazorComponents.Models.ServiceResponses;

namespace HandyBlazorComponents.Abstracts;

public abstract class HandyGridStateAbstract<T, U> where T : HandyGridEntityAbstract<U> where U : class, new()
{
    public List<T> Items;
    public List<string> Columns => typeof(U).GetProperties().Select(prop => prop.Name).ToList();
    public abstract GridValidationResponse ValidationChecks(T item, List<string> columns);
    public virtual void SetEditModeFragments(Dictionary<string, RenderFragment<T>>? EditModeFragments)
    {
        this.EditModeFragments = EditModeFragments;
    }
    public virtual void SetViewModeFragments(Dictionary<string, RenderFragment<T>>? ViewModeFragments)
    {
        this.ViewModeFragments = ViewModeFragments;
    }

    public Dictionary<string, RenderFragment<T>>? EditModeFragments;
    public Dictionary<string, RenderFragment<T>>? ViewModeFragments;
    public IReadOnlyCollection<string> ReadonlyColumns;
    public string ExampleFileUploadUrl;
    public EventCallback<IEnumerable<T>> SubmitFileAction => EventCallback.Factory.Create<IEnumerable<T>>(this, OnSubmitFile);
    public Func<IEnumerable<T>, Task> OnSubmitFile;

    public HandyGridStateAbstract(List<T> Items, List<string> ReadonlyColumns, string ExampleFileUploadUrl, Func<IEnumerable<T>, Task> OnSubmitFile)
    {
        this.ReadonlyColumns = ReadonlyColumns;
        this.ExampleFileUploadUrl = ExampleFileUploadUrl;
        this.OnSubmitFile = OnSubmitFile;
        this.Items = Items;
    }
}
