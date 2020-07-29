#pragma warning disable 649

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LNE.UI
{
	[Serializable]
	public class UnityEvent_String : UnityEvent<string>
	{

	}

	public class InputFieldOnEnter : MonoBehaviour
	{
		[SerializeField]
		private InputField inputField;

		[SerializeField]
		private UnityEvent_String onEnter;

		void Update()
		{
			if (inputField.isFocused && inputField.text != "" && Input.GetKey(KeyCode.Return))
			{
				onEnter.Invoke(inputField.text);
			}
		}
	}
}