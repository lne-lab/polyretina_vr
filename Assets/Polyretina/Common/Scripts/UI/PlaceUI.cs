using UnityEngine;

namespace LNE.UI
{
	using VectorExts;

	/// <summary>
	/// Place the UI element around the screen
	/// </summary>
	public class PlaceUI : MonoBehaviour
	{
		/*
		 * Editor fields
		 */

#pragma warning disable 649

		[SerializeField]
		private RectTransform _UIElement;

		[SerializeField]
		private MouseButton _placeButton;

#pragma warning restore 649

		/*
		 * Unity callbacks
		 */

		void Update()
		{
			if (Input.GetMouseButtonDown((int)_placeButton))
			{
				_UIElement.anchoredPosition = Input.mousePosition.SubtractXY(Screen.width / 2, Screen.height / 2);
			}
		}
	}
}
