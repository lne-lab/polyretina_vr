using UnityEngine;

namespace LNE.Demos.Objects
{
	public class ObjectRotator : MonoBehaviour
	{
		[SerializeField, Range(0, 10)]
		private float speed = 5f;

		void FixedUpdate()
		{
			if (Input.GetMouseButton(0))
			{
				var x = Input.GetAxis("Mouse X");
				var y = Input.GetAxis("Mouse Y");

				transform.Rotate(-y * speed, 0, 0, Space.World);
				transform.Rotate(0, -x * speed, 0, Space.Self);
			}
		}
	}
}
