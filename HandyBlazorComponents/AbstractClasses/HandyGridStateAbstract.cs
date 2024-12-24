using HandyBlazorComponents.Abstracts;
using HandyBlazorComponents.Models;
using Microsoft.AspNetCore.Components;
using static HandyBlazorComponents.Models.ServiceResponses;

namespace HandyBlazorComponents.Abstracts;

public abstract class HandyGridStateAbstract<T, U> where T : HandyGridEntityAbstract<U> where U : class, new()
{
    public List<T> Items;
    public List<string> Columns => typeof(U).GetProperties().Select(prop => prop.Name).ToList();
    public abstract GridValidationResponse ValidationChecks(T item, List<string> columns);

    public List<NamedRenderFragment<T>>? EditModeFragments;
    public List<NamedRenderFragment<T>>? ViewModeFragments;
    public List<string> ReadonlyColumns;
    public string ExampleFileUploadUrl;
    public EventCallback<IEnumerable<T>> SubmitFileAction => EventCallback.Factory.Create<IEnumerable<T>>(this, OnSubmitFile);
    public Func<IEnumerable<T>, Task>? OnSubmitFile;
    public bool Exportable;
    public bool IsReadonly;
    public int PageSize;
    public List<string> ColumnsToHide;

    public EventCallback<IEnumerable<T>> OnCreateAction => EventCallback.Factory.Create<IEnumerable<T>>(this, OnCreate);
    public Func<IEnumerable<T>, Task>? OnCreate;

    public EventCallback<T> OnUpdateAction => EventCallback.Factory.Create<T>(this, OnUpdate);
    public Func<T, Task>? OnUpdate;

    public EventCallback<T> OnDeleteAction => EventCallback.Factory.Create<T>(this, OnDelete);
    public Func<T, Task>? OnDelete;

    public HandyGridStateAbstract(List<T> Items,
        int PageSize = 5,
        string? ExampleFileUploadUrl = null,
        bool Exportable = false,
        bool IsReadonly = true,
        Func<IEnumerable<T>, Task>? OnCreate = null,
        Func<T, Task>? OnUpdate = null,
        Func<T, Task>? OnDelete = null,
        Func<IEnumerable<T>, Task>? OnSubmitFile = null,
        List<string>? ColumnsToHide = null,
        List<string>? ReadonlyColumns = null,
        List<NamedRenderFragment<T>>? ViewModeFragments = null,
        List<NamedRenderFragment<T>>? EditModeFragments = null
    )
    {
        this.Exportable = Exportable;
        this.OnCreate = OnCreate;
        this.OnUpdate = OnUpdate;
        this.OnDelete = OnDelete;
        this.PageSize = PageSize;
        this.IsReadonly = IsReadonly;
        this.ColumnsToHide = ColumnsToHide ?? [];
        this.ReadonlyColumns = ReadonlyColumns ?? [];
        this.ExampleFileUploadUrl = ExampleFileUploadUrl ?? string.Empty;
        this.OnSubmitFile = OnSubmitFile;
        this.Items = Items;
        this.EditModeFragments = EditModeFragments;
        this.ViewModeFragments = ViewModeFragments;
    }
}
