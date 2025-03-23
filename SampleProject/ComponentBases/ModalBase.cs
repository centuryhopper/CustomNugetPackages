using HandyBlazorComponents.Models;
using Microsoft.AspNetCore.Components;

namespace SampleProject.ComponentBases;

public class ModalBase : ComponentBase
{
    protected bool IsVisible { get; set; }
    protected TaskCompletionSource<bool>? TaskCompletionSource;
    protected string ModalClass = string.Empty;
    [Parameter] public string Title { get; set; } = "Confirmation";
    [Parameter] public string BodyText { get; set; } = "Are you sure?";
    [Parameter] public HandyModalType HandyModalType { get; set; } = HandyModalType.INFO;
    public virtual Task<bool> ShowAsync(string title, string bodyText, HandyModalType chosenModalType)
    {
        // prioritize user passed in parameters over component parameters
        HandyModalType = chosenModalType;
        Title = title;
        BodyText = bodyText;
        SetModalClass(HandyModalType);
        IsVisible = true;
        TaskCompletionSource = new TaskCompletionSource<bool>();
        StateHasChanged();
        return TaskCompletionSource.Task;
    }

    public virtual Task<bool> ShowAsync()
    {
        SetModalClass(HandyModalType);
        IsVisible = true;
        TaskCompletionSource = new TaskCompletionSource<bool>();
        StateHasChanged();
        return TaskCompletionSource.Task;
    }

    public virtual void Confirm()
    {
        IsVisible = false;
        TaskCompletionSource?.SetResult(true);
        StateHasChanged();
    }

    public virtual void Cancel()
    {
        TaskCompletionSource?.SetResult(false);
        IsVisible = false;
        StateHasChanged();
    }

    private void SetModalClass(HandyModalType modalType)
    {
        ModalClass = modalType switch
        {
            HandyModalType.SUCCESS => "text-success",
            HandyModalType.WARNING => "text-warning",
            HandyModalType.ERROR => "text-danger",
            HandyModalType.INFO => "text-info",
            // default to text-info
            _ => "text-info"
        };
    }
}

