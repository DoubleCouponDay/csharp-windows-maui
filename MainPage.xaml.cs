namespace csharp_windows_desktop_gui;

using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using WinRT;

public partial class MainPage : ContentPage
{
	const int MINIMUM = 1;
	const int MAXIMUM = 6;
	const int REROLLS = 1;
	const string DICEROLL_API = "http://coreservice.xyz/arc/api.php";

	public MainPage()
	{
		InitializeComponent();
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
		var url = $"{DICEROLL_API}?minimum={minimum}&maximum={maximum}&rerolls={rerolls}";
		HttpResponseMessage response = httpClient.GetAsync(url).Result;
		string? result = response.Content.ReadAsStringAsync().Result ?? "";
		Console.WriteLine("Result:");
		Console.WriteLine(result.ToString());
	}
}

