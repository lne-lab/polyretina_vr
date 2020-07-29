#pragma warning disable 649

using UnityEngine;

namespace LNE.Demos.Objects
{
	public class ObjectSpawner : MonoBehaviour
	{
		[SerializeField]
		private GameObject[] objects;

		private int objectIndex;
		private GameObject instantiated;
		
		private GameObject nextObject
		{
			get
			{
				objectIndex++;
				if (objectIndex == objects.Length)
				{
					objectIndex = 0;
				}

				return objects[objectIndex];
			}
		}

		private GameObject prevObject
		{
			get
			{
				objectIndex--;
				if (objectIndex < 0)
				{
					objectIndex = objects.Length - 1;
				}

				return objects[objectIndex];
			}
		}

		void Start()
		{
			SpawnNextObject();
		}

		public void SpawnNextObject()
		{
			if (instantiated != null)
			{
				Destroy(instantiated);
			}

			instantiated = Instantiate(nextObject);
		}

		public void SpawnPrevObject()
		{
			if (instantiated != null)
			{
				Destroy(instantiated);
			}

			instantiated = Instantiate(prevObject);
		}
	}
}
