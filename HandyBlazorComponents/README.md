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
    - Parameters:
          - CooldownTime: int
          - MaxAttempts: int
          - OnCooldownComplete: EventCallback (use this to call a function if needed once the cooldown completes)
- **DynamicHandyGrid**:
  - This is a tool that you can use to display your collection of items in a grid view and perform crud operations on it.
  - You must wrap this component with a class that inherits from HandyGridStateAbstract
  - <DynamicHandyGrid @ref="dynamicHandyGrid" TItem="SomeClassThatYouCreate" Items="Items" />
- **UploadComponent**:
  - If used, this should only be used with the DynamicHandyGrid component
  - Example:
    - <UploadComponent TItem="TestClass" TMapper="TestClassMapper" TEntity="HandyGridEntity">
  - Parameters:
    - TItem: The class name of your entity
    - TMapper: The class name of your mapper that inherits from the ClassMap class from the 
    CsvHelper nuget package library
    - TEntity: The class name that you created which inherits from the 'HandyGridEntityAbstract' class
    - Items: Your collection of entities that would be listened to for changes in real-time



## Abstract classes

Here are the two abstract classes you would use for the DynamicHandyGrid component
  - HandyGridEntityAbstract
    - You really only have to worry about these public methods:
      - public override object? DisplayPropertyInGrid(string propertyName)
        - Gives you the freedom to display each property of your entity however you'd like
        - Strings and primitive types ought to be displayed directly displayed but more complicated types such as collections can be displayed by, for example, joining the elements into a string separated by a comma
      - public override int GetPrimaryKey()
        - Get the primary key of your entity
      - public override void SetPrimaryKey(int id)
        - Set the primary key of your entity (this is useful for creating new items on the fly)
        - If your entity's primary key is auto generated then that will take precedence over this
      - public override void ParsePropertiesFromCSV(Dictionary<string, object> properties)
        - Set the parent class's Object property to the values that were parsed.
        - Most of the time calling the parent class version of this will suffice
      - public override void SetProperties(Dictionary<string, object> properties)
        - Assign the properties of the Object to the values of the 'properties' parameter
        - Most of the time calling the parent class version of this will suffice

  - HandyGridStateAbstract
    - Public variables
      - public List<T> Items
        - This is your collection of entities you wish to display to the HandyGrid (List of TItems that is derived from some datasource of yours). Without this, there would be no data to display
      - public List<NamedRenderFragment<T>>? EditModeFragments
        - This is your collection of html that you wish to display when in edit mode. Each item in this collection has to be of Type NamedRenderFragment, which is a class containing two properties:
          - public string Name { get; set; } // This should be the name of the column you want the html fragment to render in when editing
          - public RenderFragment<T> Fragment { get; set; } // This is the actual html fragment. It is recommended that you use one of my handy components for anything else besides an input element to avoid weird behavior. Check the Handy* Components section for currently available ones to use for editing. If there's a component for editing that is not in my library that you'd like me to create, feel free to let me know.
        - Example:
          - given the class:
            - private class TestClass
              {
                  public int Id {get;set;}
                  public string Title {get;set;} = string.Empty;
                  public string Description {get;set;} = string.Empty;
              }
          - you can create something like this:
            - EditModeFragments: [
                new NamedRenderFragment<TestClass>(
                    Name: nameof(TestClass.Title),
                    Fragment: o => @<input class="form-control" @bind="o.Title" placeholder="Enter Title" />
                ),
                new NamedRenderFragment<TestClass>(
                    Name: nameof(TestClass.Description),
                    Fragment: o => @<HandyTextAreaInput TItem="TestClass" ColumnName="@(nameof(TestClass.Description))" Item="o.Object" Style="width: 12rem;" />
                ),
              ]
            - *You are responsible for passing in the correct input type when editing, otherwise an error may occur*
      - public List<NamedRenderFragment<T>>? ViewModeFragments
        - Same idea as EditModeFragments property except these fragments are displayed when the grid is in view (non-editing) mode
        - Example:
          - ViewModeFragments: [
                new NamedRenderFragment<TestClass>(
                    Name: nameof(TestClass.Title),
                    Fragment: o => @<a style="cursor: pointer;" @onclick="@(()=>Console.WriteLine("This is the title!"))">@(o.Object.Title)</a>
                ),
            ]
      - public List<string> ReadonlyColumns
        - Add the names of the columns of your entity to this grid you want to remain in view mode even when the edit button is clicked. A good example of this would be to add the primary key (e.g. Id)
        - Column names added to this collection will also be ignored in the grid pop up for adding new items to your collection
      - public string ExampleFileUploadUrl
        - This is the path of the sample csv file for users to use as a guide if you allow users to bulk upload to your grid collection
      - public bool Exportable
        - Set this to true if you'd like to be able to download your grid collection to a csv file (default is false)
      - public bool IsReadonly
        - Set to false if you'd like to be able to edit your grid values (default is true)
      - public int PageSize
        - Set this to the number of rows you'd like to display per page in the grid
      - public List<string> ColumnsToHide
        - Add the names of the columns of your entity to this grid you want to hide completely at all times.

    - Public methods:
      - public Func<IEnumerable<T>, Task>? OnSubmitFile
        - called when user uploads a csv file for the grid to consume (this method will not be used if isExportable is set to false)
      - public Func<IEnumerable<T>, Task>? OnCreate
        - called when user creates one or more rows of data in the grid
      - public Func<T, Task>? OnUpdate
        - called when user updates a row of data in the grid
      - public Func<T, Task>? OnDelete
        - called when user deletes a row of data in the grid


## Handy* Components

- **HandyMultiSelectCheckboxList**:
  - This is a dropdown list consisting of a colllection of strings that you can select multiple 
  - <HandyMultiSelectCheckboxList
      TItem="SomeClassThatYouCreate"
      Items="ListOfStrings"
      SelectedItems="SelectedListOfStrings"
      Placeholder="Select Values" />
  - Parameters:
    - Items: List<string> (List of strings that is derived from some datasource of yours)
    - SelectedItems: List<string> (Selected list of strings from the dropdown)
    - Placeholder: String

- **HandyTextAreaInput**:
  - This is a text area that provides two way binding for any string property in your entity that you may have
  - <HandyTextAreaInput TItem="TestClass" ColumnName="@(nameof(TestClass.Description))" Item="o.Object" Style="width: 12rem;" />
  - Parameters:
      - TItem: The class name of your entity
      - ColumnName: The column name of your entity (property name)
      - Item: The HandyGridEntityAbstract Object you want this component to bind to.
      - Style: Gives you the freedom of styling this component using inline css

- **HandyCheckbox**:
  - This is a checkbox that provides two way binding for any string property in your entity that you may have
  - <HandyCheckbox TItem="TestClass" ColumnName="@(nameof(TestClass.Description))" Item="o.Object" Style="width: 12rem;" />
  - Parameters:
      - TItem: The class name of your entity
      - ColumnName: The column name of your entity (property name)
      - Item: The HandyGridEntityAbstract Object you want this component to bind to.
      - Style: Gives you the freedom of styling this component using inline css


- **HandyDatePicker**:
  - This is a datepicker that provides two way binding for any dateonly property in your entity that you may have
  - <HandyDatePicker TItem="TestClass" ColumnName="@(nameof(TestClass.DateAdded))" Item="o.Object" Style="width: 12rem;" />
  - Parameters:
      - TItem: The class name of your entity
      - ColumnName: The column name of your entity (property name)
      - Item: The HandyGridEntityAbstract Object you want this component to bind to.
      - Style: Gives you the freedom of styling this component using inline css

- **HandyDropdown**:
  - This is a dropdown that provides two way binding for any string property in your entity that you may have
  - <HandyDropdown TItem="TestClass" ColumnName="@(nameof(TestClass.DropdownChoice))" Item="o.Object" Style="width: 12rem;" />
  - Parameters:
      - TItem: The class name of your entity
      - ColumnName: The column name of your entity (property name)
      - Item: The HandyGridEntityAbstract Object you want this component to bind to.
      - Style: Gives you the freedom of styling this component using inline css

- **HandyFileUpload**:
  - This is a dropdown that provides two way binding for any byte[] property in your entity that you may have
  - <HandyDropdown TItem="TestClass" ColumnName="@(nameof(TestClass.File))" Item="o.Object" Style="width: 12rem;" />
  - Parameters:
      - TItem: The class name of your entity
      - ColumnName: The column name of your entity (property name)
      - Item: The HandyGridEntityAbstract Object you want this component to bind to.
      - Style: Gives you the freedom of styling this component using inline css

- **HandyFileUpload**:
  - This is a dropdown that provides two way binding for any byte[] property in your entity that you may have
  - <HandyToast @ref="handyToast" Title="Success" Message="Your operation completed successfully." ToastType="HandyToastType.SUCCESS" Duration="5" />
  - Parameters:
      - ref: a reference to a HandyToast instance so that you can call its public method later
      - Title: The title of this toast popup
      - Message: The message you would like to inform the user from this toast popup
      - Duration: The number of seconds you would like this toast popup to stay visible
      - ToastType: The type of information this toast should identify as
        - public enum HandyToastType
          {
              ERROR, // the toast will be red
              WARNING, // the toast will be yellow
              SUCCESS, // the toast will be green
              INFO, // the toast will be a shade of blue
          }
  Example:
  - Here we show the pop up in a non-blocking and asynchronous way:
    - _ = handyToast.ShowToastAsync("Error", "Something went wrong. Please try again later...", HandyToastType.ERROR);

        

More will be added as per request, so feel free to create an issue.
Contributions are welcome as well! Just create a pull request and I will take a look.

## Installation
Install via NuGet Package Manager:
- dotnet add package HandyBlazorComponents

Configuration:
- Append these lines to your _Imports.razor:
  ``@using HandyBlazorComponents.Components
    @using HandyBlazorComponents.Utils
    @using HandyBlazorComponents.Models
    @using ModalType = HandyBlazorComponents.Utils.ModalType``

- Add this to your index.html:
    ``<script src="_content/HandyBlazorComponents/handyBlazorComponents.js"></script>``

This package is still actively under development and changes are constantly made so use at your own risk.

Inside the HandyBlazorComponents directory:
  dotnet pack && dotnet nuget push bin/Release/HandyBlazorComponents.x.x.x.nupkg -s nuget.org -k [insert_your_api_key_here]
  