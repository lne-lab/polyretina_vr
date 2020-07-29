#pragma warning disable 649

using UnityEngine;
using UnityEngine.UI;

namespace LNE.Studies
{
	using ArrayExts;

	/// <summary>
	/// Word reading study
	/// </summary>
	public class ReadingStudy : Study
	{
		[Header("Items")]

		/*
		 * Inspector fields
		 */

		[SerializeField]
		private TextAsset _words;

		[SerializeField]
		private int _wordsPerCondition;

		[Space]

		[SerializeField]
		private Text _textBox;
		
		/*
		 * Overridden methods
		 */

		protected override object[] InitialiseItems()
		{			
			var items = _words
						.text
						.Split('\n')
						.Apply((s) => s.Trim('\r'))
						.Randomise(_identifier - 1);

			var numExposures = numConditions * _wordsPerCondition;
			if (numExposures <= items.Length)
			{
				return items.Subarray(0, numExposures);
			}
			else
			{
				var maxWordsPerCondition = (int)((float)items.Length / numConditions);
				Debug.LogError("Too many words per condition. Maximum number of words per condition is " + maxWordsPerCondition.ToString() + ".");
				return null;
			}
		}
		
		protected override object InstantiateItem(object item, float visualAngle)
		{
			_textBox.text = item as string;
			ScaleTextToAngle(_textBox, item as string, visualAngle);

			return item;

		}

		protected override void ShowItem()
		{
			_textBox.text = instantiatedItem as string;
		}

		protected override void HideItem()
		{
			_textBox.text = "";
		}

		protected override void DestroyItem()
		{
			instantiatedItem = "";
		}

		public override string GetItemName()
		{
			return instantiatedItem as string;
		}

		public override string GetItemName(object item)
		{
			return item as string;
		}

		public override void SetVisualAngle(float visualAngle)
		{
			ScaleTextToAngle(_textBox, _textBox.text, visualAngle);
		}

		public void SetVisualAngle(string visualAngle)
		{
			try
			{
				SetVisualAngle(float.Parse(visualAngle));
			}
			catch
			{

			}
		}

		/*
		 * Private methods
		 */

		/* Warning: Enrico's code below */

		void ScaleTextToAngle(Text t, string _, float angle)
		{
			float targetHeight = Mathf.Abs(2f * Mathf.Sin(angle * Mathf.Deg2Rad / 2f) * (GameObject.Find("Polyretina").transform.position - t.transform.position).magnitude);
			int targetSize = (int)Mathf.Min(Mathf.Floor(targetHeight * 24f / 13f), 64); // Figures are determined empirically
			t.fontSize = targetSize;
			//Note: the font size is apparently capped at 32
		}
	}
}
