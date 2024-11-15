using Microsoft.AspNetCore.Components;
using static HandyBlazorComponents.Models.ServiceResponses;

public interface IHandyGridState<T>
{
    List<T> Items { get; set; }
    GridValidationResponse ValidationChecks(T item, List<string> columns);
    Dictionary<string, RenderFragment<T>> EditModeFragments { get; set; }
    Dictionary<string, RenderFragment<T>> ViewModeFragments { get; set; }
    IReadOnlyCollection<string> ReadonlyColumns { get; set; }
    string ExampleFileUploadUrl { get; set; }

    EventCallback<IEnumerable<T>> SubmitFileAction { get =>
        EventCallback.Factory.Create<IEnumerable<T>>(this, OnSubmitFile); }
    Func<IEnumerable<T>, Task> OnSubmitFile {get;set;}
}
