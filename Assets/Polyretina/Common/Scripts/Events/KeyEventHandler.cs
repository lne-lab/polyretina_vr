#pragma warning disable 649

using System;
using UnityEngine;
using UnityEngine.Events;

namespace LNE.Events
{
	/// <summary>
	/// Handles key down events
	/// </summary>
	public class KeyEventHandler : MonoBehaviour
	{
		[SerializeField]
		private KeyEvent[] _events = new KeyEvent[1];

		void Update()
		{
			foreach (var keyEvent in _events)
			{
				if (Input.GetKeyDown(keyEvent.trigger))
				{
					keyEvent.unityEvent.Invoke(keyEvent.trigger);
				}
			}
		}
	}

	[Serializable]
	public class KeyEvent
	{
		[Serializable]
		public class UnityEvent_KeyCode : UnityEvent<KeyCode> { }

		public KeyCode trigger;
		public UnityEvent_KeyCode unityEvent;

		// for editor
		public bool foldout;
	}
}
