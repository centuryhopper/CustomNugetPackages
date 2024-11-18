
namespace Client.Services;

using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using Client.Models;
using HandyBlazorComponents.Interfaces;
using Microsoft.AspNetCore.Components;
using static HandyBlazorComponents.Models.ServiceResponses;

public class GridStateService : IHandyGridState<HandyGridEntity, TestClass>
{
    public GridValidationResponse ValidationChecks(HandyGridEntity item, List<string> columns)
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
    public List<HandyGridEntity> Items { get; set; }
    public Dictionary<string, RenderFragment<HandyGridEntity>> EditModeFragments { get; set; }
    public Dictionary<string, RenderFragment<HandyGridEntity>> ViewModeFragments { get; set; }
    public IReadOnlyCollection<string> ReadonlyColumns { get; set; } = [nameof(TestClass.Id)];
    public string ExampleFileUploadUrl { get; set; } = "templates/example.csv";
    public Func<IEnumerable<HandyGridEntity>, Task> OnSubmitFile {get;set;}


}
