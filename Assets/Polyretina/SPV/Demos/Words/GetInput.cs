#pragma warning disable 649

using UnityEngine;
using UnityEngine.UI;

namespace LNE.Demos.Words
{
	[RequireComponent(typeof(Text))]
	public class GetInput : MonoBehaviour
	{
		private Text textBox => GetComponent<Text>();

		void Update()
		{
			foreach (var c in Input.inputString)
			{
				GetChar(c);
			}
		}

		private void GetChar(char c)
		{
			if (c == '\b' && textBox.text.Length > 0)
			{
				textBox.text = textBox.text.Remove(textBox.text.Length - 1);
			}
			else if (c == '\r')
			{
				textBox.text += '\n';
			}
			else
			{
				textBox.text += c;
			}
		}
	}
}
