#pragma warning disable 649

using System;
using UnityEngine;
using UnityEngine.Events;

namespace LNE.Events
{
	using Threading;

	/// <summary>
	/// Handles chronological events
	/// </summary>
	public class TimeEventHandler : MonoBehaviour
	{
		[SerializeField]
		private TimeEvent[] _events = new TimeEvent[1];

		void Start()
		{
			foreach (var timeEvent in _events)
			{
				CallbackManager.InvokeOnce(timeEvent.trigger, () => timeEvent.unityEvent.Invoke(timeEvent.trigger));
			}
		}
	}

	[Serializable]
	public class TimeEvent
	{
		[Serializable]
		public class UnityEvent_Time : UnityEvent<float> { }

		public float trigger;
		public UnityEvent_Time unityEvent;

		// for editor
		public bool foldout;
	}
}
