using UnityEngine;

namespace LNE.UI
{
	/// <summary>
	/// Enlarge UI element using the scrollwheel
	/// </summary>
	[RequireComponent(typeof(RectTransform))]
	public class ZoomUI : MonoBehaviour
	{
		/*
		 * Editor fields
		 */

#pragma warning disable 649

		[SerializeField]
		private float _zoomSpeed;

		[SerializeField]
		private float _minZoom;

		[SerializeField]
		private float _maxZoom;

#pragma warning restore 649

		/*
		 * Public properties
		 */

		public Vector2 StartingSize { get; set; }

		/*
		 * Private properties
		 */

		private RectTransform uiElement => GetComponent<RectTransform>();

		/*
		 * Unity callbacks
		 */ 

		void Start()
		{
			StartingSize = uiElement.sizeDelta;
		}

		void Update()
		{
			uiElement.sizeDelta *= 1 + Input.GetAxis("Mouse ScrollWheel") * _zoomSpeed;
			uiElement.sizeDelta = AuxMath.Clamp(uiElement.sizeDelta, StartingSize * _minZoom, StartingSize * _maxZoom);
		}
	}
}
