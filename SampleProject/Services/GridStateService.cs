
namespace Client.Services;

using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Client.Models;
using HandyBlazorComponents.Abstracts;
using Microsoft.AspNetCore.Components;
using static HandyBlazorComponents.Models.ServiceResponses;

public class GridStateService : HandyGridStateAbstract<HandyGridEntity, TestClass>
{
    public GridStateService(List<HandyGridEntity> Items, List<string> ReadonlyColumns, string ExampleFileUploadUrl, Func<IEnumerable<HandyGridEntity>, Task> OnSubmitFile, Dictionary<string, RenderFragment<HandyGridEntity>>? ViewModeFragments, Dictionary<string, RenderFragment<HandyGridEntity>>? EditModeFragments) : base(Items, ReadonlyColumns, ExampleFileUploadUrl, OnSubmitFile, ViewModeFragments, EditModeFragments)
    {
        this.Items = Items;
        this.OnSubmitFile = OnSubmitFile;
        this.ReadonlyColumns = ReadonlyColumns;
        this.ExampleFileUploadUrl = ExampleFileUploadUrl;
        this.ViewModeFragments = ViewModeFragments;
        this.EditModeFragments = EditModeFragments;
    }

    public override GridValidationResponse ValidationChecks(HandyGridEntity item, List<string> columns)
    {
        Dictionary<int, List<string>> errorMessagesDict = new();

        int titleIndex = columns.IndexOf(nameof(item.Object.Title));
        int descriptionIndex = columns.IndexOf(nameof(item.Object.Description));
        int descriptionsIndex = columns.IndexOf(nameof(item.Object.Descriptions));

        // Console.WriteLine($"{titleIndex},{userNameIndex},{passwordIndex}");

        if (string.IsNullOrWhiteSpace(item.Object.Title))
        {
            if (errorMessagesDict.ContainsKey(titleIndex))
            {
                errorMessagesDict[titleIndex].Add($"Please fill out {nameof(item.Object.Title)}");
            }
            else
            {
                errorMessagesDict.Add(titleIndex, [$"Please fill out {nameof(item.Object.Title)}"]);
            }
        }
        if (string.IsNullOrWhiteSpace(item.Object.Description))
        {
            if (errorMessagesDict.ContainsKey(descriptionIndex))
            {
                errorMessagesDict[descriptionIndex].Add($"Please fill out {nameof(item.Object.Description)}");
            }
            else
            {
                errorMessagesDict.Add(descriptionIndex, [$"Please fill out {nameof(item.Object.Description)}"]);
            }
        }

        if (!item.Object.Descriptions.Any())
        {
            if (errorMessagesDict.ContainsKey(descriptionsIndex))
            {
                errorMessagesDict[descriptionsIndex].Add($"Please make at least one selection for {nameof(item.Object.Descriptions)}");
            }
            else
            {
                errorMessagesDict.Add(descriptionsIndex, [$"Please make at least one selection for {nameof(item.Object.Descriptions)}"]);
            }
        }

        if (!string.IsNullOrWhiteSpace(item.Object.Title) && item.Object.Title?.Length > 256)
        {
            if (errorMessagesDict.ContainsKey(titleIndex))
            {
                errorMessagesDict[titleIndex].Add("Please make sure all fields are under 256 characters");
            }
            else
            {
                errorMessagesDict.Add(titleIndex, ["Please make sure all fields are under 256 characters"]);
            }
        }

        if (!string.IsNullOrWhiteSpace(item.Object.Description) && item.Object.Description?.Length > 256)
        {
            if (errorMessagesDict.ContainsKey(descriptionIndex))
            {
                errorMessagesDict[descriptionIndex].Add("Please make sure all fields are under 256 characters");
            }
            else
            {
                errorMessagesDict.Add(descriptionIndex, ["Please make sure all fields are under 256 characters"]);
            }
        }
        
        // Console.WriteLine($"{titleIndex},{userNameIndex},{passwordIndex}");

        if (errorMessagesDict.Any())
        {
            return new GridValidationResponse(Flag: false, errorMessagesDict);
        }

        return new GridValidationResponse(Flag: true, null);
    }
}

