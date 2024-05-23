namespace csharp_windows_desktop_maui;

using System.Net.Http;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using Microsoft.Maui.Graphics;
using System.Collections.ObjectModel;

public partial class MainPage : ContentPage, IDisposable
{
	private HttpClient http = new HttpClient();

	public ObservableCollection<List<ColouredDice>> resultsTable { get; set; }

	public MainPage()
	{
		InitializeComponent();
		resultsTable = new ObservableCollection<List<ColouredDice>>();
		BindingContext = this;
		resultsTitle.IsVisible = false;
	}

	private void OnRollClicked(object sender, EventArgs e)
	{
		UserInput? validatedInput = ValidateInput();

		if (validatedInput == null) {
			return;
		}
		DiceResult? result = RequestDicerolls(validatedInput);

		if(result == null) {
			return;
		}
		var newRow = new List<ColouredDice>(result.Dice.Length);
		(int minimum, int maximum) = FindLimits(result, newRow);
		newRow[minimum].BackgroundColour = Color.FromArgb(Colours.Highlight);
		newRow[maximum].BackgroundColour = Color.FromArgb(Colours.Highlight);
		resultsTable.Add(newRow);
		resultsTitle.IsVisible = true;
	}

	/// <summary>
	/// Returns the validated user input. Null if not valid.
	/// </summary>
	/// <returns></returns>
	private UserInput? ValidateInput() {
		var input = new UserInput {
			Minimum = Defaults.MINIMUM,
			Maximum = Defaults.MAXIMUM,
			Rerolls = Defaults.REROLLS
		};

		try {
			input.Minimum = int.Parse(minimumEntry.Text);
		}

		catch {
			Console.WriteLine(Error.InvalidMinimum);
			DisplayAlert(nameof(Error), Error.InvalidMinimum, UI.Okay);
			return null;
		}

		try {
			input.Maximum = int.Parse(maximumEntry.Text);
		}

		catch {
			Console.WriteLine(Error.InvalidMaximum);
			DisplayAlert(nameof(Error), Error.InvalidMaximum, UI.Okay);
			return null;
		}

		try {
			input.Rerolls = int.Parse(rerollsEntry.Text);
		}

		catch {
			Console.WriteLine(Error.InvalidRerolls);
			DisplayAlert(nameof(Error), Error.InvalidRerolls, UI.Okay);
			return null;
		}
		return input;
	}

	/// <summary>
	/// Gets the dicerolls from the api. Returns null connection failed.
	/// </summary>
	/// <param name="input"></param>
	/// <returns></returns>
	/// <exception cref="Exception"></exception>
	private DiceResult? RequestDicerolls(UserInput input) {

		var url = $"{Endpoints.Diceroller}?min={input.Minimum}&max={input.Maximum}&count={input.Rerolls}";
		HttpResponseMessage response;
		
		try {
			response = http.GetAsync(url).Result;
		}

		catch {
			Console.WriteLine(Error.ConnectionFailed);
			DisplayAlert(nameof(Error), Error.ConnectionFailed, UI.Okay);
			return null;
		}
		string stringResult = response.Content.ReadAsStringAsync().Result ?? throw new Exception($"null result from url: {url}");
		DiceResult result = JsonConvert.DeserializeObject<DiceResult>(stringResult) ?? throw new Exception("returned value cannot be serialised as dataclass DiceResult");
		Console.Write("Result:");
		Console.WriteLine(stringResult.ToString());
		return result;
	}

	/// <summary>
	/// Returns the index for the minimum and maximum values in the DiceResult
	/// </summary>
	/// <param name="result"></param>
	/// <param name="newRow"></param>
	/// <returns></returns>
	private (int minimum, int maximum) FindLimits(DiceResult result, List<ColouredDice> newRow) {
		var minimumIndex = 0;
		var maximumIndex = 0;
		var currentMin = int.MaxValue;
		var currentMax = 0;

		for(var i = 0; i < result.Dice.Length; i++) {
			int current = result.Dice[i];

			if(current <= currentMin) {
				minimumIndex = i;
				currentMin = current;
			}

			if(current >= currentMax) {
				maximumIndex = i;
				currentMax = current;
			}

			var valueString = result.Dice[i].ToString();
			var white = new Color(Defaults.RGB_MAX, Defaults.RGB_MAX, Defaults.RGB_MAX);
			var cell = new ColouredDice(valueString, white); //white

			newRow.Add(cell);
		}
		return (minimumIndex, maximumIndex);
	}

    public void Dispose() {
        http.Dispose();
    }
}
