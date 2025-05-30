using HandyBlazorComponents.Abstracts;
using HandyBlazorComponents.Models;
using Microsoft.AspNetCore.Components;
using static HandyBlazorComponents.Models.HandyServiceResponses;

namespace HandyBlazorComponents.Abstracts;

public abstract class HandyGridStateAbstract<T, U> where T : HandyGridEntityAbstract<U> where U : class, new()
{
    public List<T> Items;
    public List<string> Columns => typeof(U).GetProperties().Select(prop => prop.Name).ToList();
    public virtual HandyGridValidationResponse ValidationChecks(T item)
    {
        foreach (var key in ErrorMessagesDict.Keys)
        {
            ErrorMessagesDict[key].Clear();
        }
        return default!;
    }

    public List<HandyNamedRenderFragment<T>>? EditModeFragments;
    public List<HandyNamedRenderFragment<T>>? ViewModeFragments;
    public List<string> ReadonlyColumns;
    public string ExampleFileUploadUrl;
    public bool Exportable;
    public bool IsReadonly;
    public bool ShowRowIndex;
    public bool ShowFilters;
    public int PageSize;
    public List<string> ColumnsToHide;

    public EventCallback<IEnumerable<T>> SubmitFileAction => EventCallback.Factory.Create<IEnumerable<T>>(this, OnSubmitFile);
    public Func<IEnumerable<T>, Task>? OnSubmitFile;
    public EventCallback<IEnumerable<T>> OnCreateAction => EventCallback.Factory.Create<IEnumerable<T>>(this, OnCreate);
    public Func<IEnumerable<T>, Task>? OnCreate;

    public EventCallback<T> OnUpdateAction => EventCallback.Factory.Create<T>(this, OnUpdate);
    public Func<T, Task>? OnUpdate;

    public EventCallback<T> OnDeleteAction => EventCallback.Factory.Create<T>(this, OnDelete);
    public Func<T, Task>? OnDelete;
    public Dictionary<string, List<string>> ErrorMessagesDict = new();
    public string AddNewItemsText;
    public bool CanAddNewItems;

    public HandyGridStateAbstract(List<T> Items,
        int PageSize = 5,
        bool CanAddNewItems = true,
        string? ExampleFileUploadUrl = null,
        string AddNewItemsText = "Add New Items",
        bool Exportable = false,
        bool IsReadonly = true,
        bool ShowRowIndex = true,
        bool ShowFilters = true,
        Func<IEnumerable<T>, Task>? OnCreate = null,
        Func<T, Task>? OnUpdate = null,
        Func<T, Task>? OnDelete = null,
        Func<IEnumerable<T>, Task>? OnSubmitFile = null,
        List<string>? ColumnsToHide = null,
        List<string>? ReadonlyColumns = null,
        List<HandyNamedRenderFragment<T>>? ViewModeFragments = null,
        List<HandyNamedRenderFragment<T>>? EditModeFragments = null
    )
    {
        this.CanAddNewItems = CanAddNewItems;
        this.Exportable = Exportable;
        this.OnCreate = OnCreate;
        this.OnUpdate = OnUpdate;
        this.OnDelete = OnDelete;
        this.PageSize = PageSize;
        this.AddNewItemsText = AddNewItemsText;
        this.IsReadonly = IsReadonly;
        this.ShowRowIndex = ShowRowIndex;
        this.ShowFilters = ShowFilters;
        this.ColumnsToHide = ColumnsToHide ?? [];
        this.ReadonlyColumns = ReadonlyColumns ?? [];
        this.ExampleFileUploadUrl = ExampleFileUploadUrl ?? string.Empty;
        this.OnSubmitFile = OnSubmitFile;
        this.Items = Items;
        this.EditModeFragments = EditModeFragments;
        this.ViewModeFragments = ViewModeFragments;
        foreach (var column in Columns)
        {
            ErrorMessagesDict[column] = [];
        }
    }
}
