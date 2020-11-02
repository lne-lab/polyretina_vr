using UnityEngine;

namespace LNE
{
	/// <summary>
	/// WASD controller
	/// </summary>
	public class WASDController : MonoBehaviour
	{
		/*
		 * Editor fields
		 */

		[Range(0, 1)]
		public float	lookSpeed = .5f,
						moveSpeed = .5f;

		/*
		 * Private fields
		 */

		private bool paused;
		private bool canFly;

		/*
		 * Unity callbacks
		 */

		void Awake()
		{
			canFly = UnityApp.InputAxisExists("Fly");
		}

		void Update()
		{
			paused ^= Input.GetKeyDown(KeyCode.Escape);
		}

		void FixedUpdate()
		{
			if (paused)
				return;

			var x = Input.GetAxis("Mouse X")	* (lookSpeed * 5);
			var y = Input.GetAxis("Mouse Y")	* (lookSpeed * 5);
			var h = Input.GetAxis("Horizontal")	* (moveSpeed / 2);
			var v = Input.GetAxis("Vertical")	* (moveSpeed / 2);
			
			var f = canFly ? Input.GetAxis("Fly") * (moveSpeed / 2) : 0;

			transform.Rotate(0, x, 0, Space.World);
			transform.Rotate(-y, 0, 0, Space.Self);

			transform.Translate(0, f, 0, Space.World);
			transform.Translate(h, 0, v, Space.Self);
		}
	}
}
