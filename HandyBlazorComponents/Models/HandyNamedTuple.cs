
using Microsoft.AspNetCore.Components;

namespace HandyBlazorComponents.Models;

public class HandyNamedTuple
{
    public string Text { get; set; }
    public string Value { get; set; }

    public HandyNamedTuple(string Text, string Value)
    {
        this.Text = Text;
        this.Value = Value;
    }
}
