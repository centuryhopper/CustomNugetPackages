using HandyBlazorComponents.Abstracts;
using Microsoft.AspNetCore.Components;
using static HandyBlazorComponents.Models.ServiceResponses;

namespace HandyBlazorComponents.Interfaces;

public interface IHandyGridState<T, U> where T : HandyGridEntityAbstract<U> where U : class, new()
{
    List<T> Items { get; set; }
    List<string> Columns => typeof(U).GetProperties().Select(prop => prop.Name).ToList();
    GridValidationResponse ValidationChecks(T item, List<string> columns);
    Dictionary<string, RenderFragment<T>> EditModeFragments { get; set; }
    Dictionary<string, RenderFragment<T>> ViewModeFragments { get; set; }
    IReadOnlyCollection<string> ReadonlyColumns { get; set; }
    string ExampleFileUploadUrl { get; set; }
    EventCallback<IEnumerable<T>> SubmitFileAction => EventCallback.Factory.Create<IEnumerable<T>>(this, OnSubmitFile);
    Func<IEnumerable<T>, Task> OnSubmitFile {get;set;}
}
