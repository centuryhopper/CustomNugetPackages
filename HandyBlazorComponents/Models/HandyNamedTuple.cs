
using Microsoft.AspNetCore.Components;

namespace HandyBlazorComponents.Models;
// TODO: Copy over to fwc handy blazor components
public class HandyNamedTuple
{
    public string Text { get; set; }
    public string Value { get; set; }

    public HandyNamedTuple()
    {
        this.Text = string.Empty;
        this.Value = string.Empty;
    }

    public HandyNamedTuple(string Text, string Value)
    {
        this.Text = Text;
        this.Value = Value;
    }
}
