using System;
using System.Threading;

namespace LNE.Threading
{
	/// <summary>
	/// Create a group of threads used for parallel processing
	/// </summary>
	public class ThreadGroup
	{
		/*
		 * Private fields
		 */

		private AutoResetEvent mutex = new AutoResetEvent(false);

		/*
		 * Public properties
		 */

		public Thread[] Threads { get; private set; }
		public float[] Progress { get; private set; }

		public int NumThreads
		{
			get
			{
				return Threads.Length;
			}
		}

		private int numThreadsFinished;
		public int NumThreadsFinished
		{
			get
			{
				return numThreadsFinished;
			}

			private set
			{
				numThreadsFinished = value;

				if (numThreadsFinished == NumThreads)
				{
					OnAllThreadsFinished?.Invoke();
				}
			}
		}

		/*
		 * Public events
		 */

		public event Action OnAllThreadsFinished;

		/*
		 * Constructor / Destructor
		 */

		/// <summary>
		/// Group of threads used for parallel processing. Number of threads will equal the processor count.
		/// </summary>
		public ThreadGroup() : this(Environment.ProcessorCount)
		{

		}

		/// <summary>
		/// Group of threads used for parallel processing.
		/// </summary>
		public ThreadGroup(int numThreads)
		{
			Threads = new Thread[numThreads];
			Progress = new float[numThreads];

			NumThreadsFinished = numThreads;
		}
		
		~ThreadGroup()
		{
			Abort();
		}

		/*
		 * Public methods
		 */

		/// <summary>
		/// Execute callback "length" number of times distributed equally among threads.
		/// </summary>
		public void ProcessArray(int length, Action<int> callback)
		{
			var threadsNeeded = Math.Min(NumThreads, length);

			NumThreadsFinished = NumThreads - threadsNeeded;

			var offset = 0;
			var size = length / threadsNeeded;
			for (int i = 0; i < threadsNeeded; i++)
			{
				if (i == threadsNeeded - 1)
				{
					size += length % threadsNeeded;
				}

				Threads[i] = new Thread(() => ProcessPart(i, offset, size, callback));
				Threads[i].Start();
				mutex.WaitOne();

				offset += size;
			}
		}

		/// <summary>
		/// Join all threads
		/// </summary>
		public void Join()
		{
			foreach (var thread in Threads)
			{
				thread?.Join();
			}
		}

		/// <summary>
		/// Abort all threads
		/// </summary>
		public void Abort()
		{
			foreach (var thread in Threads)
			{
				thread?.Abort();
			}

			Threads = new Thread[NumThreads];
			Progress = new float[NumThreads];
		}


		/*
		 * Private methods
		 */

		private void ProcessPart(int threadId, int offset, int size, Action<int> processor)
		{
			mutex.Set();
			
			for (int i = offset; i < offset + size; i++)
			{
				processor(i);
				Progress[threadId] = (float)(i - offset + 1) / size;
			}

			lock (this)
			{
				NumThreadsFinished++;
			}
		}
	}
}
