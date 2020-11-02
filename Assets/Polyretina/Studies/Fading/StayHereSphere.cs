using UnityEngine;
using UnityEngine.Events;

public class StayHereSphere : MonoBehaviour
{
	[SerializeField]
	private Transform _target;

	[SerializeField]
	private float _radius;

	[SerializeField]
	private UnityEvent _onLeaveSphere;

	[SerializeField]
	private UnityEvent _onEnterSphere;

	private bool inside;

	void Update()
	{
		if (Vector3.Distance(_target.position, transform.position) > _radius)
		{
			if (inside)
			{
				_onLeaveSphere.Invoke();
				inside = false;
			}
		}
		else
		{
			if (inside == false)
			{
				_onEnterSphere.Invoke();
				inside = true;
			}
		}
	}

	public void ThisToTarget()
	{
		transform.position = _target.position;
	}

	public void TargetToThis()
	{
		_target.position = transform.position;
	}
}
