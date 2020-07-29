using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LNE.UI
{
	using ArrayExts;

	/// <summary>
	/// Fill referenced dropdown menu with enum values
	/// </summary>
	public class EnumDropdown : MonoBehaviour
	{
		/*
		 * Editor fields
		 */

#pragma warning disable 649

		[SerializeField]
		private Dropdown _dropdown;

		[SerializeField]
		private string _enumName;

		[SerializeField]
		private int _startingValue;

#pragma warning restore 649

		/*
		 * Public properties
		 */

		public Type EnumType
		{
			get => Type.GetType(_enumName);
			set => _enumName = value.Name;
		}

		/*
		 * Unity callbacks
		 */

		void Start()
		{
			var enumType = EnumType;
			var enumValues = Enum.GetNames(enumType).Apply((ev) => UnityApp.ToDisplayFormat(ev));

			_dropdown.ClearOptions();
			_dropdown.AddOptions(new List<string>(enumValues));
			_dropdown.value = _startingValue;
		}
	}
}
