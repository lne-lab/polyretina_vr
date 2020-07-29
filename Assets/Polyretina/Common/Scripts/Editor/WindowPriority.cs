
namespace LNE.UI
{
	public static partial class WindowPriority
	{
		public const int settingsWindow = 3;
		public const int viveStartAll = 4;
		public const int viveStartWireless = viveStartAll + 11;
		public const int viveStartSteam = viveStartWireless + 1;
		public const int viveStartSteamVR = viveStartSteam + 1;
		public const int viveStartEyeTracking = viveStartSteamVR + 1;
		public const int screenshot = viveStartEyeTracking + 11;
	}
}
