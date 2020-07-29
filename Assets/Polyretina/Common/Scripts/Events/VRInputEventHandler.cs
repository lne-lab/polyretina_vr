using System;
using UnityEngine;
using UnityEngine.Events;

namespace LNE.Events
{
	using VR;

	/// <summary>
	/// Handles VR Input events
	/// </summary>
	public class VRInputEventHandler : MonoBehaviour
	{
		public VRInputEvent[] _events = new VRInputEvent[1];

		void Update()
		{
			foreach (var vrInputEvent in _events)
			{
				if (VRInput.GetButtonDown(vrInputEvent.trigger)		|| 
					VRInput.GetTouchDown(vrInputEvent.trigger)		||
					VRInput.GetAxisAsButtonDown(vrInputEvent.trigger))
				{
					vrInputEvent.unityEvent.Invoke(vrInputEvent.trigger);
				}
			}
		}
	}

	[Serializable]
	public class VRInputEvent
	{
		[Serializable]
		public class UnityEvent_VRButton : UnityEvent<VRButton> { }

		public VRButton trigger;
		public UnityEvent_VRButton unityEvent;

		// for editor
		public bool foldout;
	}
}
