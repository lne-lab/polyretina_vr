using UnityEditor;

namespace LNE.Events.UI
{
	[CustomEditor(typeof(TimeEventHandler))]
	public class TimeEventHandlerEditor : EventHandlerEditor<TimeEventHandler>
	{
		protected override string TriggerName { get => "Time"; }
	}
}
