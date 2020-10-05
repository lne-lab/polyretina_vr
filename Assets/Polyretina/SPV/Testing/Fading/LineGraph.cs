#pragma warning disable 649

using System.Collections.Generic;
using UnityEngine;

namespace LNE.ProstheticVision.Fading
{
	using ArrayExts;
	using TransformExts;

	public class LineGraph : MonoBehaviour
	{
		/*
		 * Editor fields
		 */

		[Space]

		[SerializeField]
		private float minValue = 0;

		[SerializeField]
		private float maxValue = 1;

		[Space]

		[SerializeField]
		private int updateFrequency = 1;

		[SerializeField]
		private float timeScale = .25f;

		[Space]

		[SerializeField]
		private LineRenderer lineRenderer = null;

		[SerializeField]
		private TextMesh minValueText;

		[SerializeField]
		private TextMesh maxValueText;

		[SerializeField]
		private TextMesh valueText;

		[SerializeField]
		private bool valueFollowLine;

		[Space]

		[SerializeField]
		private GameObject tickTemplate = null;

		/*
		 * Private fields
		 */

		private List<(GameObject, float)> ticks;
		private float lastTickTime;
		private int updateCount;
		private float cameraAspect;

		private float _value;
		public float Value
		{
			get
			{
				return _value;
			}

			set
			{
				_value = AuxMath.Normalise(value, minValue, maxValue);
			}
		}

		/*
		 * Private properties
		 */

		private float scaledTime
		{
			get
			{
				return Time.time * timeScale;
			}
		}

		/*
		 * Unity callbacks
		 */

		void Start()
		{
			ticks = new List<(GameObject, float)>();
			AddTick(1, true);

			UpdateCameraAspect();
		}

		void Update()
		{
			// select graph camera
			if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonDown(0))
			{
				UpdateCameraAspect();
			}

			UpdateLine();
			UpdateTicks();

			// update texts
			minValueText.text = minValue.ToString("N3");
			maxValueText.text = maxValue.ToString("N3");

			if (float.IsNaN(Value) == false)
			{
				valueText.text = AuxMath.Formalise(Value, minValue, maxValue).ToString("N5");

				if (valueFollowLine)
				{
					var height = Value * 1.95f - .9f;
					valueText.transform.SetY(lineRenderer.transform.position.y + height);
				}
			}

			updateCount++;
		}

		/*
		 * Public methods
		 */

		public void AddTick(float height, bool updateTime = false)
		{
			var tick = Instantiate(tickTemplate);
			ticks.Add((tick, scaledTime));
			tick.transform.parent = transform;
			tick.transform.localScale = Vector3.Scale(
				tick.transform.localScale,
				new Vector3(1, height, 1)
			);

			if (updateTime)
			{
				lastTickTime = scaledTime;
			}
		}

		/*
		 * Private methods
		 */

		private void UpdateLine()
		{
			if (updateCount % updateFrequency == 0)
			{
				// move line
				lineRenderer.transform.SetX(cameraAspect - scaledTime);

				// add point
				lineRenderer.positionCount++;
				lineRenderer.SetPosition(
					lineRenderer.positionCount - 1,
					new Vector3(scaledTime, (Value - 0.5f) * 2, 0)
				);

				// remove finished positions
				var positions = new Vector3[lineRenderer.positionCount];
				var positionCount = lineRenderer.GetPositions(positions);

				var newPositions = positions.Remove(
					(p) => p.x + lineRenderer.transform.position.x < -cameraAspect
				);

				lineRenderer.positionCount = newPositions.Length;
				lineRenderer.SetPositions(newPositions);
			}
		}

		private void UpdateTicks()
		{
			// add second tick
			if (lastTickTime <= scaledTime - timeScale)
			{
				AddTick(1, true);
			}

			// move all ticks
			if (updateCount % updateFrequency == 0)
			{
				foreach (var tick in ticks)
				{
					tick.Item1.transform.position = new Vector3(
						cameraAspect + tick.Item2 - scaledTime,
						1000 - 1.25f,
						2
					);
				}
			}

			// remove finished ticks
			if (ticks.Count > 0 && ticks[0].Item1.transform.position.x < -cameraAspect)
			{
				Destroy(ticks[0].Item1);
				ticks.RemoveAt(0);
			}
		}

		private void UpdateCameraAspect()
		{
			// .95f to allow some whitespace for min/max/current value texts
			cameraAspect = GetComponentInChildren<Camera>().aspect * .95f;
		}
	}
}
