#pragma warning disable 649

using UnityEngine;

namespace LNE.Studies
{
	using ArrayExts;
	using ProstheticVision;

	/// <summary>
	/// Object recognition study
	/// </summary>
	public class ObjRecStudy : Study
	{
		[Header("Items")]

		/*
		 * Inspector fields
		 */

		[SerializeField]
		private Transform[] _objects;

		/*
		 * Private fields
		 */

		private float prevVisualAngle;

		/*
		 * Overridden methods
		 */

		protected override object[] InitialiseItems()
		{
			_objects = _objects.Randomise(_identifier - 1);

			if (_objects.Length % numConditions != 0)
			{
				var objectsPerCondition = _objects.Length / numConditions;
				_objects = _objects.Subarray(0, objectsPerCondition * numConditions);
			}

			return _objects;
		}

		protected override object InstantiateItem(object item, float visualAngle)
		{
			var clone = Instantiate(item as Transform);

			prevVisualAngle = 35;
			SetVisualAngle(clone, visualAngle);
			
			return clone;
		}

		protected override void ShowItem()
		{
			if (instantiatedItem != null)
			{
				(instantiatedItem as Transform).gameObject.SetActive(true);
			}
		}

		protected override void HideItem()
		{
			if (instantiatedItem != null)
			{
				(instantiatedItem as Transform).gameObject.SetActive(false);
			}
		}

		protected override void DestroyItem()
		{
			if (instantiatedItem != null)
			{
				Destroy((instantiatedItem as Transform).gameObject);
				instantiatedItem = null;
			}
		}

		public override string GetItemName()
		{
			return (instantiatedItem as Transform).name.Split('(')[0];
		}

		public override string GetItemName(object item)
		{
			return (item as Transform).name.Split('(')[0];
		}

		public override void SetVisualAngle(float visualAngle)
		{
			SetVisualAngle(instantiatedItem as Transform, visualAngle);
		}

		/*
		 * Private methods
		 */

		private void SetVisualAngle(Transform t, float visualAngle)
		{
			var stdUm = CoordinateSystem.VisualAngleToRetina(prevVisualAngle);
			var reqUm = CoordinateSystem.VisualAngleToRetina(visualAngle);
			var delta = reqUm / stdUm;

			t.localScale *= delta;

			// reposition object to the center of the screen
			Reposition(t);
			Reposition(t);
			Reposition(t);

			// and set previous visual angle...
			prevVisualAngle = visualAngle;
		}

		private void ScaleToVisualAngle(Transform t, float angle)
		{
			Resize(t, angle);
			Reposition(t);
		}

		private void Resize(Transform t, float angle)
		{
			var targetTan = Mathf.Tan(angle / 2 * Mathf.Deg2Rad);
			var actualAdj = CameraToItem();
			var targetOpp = targetTan * actualAdj;
			var actualOpp = ItemExtent(t);

			var actualToTarget = targetOpp / actualOpp;

			t.localScale *= actualToTarget;
		}

		private void Reposition(Transform t)
		{
			var bounds = ItemBounds(t);

			t.position -= bounds.center;
		}

		private float CameraToItem()
		{
			return Prosthesis.Instance.transform.position.magnitude;
		}

		private float ItemExtent(Transform t)
		{
			var bounds = ItemBounds(t);

			return Mathf.Max(bounds.extents.x, bounds.extents.y, bounds.extents.z);
		}

		private Bounds ItemBounds(Transform t)
		{
			var bounds = new Bounds();
			foreach (var renderer in t.GetComponentsInChildren<Renderer>())
			{
				bounds.Encapsulate(renderer.bounds);
			}

			return bounds;
		}

		/* Warning: Enrico's code below */

		private void ScaleToAngle(Transform t, float angle)
		{
			float diagonal = GetBounds(t).size.magnitude;
			float target = Mathf.Abs(2f * Mathf.Sin(angle * Mathf.Deg2Rad / 2f) * Prosthesis.Instance.transform.position.magnitude);
			Bounds origBounds = GetBounds(t);
			t.localScale = t.localScale * (target / diagonal);
			t.position = ElemwiseProduct(origBounds.center, t.localScale) * (-1f);
		}

		private Bounds GetBounds(Transform t)
		{
			Renderer renderer = t.GetComponent<Renderer>();
			Bounds combinedBounds;
			if (renderer != null)
			{// If the object has a renderer, it's not a group.
				combinedBounds = renderer.bounds;
			}
			else
			{
				combinedBounds = new Bounds(t.position, new Vector3(0, 0, 0));
			}
			// There are objects with a renderer whose children are objects with a renderer
			foreach (Transform child in t.transform)
			{
				combinedBounds.Encapsulate(GetBounds(child));
			}
			return combinedBounds;
		}

		private Vector3 ElemwiseProduct(Vector3 first, Vector3 second)
		{
			return new Vector3(first.x * second.x, first.y * second.y, first.z * second.z);
		}
	}
}
