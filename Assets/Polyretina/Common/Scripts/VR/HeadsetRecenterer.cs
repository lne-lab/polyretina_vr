#pragma warning disable 649

using UnityEngine;

namespace LNE.VR
{
	using UI;
	using UI.Attributes;

	/// <summary>
	/// Recenter VR Headset to original position/orientation
	/// </summary>
	public class HeadsetRecenterer : Singleton<HeadsetRecenterer>
	{
		[SerializeField]
		private Transform _camera;

		[SerializeField]
		private Transform _cameraParent;

		[SerializeField, CustomLabel(label = "Recenter")]
		private EditorButton _recenterButton;

		private Vector3 startingPosition;
		private Quaternion startingRotation;

		void Start()
		{
			_recenterButton = new EditorButton(Recenter);

			startingPosition = _cameraParent.position;
			startingRotation = _cameraParent.rotation;
		}

		public void Recenter()
		{
			RecenterRotation();
			RecenterPosition();
		}

		public void RecenterRotation()
		{
			_camera.rotation.ToAngleAxis(out float viveAngle, out Vector3 viveAxis);
			startingRotation.ToAngleAxis(out float startingAngle, out Vector3 startingAxis);

			_cameraParent.RotateAround(_camera.position, viveAxis, -viveAngle);
			_cameraParent.RotateAround(_camera.position, startingAxis, startingAngle);
		}

		public void RecenterPosition()
		{
			var viveToParent = _cameraParent.position - _camera.position;
			var parentToCenter = -_cameraParent.position;
			var centerToStart = startingPosition;

			_cameraParent.Translate(viveToParent, Space.World);
			_cameraParent.Translate(parentToCenter, Space.World);
			_cameraParent.Translate(centerToStart, Space.World);
		}
	}
}
