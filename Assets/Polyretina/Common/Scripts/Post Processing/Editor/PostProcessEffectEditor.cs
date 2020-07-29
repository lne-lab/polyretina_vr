using UnityEditor;

namespace LNE.PostProcessing.UI
{
	using LNE.UI;	

	[CustomEditor(typeof(PostProcessEffect))]
	public class PostProcessEffectEditor : ExtendedEditor<PostProcessEffect>
	{
		public override void OnInspectorGUI()
		{
			BeginInspectorGUI();

			var source = Field<PostProcessEffect.Source>(true, "_source");
			switch (source)
			{
				case PostProcessEffect.Source.Shader:
					Field(true, "_shader");
					break;
				case PostProcessEffect.Source.Material:
					Field(true, "_material");
					break;
			}

			EndInspectorGUI();
		}
	}
}
