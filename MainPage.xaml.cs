namespace csharp_windows_desktop_gui;

using System.Net.Http;

public partial class MainPage : ContentPage
{


	public MainPage()
	{
		InitializeComponent();
	}

	private void OnRollClicked(object sender, EventArgs e)
	{
		

		SemanticScreenReader.Announce("");
	}
}

