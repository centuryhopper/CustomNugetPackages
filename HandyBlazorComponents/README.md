# Handy Blazor Components Library

A collection of reusable Blazor components designed to simplify the development process and enhance productivity in Blazor applications.

## Features

- **Easy-to-Use Components**: Ready-to-use Blazor components with customizable options.
- **Responsive Design**: Components are optimized for responsive layouts.
- **Lightweight and Fast**: Designed with performance in mind, minimal dependencies.
- **Theming Support**: Easily theme components to match your application’s design.
- **Extensible**: Built to be extensible, allowing for custom modifications and enhancements.
- Clean: Built with minimal dependencies from other third party libraries.

enums:
  - HandyModalType enum code:
      public enum HandyModalType
      {
          ERROR,
          WARNING,
          SUCCESS,
          INFO,
      }
  - HandyToastType enum code:
    public enum HandyToastType
    {
        ERROR, // the toast will be red
        WARNING, // the toast will be yellow
        SUCCESS, // the toast will be green
        INFO, // the toast will be a shade of blue
    }



## Components

Here’s a list of the components included in this library:

- **ConfirmModal**:
  - This is a pop up that gives you a chance to either confirm the choice you just make or change your mind on it.
  - Example:
    - <ConfirmModal @ref="confirmModal" Title="Warning"
    BodyText="Are you sure you want to delete this record? THIS ACTION IS IRREVERSIBLE!" />
      - Public methods:
        - Task<bool> ShowAsync(string title, string bodyText, HandyModalType chosenModalType), returns a boolean of true if the user confirms or false if the user cancels
        - Task<bool> ShowAsync(), returns a boolean of true if the user confirms or false if the user cancels
      - Parameters:
        - Title: String (defaults to "Confirmation")
        - BodyText: String (defaults to "Are you sure?")
        - HandyModalType: HandyModalType (defaults to HandyModalType.INFO)
- **NotificationModal**:
  - This is a pop up that allows you to customize information to display to the end user.
  - private NotificationModal notificationModal = default!;
  - <NotificationModal @ref="notificationModal" />
    - Public methods:
      - Task<bool> ShowAsync(string title, string bodyText, HandyModalType chosenModalType), returns a boolean of true if the user confirms or false if the user cancels
      - Task<bool> ShowAsync(), returns a boolean of true if the user confirms or false if the user cancels
    - Parameters:
      - Title: String (defaults to "Confirmation")
      - BodyText: String (defaults to "Are you sure?")
      - HandyModalType: HandyModalType (defaults to HandyModalType.INFO)
- **CooldownTimer**:
  - This is a tool that you can use to time out users (or hackers) who repeatedly try to access a secure resource.
  - <CooldownTimer @ref="cooldownTimer" CooldownTime="10" MaxAttempts="5" OnCooldownComplete="()=>StateHasChanged()" />
    - Public methods:
      - void IncrementSubmissionCount(), increments the count before the button is disabled
    - Public getter:
      - IsCoolingDown, returns true if the CooldownTimer component is active
    - Parameters:
          - CooldownTime: int (defaults to 30 seconds)
          - MaxAttempts: int (defaults to 5 attempts)
          - OnCooldownComplete: EventCallback (use this to call a function if needed once the cooldown completes)
- **DynamicHandyGrid**:
  - This is a tool that you can use to display your collection of items in a grid view and perform crud operations on it.
  - The documentation for this was pretty complicated to write so I will advise you check out my sample project example. I will make a youtube video on this soon.
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
  - <HandyFileUpload TItem="TestClass" ColumnName="@(nameof(TestClass.File))" Item="o.Object" Style="width: 12rem;" />
  - Parameters:
      - TItem: The class name of your entity
      - ColumnName: The column name of your entity (property name)
      - Item: The HandyGridEntityAbstract Object you want this component to bind to.
      - Style: Gives you the freedom of styling this component using inline css

- **HandyToast**:
  - This is a component that displays users a temporary pop up with the message of their choice
  - <HandyToast @ref="handyToast" Title="Success" Message="Your operation completed successfully." ToastType="HandyToastType.SUCCESS" Duration="5" />
  - Public methods
    - Task ShowToastAsync(string title, string message, HandyToastType toastType): Displays the toast pop up
  - Parameters:
      - ref: a reference to a HandyToast instance so that you can call its public method later
      - Title: The title of this toast popup (defaults to "Notification")
      - Message: The message you would like to inform the user from this toast popup (defaults to "This is a toast message.")
      - Duration: The number of seconds you would like this toast popup to stay visible (defaults to 5 seconds)
      - ToastType: The type of information this toast should identify as (defaults to HandyToastType.INFO)
  Example:
  - Here we show the pop up in a non-blocking and asynchronous way:
    - _ = handyToast.ShowToastAsync("Error", "Something went wrong. Please try again later...", HandyToastType.ERROR);
  
- **StackedHandyToast**:
  - This is a component that displays users a temporary pop up with the message of their choice. If there are multiple messages need to be shown in quick succession, then the newer toasts will stack on top of older ones
  - <StackedHandyToast @ref="stackedHandyToast" Title="Success" Message="Your operation completed successfully." ToastType="HandyToastType.SUCCESS" Position="HandyToastPosition.LEFT_ALIGN" Duration="5" />
  - Public methods
    - Task ShowToastAsync(string title, string message, HandyToastType toastType): Displays the toast pop up
  - Parameters:
      - ref: a reference to a HandyToast instance so that you can call its public method later
      - Title: The title of this toast popup (defaults to "Notification")
      - Message: The message you would like to inform the user from this toast popup (defaults to "This is a toast message.")
      - Duration: The number of seconds you would like this toast popup to stay visible (defaults to 5 seconds)
      - ToastType: The type of information this toast should identify as (defaults to HandyToastType.INFO)
  Example:
  - Here we show the pop up in a non-blocking and asynchronous way:
    - _ = stackedHandyToast.ShowToastAsync("Error", "Something went wrong. Please try again later...", HandyToastType.ERROR);

- **HandyFormTracker**:
  - <HandyFormTracker />
  - Add this to any page with a form. This component is essentially a wrapper around a javascript call that will keep track of the dirtiness of a form, meaning whether edits have been made to the form. If so then the user will get a confirmation popup whenever they navigate away from or refresh the page

- **DownloadComponent**:
  - Users can utilize this component to download byte[] values to their local computer
  - <DownloadComponent />
  - Parameters:
    - LinkText: The text to display to the user to signify that this is the text to click on to download (defaults to "Download")
    - Style: The css style that developers can set to their liking (defaults to "color: blue; text-decoration: underline; cursor:pointer;")
    - Contents: The byte array of contents that is passed to this component to be downloaded
    - HandyDownloadType: The type of file that would be downloaded (defaults to HandyDownloadType.PDF)

- **ObscureInput**:
  - This component is useful for cases such as typing sensitive information (e.g. passwords) into an input box. Users will have the option to toggle showing/hiding their typed characters
  - <ObscureInput />
  - Parameters:
    - Item: The name of your class object to bind to this input
    - Style: The css style that developers can set to their liking (defaults to "width: 10rem")
    - ColumnName: The case-sensitive property name of the object to be two-way binded to this input

## Other Components
- **SaveAsFileComponent**:
  - This component is useful for downloading a string of contents to a csv file
  - <SaveAsFileComponent />
  - Parameters:
    - Text: The text to display to the user to signify that this is the text to click on to download (defaults to "Export as CSV")
    - Style: The css style that developers can set to their liking (defaults to "btn btn-info m-1")
    - Contents: The string of contents that will be downloaded to a .csv file. Please make sure your contents are comma delimited
    - FileName: The name of your csv file (don't append .csv at the end. The component will do it for you)

- **NavigationChecker**:
  - This component will log the user out if the user either idles for too long or their jwt token has expired
  - This component should only be placed in the App.razor
  - <NavigationChecker />
    - Parameters:
      - Seconds: The number of seconds of allowed idle time before logging the user out (defaults to 1800 seconds or half an hour). Please allow at least 20 to 30 minutes of idle time
      - JWT_TOKEN_NAME: The key name of your jwt token when stored in the browser local storage/session storage
      - JWT_TOKEN_EXP_DATE_NAME: The key name of your jwt expiration time stamp when stored in the browser local storage/session storage. Please be sure to store your jwt expiration timestamp in milliseconds
  - Usage instructions:
    - add this as a child of <head> tag:
        - <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    - add this with the other script tags at the tail of the body tag:
        - <script src="_content/FWC.HandyBlazorComponents/handyBlazorComponents.js"></script>
    - add this in program.cs:
      - using HandyBlazorComponents.Extensions;
      - builder.Services.AddHandyBlazorServices();
    - add this line to your _Imports.razor:
      - @inject HandyBlazorService HandyBlazorService
    - Now you can do something like the dirty form demo in the sample project I provided with adding a HandyFormTracker component etc.
    - In your App.razor, make sure your Router tag has a method bound to its OnNavigateAsync parameter like so:
      <Router AppAssembly="@typeof(App).Assembly" OnNavigateAsync="OnNavigateAsync">
    - Make sure to wrap your App.razor with a cascading value:
      - <CascadingValue Value="navigationChecker" Name="@(nameof(NavigationChecker))">
    - Call HandyBlazorService.ResetFormStates() in your App.razor's OnNavigateAsync() method, which would ensure that no forms are dirty each time you navigate to a new page:
      private async Task OnNavigateAsync(NavigationContext args)
      {
          navigationChecker.SetPageDirtyValue(false);
          await HandyBlazorService.ResetFormStates();
      }


C# Services:
HandyBlazorService class:
  - Once you have conducted dependency injection of an instance of this object to your blazor application, you then have several methods to your convenience
  - Public methods:
    - Task ResetFormStates(): Resets all forms to be clean
    - Task StoreJwtExpiration(string jwtName, string jwtExpDateName, DateTime jwtExpDate): Adds your Jwt expiration timestamp to both your browser session and local storages. Will only do so if there's already a jwt token already stored in either the session or local storage
    - Task DownloadFile(string base64, HandyDownloadType handyDownloadType, string fileName): downloads the file that you passed in as a base64
    - More methods will potentially be added



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
    
- Add this to your index.html as a child of your head tag:
  You should at the latest version. In my case at the time of this writing, is ``<script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>``
- Add this to your index.html after your <script src="_framework/blazor.webassembly.js"></script>:
    ``<script src="_content/HandyBlazorComponents/handyBlazorComponents.js"></script>``

This package is still actively under development and changes are constantly made so use at your own risk.

Inside the HandyBlazorComponents directory:
  dotnet pack && dotnet nuget push bin/Release/HandyBlazorComponents.x.x.x.nupkg -s nuget.org -k [insert_your_api_key_here]
  