namespace csharp_windows_desktop_maui;

using Microsoft.Maui.Graphics;

public class DiceView {
    public string Value {get; set;}
    public Color BackgroundColour {get; set;}

    public DiceView(string inputValue, Color backgroundColour) {
        Value = inputValue;
        BackgroundColour = backgroundColour;
    }
}
