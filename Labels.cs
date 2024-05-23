namespace csharp_windows_desktop_maui;

public static class Error {
    public static string InvalidMinimum = "failed to parse minimum";
    public static string InvalidMaximum = "failed to parse maximum";
    public static string InvalidRerolls = "failed to parse maximum";
    public static string ConnectionFailed = "Connection to server failed";
}

public static class Colours {
    public static string Highlight ="#F37621";
    public static string ButtonBackground = "#F05324";
}

public static class Defaults {
	public static int RGB_MAX = 255;
    public static int MINIMUM = 1;
	public static int MAXIMUM = 6;
	public static int REROLLS = 1;
}

public static class Endpoints {
    public static string Diceroller = "http://coreservice.xyz/arc/api.php";
}

public static class UI {
    public static string Okay = "Ok";
}