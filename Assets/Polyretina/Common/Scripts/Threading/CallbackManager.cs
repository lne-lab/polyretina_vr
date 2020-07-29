using System;
using System.Collections;
using UnityEngine;

namespace LNE.Threading
{
	/// <summary>
	/// Invoke callback methods
	/// </summary>
	public static class CallbackManager
	{
		/*
		 * Public events
		 */

		/// <summary>
		/// Called every update
		/// </summary>
		public static event Action OnUpdate;

		/*
		 * Unity callbacks
		 */

		[RuntimeInitializeOnLoadMethod]
		private static void Awake()
		{
			UnityApp.StartCoroutine(Coroutine_Update());
		}

		/*
		 * Public methods
		 */

		/// <summary>
		/// Invoke the callback with a delay
		/// </summary>
		public static void InvokeOnce(float delay, Action callback)
		{
			UnityApp.StartCoroutine(Coroutine_Invoke(delay, callback));
		}
		
		/// <summary>
		/// Invoke until function returns false
		/// </summary>
		public static void InvokeUntil(Func<bool> callback)
		{
			UnityApp.StartCoroutine(Coroutine_Until(0, callback, null));
		}

		/// <summary>
		/// Invoke until function returns false. Ends with a final action
		/// </summary>
		public static void InvokeUntil(Func<bool> callback, Action final)
		{
			UnityApp.StartCoroutine(Coroutine_Until(0, callback, final));
		}

		/// <summary>
		/// Invoke until function returns false
		/// </summary>
		public static void InvokeUntil(float delay, Func<bool> callback)
		{
			UnityApp.StartCoroutine(Coroutine_Until(delay, callback, null));
		}

		/// <summary>
		/// Invoke until function returns false. Ends with a final action
		/// </summary>
		public static void InvokeUntil(float delay, Func<bool> callback, Action final)
		{
			UnityApp.StartCoroutine(Coroutine_Until(delay, callback, final));
		}

		/// <summary>
		/// Linearly interpolate value over time
		/// </summary>
		public static void Lerp(float from, float to, float lerpTime, Action<float> action)
		{
			Lerp(from, to, lerpTime, action, null);
		}

		/// <summary>
		/// Linearly interpolate value over time. Ends with a final action
		/// </summary>
		public static void Lerp(float from, float to, float lerpTime, Action<float> action, Action final)
		{
			var startTime = Time.time;

			InvokeUntil(() =>
			{
				var currTime = Time.time - startTime;
				var value = Mathf.Lerp(from, to, currTime / lerpTime);
				action(value);
				return currTime < lerpTime;
			}, 
			final);
		}

		/*
		 * Private methods
		 */

		private static IEnumerator Coroutine_Update()
		{
			while (Application.isPlaying)
			{
				OnUpdate?.Invoke();
				yield return null;
			}
		}

		private static IEnumerator Coroutine_Invoke(float delay, Action callback)
		{
			yield return new WaitForSeconds(delay);
			callback();
		}

		private static IEnumerator Coroutine_Until(float delay, Func<bool> callback, Action final)
		{
			yield return new WaitForSeconds(delay);

			while (callback())
			{
				yield return null;
			}

			final?.Invoke();
		}
	}
}
