namespace csharp_windows_desktop_gui;

using System.Net;
using System.Net.Http;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using Microsoft.Maui.Graphics;
using System.Collections.ObjectModel;

public partial class MainPage : ContentPage
{
	const int MINIMUM = 1;
	const int MAXIMUM = 6;
	const int REROLLS = 1;
	const string DICEROLL_API = "http://coreservice.xyz/arc/api.php";
	const int RGB_MAX = 255;
	const string HIGHLIGHT_HEX = "#F37621";

	public ObservableCollection<List<DiceView>> resultsTable {get; set;} =  new ObservableCollection<List<DiceView>>();

	public MainPage()
	{
		InitializeComponent();
		BindingContext = this;
		resultsTitle.IsVisible = false;
	}

	private void OnRollClicked(object sender, EventArgs e)
	{
		var minimum = MINIMUM;
		var maximum = MAXIMUM;
		var rerolls = REROLLS;

		try {
			minimum = int.Parse(minimumEntry.Text);
		}

		catch {
			Console.WriteLine("failed to parse minimum");
		}

		try {
			maximum = int.Parse(maximumEntry.Text);
		}

		catch {
			Console.WriteLine("failed to parse maximum");
		}

		try {
			rerolls = int.Parse(rerollsEntry.Text);
		}

		catch {
			Console.WriteLine("failed to parse rerolls");
		}

		using var httpClient = new HttpClient();
		var url = $"{DICEROLL_API}?min={minimum}&max={maximum}&count={rerolls}";
		HttpResponseMessage response = httpClient.GetAsync(url).Result;
		string result = response.Content.ReadAsStringAsync().Result ?? throw new Exception($"null result from url: {url}");
		resultsTitle.IsVisible = true;
		DiceResult resultObject = JsonConvert.DeserializeObject<DiceResult>(result) ?? throw new Exception("returned value cannot be serialised as dataclass DiceResult");
		Console.Write("Result:");
		Console.WriteLine(result.ToString());
		int minimumIndex = 0;
		int maximumIndex = 0;
		var newRow = new List<DiceView>(resultObject.Dice.Length);

		for(var i = 0; i < resultObject.Dice.Length; i++) {
			var current = resultObject.Dice[i];

			if(current < minimumIndex) {
				minimumIndex = current;
			}

			else if(current > maximumIndex) {
				maximumIndex = current;
			}

			var cell = new DiceView(resultObject.Dice[i].ToString(), new Color(RGB_MAX, RGB_MAX, RGB_MAX));
			newRow.Add(cell);
		}
		bool maximumHighlighted = false;
		bool minimumHighlighted = false;

		for(var i = 0; i < resultObject.Dice.Length; i++) {
			var current = resultObject.Dice[i];

			if(maximumHighlighted == false && current == maximum) {
				newRow[i].BackgroundColour = Color.FromArgb(HIGHLIGHT_HEX);
				maximumHighlighted = true;
			}

			else if(minimumHighlighted == false && current == minimum) {
				newRow[i].BackgroundColour = Color.FromArgb(HIGHLIGHT_HEX);
				minimumHighlighted = true;
			}
		}
		resultsTable.Add(newRow);
	}
}

