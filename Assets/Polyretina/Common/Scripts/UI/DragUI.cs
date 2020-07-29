using UnityEngine;

namespace LNE.UI
{
	using VectorExts;

	/// <summary>
	/// Drag the UI element around the screen
	/// </summary>
	public class DragUI : MonoBehaviour
	{
		/*
		 * Editor fields
		 */

#pragma warning disable 649

		[SerializeField]
		private RectTransform _UIElement;

		[SerializeField]
		private MouseButton _dragButton;

		[SerializeField]
		private bool _dragWithoutEventTrigger;

#pragma warning restore 649

		/*
		 * Private fields
		 */

		private Vector2 lastPos;

		/*
		 * Public properties
		 */

		private bool dragging;
		public bool Dragging
		{
			get
			{
				return dragging;
			}

			set
			{
				dragging = value;
			}
		}

		/*
		 * Unity callbacks
		 */

		void Update()
		{
			if (Input.GetMouseButtonDown((int)_dragButton))
			{
				lastPos = Input.mousePosition;
			}

			if ((Dragging || _dragWithoutEventTrigger) && 
				Input.GetMouseButton((int)_dragButton))
			{
				_UIElement.anchoredPosition += Input.mousePosition.SubtractXY(lastPos).XY();
				lastPos = Input.mousePosition;
			}
		}
	}
}
