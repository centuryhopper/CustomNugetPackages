
using Microsoft.AspNetCore.Components;

namespace HandyBlazorComponents.Models;

public class NamedRenderFragment<T>
{
    public string Name { get; set; } // The string property
    public RenderFragment<T> Fragment { get; set; } // The RenderFragment<T> property

    public NamedRenderFragment(string Name, RenderFragment<T> Fragment)
    {
        this.Name = Name;
        this.Fragment = Fragment;
    }
}
