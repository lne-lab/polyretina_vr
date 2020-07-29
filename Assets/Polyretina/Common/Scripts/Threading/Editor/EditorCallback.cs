using System;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;

namespace LNE.Threading.UI
{
	/// <summary>
	/// Invoke callback methods on the main Unity thread in the editor
	/// </summary>
	[InitializeOnLoad]
	public static class EditorCallback
	{
		private static Thread mainThread = null;
		private static Queue<MutexCallback> callbacks = new Queue<MutexCallback>();
		
		public static bool OnMainThread
		{
			get
			{
				return Thread.CurrentThread.Equals(mainThread);
			}
		}

		static EditorCallback()
		{
			mainThread = Thread.CurrentThread;
			EditorApplication.update += EditorUpdate;
		}
		
		static void EditorUpdate()
		{
			while (callbacks.Count > 0)
			{
				var job = callbacks.Dequeue();
				
				job?.callback();
				job.mutex?.Set();
			}
		}

		/// <summary>
		/// Invoke callback method on the main Unity thread in the editor
		/// </summary>
		public static void InvokeOnMainThread(Action callback, bool block = true)
		{
			if (OnMainThread)
			{
				callback();
			}
			else
			{
				var mutex = block ? new AutoResetEvent(false) : null;

				callbacks.Enqueue(new MutexCallback(callback, mutex));

				mutex?.WaitOne();
			}
		}
	}
	
	public class MutexCallback
	{
		public Action callback;
		public AutoResetEvent mutex;

		public MutexCallback(Action callback, AutoResetEvent mutex)
		{
			this.callback = callback;
			this.mutex = mutex;
		}
	}
}
