#pragma warning disable 649

using System;
using UnityEngine;

namespace LNE.ProstheticVision
{
	using static EnumExts.EnumExtensions;

	[Serializable]
	public class EpiretinalData
	{
		public enum Type { Phosphene, Axon }

		/*
		 * Public fields
		 */

		[SerializeField]
		private Texture2D[] data;

		/*
		 * Private fields
		 */

#if UNITY_EDITOR
		[SerializeField]
		public bool[] headsetFoldout = new bool[Count<HeadsetModel>()];

		[SerializeField]
		public bool[] patternFoldout = new bool[Count<HeadsetModel>() * Count<ElectrodePattern>()];
#endif

		/*
		 * Public methods
		 */

		public Texture2D GetPhospheneTexture(HeadsetModel headset, ElectrodePattern pattern, ElectrodeLayout layout)
		{
			return data[GetPhospheneTextureIndex(headset, pattern, layout)];
		}

		public Texture2D GetAxonTexture(HeadsetModel headset)
		{
			return data[GetAxonTextureIndex(headset)];
		}

		public void SetPhospheneTexture(HeadsetModel headset, ElectrodePattern pattern, ElectrodeLayout layout, Texture2D asset)
		{
			data[GetPhospheneTextureIndex(headset, pattern, layout)] = asset;
		}

		public void SetAxonTexture(HeadsetModel headset, Texture2D asset)
		{
			data[GetAxonTextureIndex(headset)] = asset;
		}

		public bool IsReady(HeadsetModel headset)
		{
			var ready = true;
			foreach (var pattern in Enumerate<ElectrodePattern>())
			{
				ready &= IsPhospheneTextureReady(headset, pattern);
			}

			return ready && IsAxonTextureReady(headset);
		}

		public bool IsPhospheneTextureReady(HeadsetModel headset, ElectrodePattern pattern)
		{
			var ready = true;
			foreach (var layout in Enumerate<ElectrodeLayout>())
			{
				ready &= IsPhospheneTextureReady(headset, pattern, layout);
			}

			return ready;
		}

		public bool IsPhospheneTextureReady(HeadsetModel headset, ElectrodePattern pattern, ElectrodeLayout layout)
		{
			return GetPhospheneTexture(headset, pattern, layout) != null;
		}

		public bool IsAxonTextureReady(HeadsetModel headset)
		{
			return GetAxonTexture(headset) != null;
		}

		/*
		 * Private methods
		 */

		private int GetPhospheneTextureIndex(HeadsetModel headset, ElectrodePattern pattern, ElectrodeLayout layout)
		{
			var numPatterns = Count<ElectrodePattern>();
			var numLayouts = Count<ElectrodeLayout>();

			var headsetIndex = headset.GetIndex();
			var patternIndex = pattern.GetIndex();
			var layoutIndex = layout.GetIndex();

			var headsetOffset = headsetIndex * (numPatterns * numLayouts + 1);
			var patternOffset = patternIndex * numLayouts;

			return headsetOffset + patternOffset + layoutIndex;
		}

		private int GetAxonTextureIndex(HeadsetModel headset)
		{
			var numPatterns = Count<ElectrodePattern>();
			var numLayouts = Count<ElectrodeLayout>();

			var headsetIndex = headset.GetIndex();

			var headsetOffset = headsetIndex * (numPatterns * numLayouts + 1);
			var axonOffset = numPatterns * numLayouts;

			return headsetOffset + axonOffset;
		}
	}
}
