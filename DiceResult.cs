namespace csharp_windows_desktop_gui;

using System.Text;

public class DiceResult {
    public int[] Dice { get; set;}

    public DiceResult(int[] inputThrows) {
        Dice = inputThrows;
    }
    public override string ToString()
    {
        var builder = new StringBuilder();

        for(var i = 0; i < Dice.Length; i++) {
            var current = Dice[i];

            if(i == 0) {
                builder.Append($"{current}");
            }

            else {
                builder.Append($", {current}");
            }
        }
        return builder.ToString();
    }
}
