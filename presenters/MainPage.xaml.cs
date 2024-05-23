﻿namespace csharp_windows_desktop_maui;

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

		DiceResult result = RequestDicerolls(validatedInput);
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
			Console.WriteLine(Errors.InvalidMinimum);
			return null;
		}

		try {
			input.Maximum = int.Parse(maximumEntry.Text);
		}

		catch {
			Console.WriteLine(Errors.InvalidMaximum);
			return null;
		}

		try {
			input.Rerolls = int.Parse(rerollsEntry.Text);
		}

		catch {
			Console.WriteLine(Errors.InvalidRerolls);
			return null;
		}
		return input;
	}

	private DiceResult RequestDicerolls(UserInput input) {

		var url = $"{Endpoints.Diceroller}?min={input.Minimum}&max={input.Maximum}&count={input.Rerolls}";
		HttpResponseMessage response = http.GetAsync(url).Result;
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
