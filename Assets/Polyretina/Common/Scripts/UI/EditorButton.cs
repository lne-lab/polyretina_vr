using System;

namespace LNE.UI
{
	/// <summary>
	/// Creates a button in the monobehaviour inspector that invokes a given method.
	/// Add the ExecuteInEditMode attribute to make the button work in edit mode
	/// </summary>
	[Serializable]
	public class EditorButton
	{
		private Action action;

		public EditorButton(Action action)
		{
			this.action = action;
		}

		public void Invoke()
		{
			action?.Invoke();
		}
	}
}
