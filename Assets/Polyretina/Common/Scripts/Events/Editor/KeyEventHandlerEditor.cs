using UnityEngine;
using UnityEditor;

namespace LNE.Events.UI
{
	[CustomEditor(typeof(KeyEventHandler))]
	public class KeyEventHandlerEditor : EventHandlerEditor<KeyEventHandler>
	{
		protected override string TriggerName { get => "Key Code"; }
	}
}
