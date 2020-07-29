using UnityEngine;

namespace LNE.Enrico
{
	public enum ControlScheme
	{
		MOUSE,
		KEYBOARD,
		GAMEPAD,
		FOVE // jake added this
	}

	public class PivotRotator : MonoBehaviour
	{

		public ControlScheme control = ControlScheme.FOVE; // jake changed this from ControlScheme.MOUSE
		public bool active = true;

		// Use this for initialization
		void Start()
		{

		}
		// Update is called once per frame
		void FixedUpdate()
		{ // jake changed this to fixed update
			if (active)
			{
				Vector3 input = new Vector3(0.0f, 0.0f, 0.0f);
				switch (control)
				{
					case ControlScheme.MOUSE:
						{
							input = new Vector3
							{
								x = -10f * Input.GetAxis("Mouse Y"),
								y = 10f * Input.GetAxis("Mouse X"),
								z = 100f * Input.GetAxis("Mouse ScrollWheel")
							};
							break;
						}

					case ControlScheme.KEYBOARD:
						{
							// Vertical means rotation across the X axis, same for Horizontal
							input = new Vector3
							{
								x = -10f * Input.GetAxis("Vertical"),
								y = 10f * Input.GetAxis("Horizontal"),
								z = 10f * Input.GetAxis("Orthogonal")
							};
							// The camera is tethered to an empty transform in the center, so there is no need to compute rotations.
							// Rotate allows me to rotate on the view's axis, rather than the world axes.
							break;
						}
					case ControlScheme.GAMEPAD:
						{
							// Vertical means rotation across the X axis, same for Horizontal
							input = new Vector3
							{
								x = -4f * Input.GetAxis("Vertical"), // jake changed the values from 20 to 4
								y = 4f * Input.GetAxis("Horizontal"),
								z = 4f * Input.GetAxis("Orthogonal")
							};
							// The camera is tethered to an empty transform in the center, so there is no need to compute rotations.
							// Rotate allows me to rotate on the view's axis, rather than the world axes.
							break;
						}
					// jake added this
					case ControlScheme.FOVE:
						transform.rotation = FoveInterface.GetHMDRotation();
						break;
				}

				if (control != ControlScheme.FOVE) // jake added this
				{
					this.transform.Rotate(input);
				}
			}
		}

		public void SetAsIdentity()
		{
			transform.rotation = Quaternion.identity;
		}
	}
}
