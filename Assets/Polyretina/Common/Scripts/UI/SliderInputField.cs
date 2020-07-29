#pragma warning disable 649

using UnityEngine;
using UnityEngine.UI;

namespace LNE.UI
{
	public class SliderInputField : MonoBehaviour
	{
		[SerializeField]
		private Slider slider;

		[SerializeField]
		private InputField inputField;

		void Start()
		{
			slider.onValueChanged.AddListener(OnSliderValueChanged);
			inputField.onEndEdit.AddListener(OnInputFieldValueChanged);
		}

		public void OnSliderValueChanged(float value)
		{
			inputField.text = value.ToString();
		}

		public void OnInputFieldValueChanged(string value)
		{
			slider.value = float.Parse(value);
		}
	}
}
