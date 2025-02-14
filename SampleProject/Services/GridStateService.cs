
namespace Client.Services;

using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Client.Models;
using HandyBlazorComponents.Abstracts;
using HandyBlazorComponents.Models;
using Microsoft.AspNetCore.Components;
using static HandyBlazorComponents.Models.ServiceResponses;

public class GridStateService : HandyGridStateAbstract<HandyGridEntity, TestClass>
{
    public GridStateService(List<HandyGridEntity> Items, int PageSize = 5, bool CanAddNewItems = true, string? ExampleFileUploadUrl = null, string AddNewItemsText = "Add New Items", bool Exportable = false, bool IsReadonly = true, bool ShowRowIndex = true, bool ShowFilters = true, Func<IEnumerable<HandyGridEntity>, Task>? OnCreate = null, Func<HandyGridEntity, Task>? OnUpdate = null, Func<HandyGridEntity, Task>? OnDelete = null, Func<IEnumerable<HandyGridEntity>, Task>? OnSubmitFile = null, List<string>? ColumnsToHide = null, List<string>? ReadonlyColumns = null, List<NamedRenderFragment<HandyGridEntity>>? ViewModeFragments = null, List<NamedRenderFragment<HandyGridEntity>>? EditModeFragments = null) : base(Items, PageSize, CanAddNewItems, ExampleFileUploadUrl, AddNewItemsText, Exportable, IsReadonly, ShowRowIndex, ShowFilters, OnCreate, OnUpdate, OnDelete, OnSubmitFile, ColumnsToHide, ReadonlyColumns, ViewModeFragments, EditModeFragments)
    {
    }

    public override GridValidationResponse ValidationChecks(HandyGridEntity item)
    {
        // clear any previous messages
        foreach (var key in ErrorMessagesDict.Keys)
        {
            ErrorMessagesDict[key].Clear();
        }

        if (string.IsNullOrWhiteSpace(item.Object.Title))
        {
            ErrorMessagesDict[nameof(item.Object.Title)].Add($"Please fill out {nameof(item.Object.Title)}");
        }

        if (string.IsNullOrWhiteSpace(item.Object.Description))
        {
            ErrorMessagesDict[nameof(item.Object.Description)].Add($"Please fill out {nameof(item.Object.Description)}");
        }

        System.Console.WriteLine(item.Object.Descriptions.Count());
        System.Console.WriteLine(item.Object.Descriptions.Any());

        if (!item.Object.Descriptions.Any())
        {
            ErrorMessagesDict[nameof(item.Object.Descriptions)].Add($"Please make at least one selection for {nameof(item.Object.Descriptions)}");
        }

        if (!string.IsNullOrWhiteSpace(item.Object.Title) && item.Object.Title?.Length > 256)
        {
            ErrorMessagesDict[nameof(item.Object.Title)].Add("Please make sure all fields are under 256 characters");
        }

        if (!string.IsNullOrWhiteSpace(item.Object.Description) && item.Object.Description?.Length > 256)
        {
            ErrorMessagesDict[nameof(item.Object.Description)].Add("Please make sure all fields are under 256 characters");
        }
        
        if (ErrorMessagesDict.Any())
        {
            return new GridValidationResponse(Flag: false, ErrorMessagesDict);
        }

        return new GridValidationResponse(Flag: true, null);
    }


}

