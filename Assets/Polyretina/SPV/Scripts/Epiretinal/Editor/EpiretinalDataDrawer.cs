using UnityEngine;
using UnityEditor;

namespace LNE.ProstheticVision.UI
{
    using LNE.UI;
	using static EnumExts.EnumExtensions;

	[CustomPropertyDrawer(typeof(EpiretinalData))]
    public class EpiretinalDataDrawer : ExtendedPropertyDrawer
	{
		private static int arraySize
		{
			get
			{
				return Count<HeadsetModel>() + Count<HeadsetModel>() * Count<ElectrodePattern>() * Count<ElectrodeLayout>();
			}
		}

		[InitializeOnLoadMethod]
		static void AddDrawer()
		{
			AddDrawer(typeof(EpiretinalData), OnGUI, GetPropertyHeight);
		}

		public static void OnGUI(ExtendedPropertyDrawer drawer)
		{
			var dataProp = drawer.Property.FindPropertyRelative("data");

			dataProp.arraySize = arraySize;

			var i = 0;
			var hfi = 0;
			var pfi = 0;
			foreach (var headset in Enumerate<HeadsetModel>())
			{
				var hfp = drawer.Property
								.FindPropertyRelative("headsetFoldout")
								.GetArrayElementAtIndex(hfi);

				hfp.boolValue = EditorGUI.Foldout(drawer.position, hfp.boolValue, UnityApp.ToDisplayFormat(headset));
				drawer.position.y += 18;

				foreach (var pattern in Enumerate<ElectrodePattern>())
				{
					EditorGUI.indentLevel++;

					var pfp = drawer.Property
									.FindPropertyRelative("patternFoldout")
									.GetArrayElementAtIndex(pfi);

					if (hfp.boolValue)
					{
						pfp.boolValue = EditorGUI.Foldout(drawer.position, pfp.boolValue, UnityApp.ToDisplayFormat(pattern));
						drawer.position.y += 18;
					}

					foreach (var layout in Enumerate<ElectrodeLayout>())
					{
						if (hfp.boolValue && pfp.boolValue)
						{
							var phosTex = dataProp.GetArrayElementAtIndex(i);
							EditorGUI.ObjectField(drawer.position, phosTex, new GUIContent(UnityApp.ToDisplayFormat(layout)));
							drawer.position.y += 18;
						}

						i++;
					}

					pfi++;

					EditorGUI.indentLevel--;
				}

				if (hfp.boolValue)
				{
					var axonTex = dataProp.GetArrayElementAtIndex(i);
					EditorGUI.ObjectField(drawer.position, axonTex, new GUIContent("Axon"));
					drawer.position.y += 18;
				}

				i++;
				hfi++;
			}
		}

		public static float GetPropertyHeight(ExtendedPropertyDrawer drawer)
		{
			var height = 0;

			var hfa = drawer.Property.FindPropertyRelative("headsetFoldout");
			var pfa = drawer.Property.FindPropertyRelative("patternFoldout");

			hfa.arraySize = Count<HeadsetModel>();
			pfa.arraySize = Count<HeadsetModel>() * Count<ElectrodePattern>();

			var i = 0;
			var hfi = 0;
			var pfi = 0;
			foreach (var headset in Enumerate<HeadsetModel>())
			{
				var hfp = hfa.GetArrayElementAtIndex(hfi);
				
				height += 18;

				foreach (var pattern in Enumerate<ElectrodePattern>())
				{
					var pfp = pfa.GetArrayElementAtIndex(pfi);

					if (hfp.boolValue)
					{
						height += 18;
					}

					foreach (var layout in Enumerate<ElectrodeLayout>())
					{
						if (hfp.boolValue && pfp.boolValue)
						{
							height += 18;
						}

						i++;
					}

					pfi++;
				}

				if (hfp.boolValue)
				{
					height += 18;
				}

				i++;
				hfi++;
			}

			return height;
		}
	}
}
