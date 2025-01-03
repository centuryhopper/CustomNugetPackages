# Handy Blazor Components Library

A collection of reusable Blazor components designed to simplify the development process and enhance productivity in Blazor applications.

## Features

- **Easy-to-Use Components**: Ready-to-use Blazor components with customizable options.
- **Responsive Design**: Components are optimized for responsive layouts.
- **Lightweight and Fast**: Designed with performance in mind, minimal dependencies.
- **Theming Support**: Easily theme components to match your application’s design.
- **Extensible**: Built to be extensible, allowing for custom modifications and enhancements.
- Clean: Built with minimal dependencies from other third party libraries.

## Components

Here’s a list of the components included in this library:

- **ConfirmModal**:
  - This is a pop up that gives you a chance to either confirm the choice you just make or change your mind on it.
  - Example:
    - <ConfirmModal @ref="confirmModal" Title="Warning"
      BodyText="Are you sure you want to delete this record? THIS ACTION IS IRREVERSIBLE!" />
      - Public methods:
        - Task<bool> ShowAsync(), returns a boolean of true if the user confirms or false if the user cancels
      - Parameters:
        - Title: String
        - Warning: String
- **NotificationModal**:
  - This is a pop up that allows you to customize information to display to the end user.
  - private NotificationModal notificationModal = default!;
  - <NotificationModal @ref="notificationModal" />
    - Public methods:
      - Task ShowAsync(Title, Message, ModalType) awaits user confirmation to close the popup
      - Parameters:
        - Title: String
        - Message: String
        - ModalType: ModalType
      - ModalType enum code:
        - public enum ModalType
          {
          ERROR,
          WARNING,
          SUCCESS,
          INFO,
          }
- **CooldownTimer**:
  - This is a tool that you can use to time out users (or hackers) who repeatedly try to access a secure resource.
  - <CooldownTimer @ref="cooldownTimer" CooldownTime="10" MaxAttempts="5" OnCooldownComplete="()=>StateHasChanged()" />
    - Public methods:
      - void IncrementSubmissionCount(), increments the count before the button is disabled
    - Public getter:
      - IsCoolingDown, returns true if the CooldownTimer component is active
    - Parameters: - CooldownTime: int - MaxAttempts: int - OnCooldownComplete: EventCallback (use this to call a function if needed once the cooldown completes)
- **DynamicHandyGrid**:
  - This is a tool that you can use to display your collection of items in a grid view and perform crud operations on it.
  - <DynamicHandyGrid @ref="dynamicHandyGrid" TItem="SomeClassThatYouCreate" Items="TestLst" OnCreate="OnCreate" OnUpdate="OnUpdate" OnDelete="OnDelete" ColumnItems="ColumnItems"/>
    - Public methods:
      - void OnUpdate(TItem), called when user updates a row of data in the grid
      - void OnDelete(TItem), called when user deletes a row of data in the grid
      - void OnCreate(List<TItem>), called when user creates one or more rows of data in the grid
    - Parameters:
      - Items: List<TItem> (List of TItems that is derived from some datasource of yours)
      - EditModeFragments: Dictionary<string, RenderFragment<TestClass>> or Dictionary<string, RenderFragment> (each key value pair would have the column name as the key and html code as the value)
        - example:
          - given the class:
            - private class TestClass
              {
              public int Id {get;set;}
              public string Title {get;set;} = string.Empty;
              public string Description {get;set;} = string.Empty;
              }
          - you can create something like this:
            - private Dictionary<string, RenderFragment<TestClass>> ColumnItems => new()
              {
              {
              "Id",
              o => @<input type="number" class="form-control" style="width: 10rem;" onkeypress="return (event.charCode !=8 && event.charCode ==0 || (event.charCode >= 48 && event.charCode <= 57))" @bind="o.Id" placeholder="Enter Id">
              },
              {
              "Title",
              o => @<input class="form-control" @bind="o.Title" placeholder="Enter Title" />
              },
              {
              "Description",
              o => @<input class="form-control" @bind="o.Description" placeholder="Enter Description" />
              },
              };
      - RenderModeFragments: Dictionary<string, RenderFragment<TestClass>> or Dictionary<string, RenderFragment> (each key value pair would have the column name as the key and html code as the value)
        - example: see above

You are responsible for passing in the correct input type when editing, otherwise an error may occur

- **MultiSelectCheckboxList**:
  - This is a dropdown list consisting of a colllection of strings that you can select multiple
  - <MultiSelectCheckBoxList
      TItem="SomeClassThatYouCreate"
      Items="ListOfStrings"
      SelectedItems="SelectedListOfStrings"
      Placeholder="Select Values" />
  - Parameters:
    - Items: List<string> (List of strings that is derived from some datasource of yours)
    - SelectedItems: List<string> (Selected list of strings from the dropdown)
    - Placeholder: String

More will be added as per request, so feel free to create an issue.
Contributions are welcome as well! Just create a pull request and I will take a look.

## Installation

Install via NuGet Package Manager:

- dotnet add package HandyBlazorComponents

Configuration:

- Append these lines to your \_Imports.razor:
  `@using HandyBlazorComponents.Components
  @using HandyBlazorComponents.Utils
  @using ModalType = HandyBlazorComponents.Utils.ModalType`

- Add this to your index.html:
  `<script src="_content/HandyBlazorComponents/handyBlazorComponents.js"></script>`

This package is still actively under development and changes are constantly made so use at your own risk.

dotnet pack && dotnet nuget push bin/Release/HandyBlazorComponents.x.x.xx.nupkg -s nuget.org -k [your_nuget_api_key]
