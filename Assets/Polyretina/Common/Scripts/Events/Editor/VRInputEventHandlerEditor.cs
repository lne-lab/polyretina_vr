using UnityEditor;

namespace LNE.Events.UI
{
	[CustomEditor(typeof(VRInputEventHandler))]
	public class VRInputEventHandlerEditor : EventHandlerEditor<VRInputEventHandler>
	{
		protected override string TriggerName { get => "VR Button"; }
	}
}
