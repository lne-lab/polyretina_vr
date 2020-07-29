using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleMove : MonoBehaviour
{
	float deltaTime = 0.0f;
	public float velocity = 0.1f;
	public Transform rightFrontWheel;
	public Transform leftFrontWheel;
	public Transform rightBackWheel;
	public Transform leftBackWheel;
	public float wheelRadius;
	[Tooltip("If the X axis is pointing inwards")]
	public bool reverseRotation;
	float currAngle = 0.0f;

	private bool stop;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.F11))
		{
			stop = !stop;
		}

		if (stop)
			return;

		float fps = 60.0f;
		deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
		fps = 1.0f / deltaTime;

        // Let's try to cut it
        fps = 60.0f;
        
		float velmod = velocity/fps;
		this.transform.position += this.transform.forward * velmod;
			
		float omega = 180*(velmod/wheelRadius)/3.1415f;
		currAngle += omega;
		rightFrontWheel.localRotation = Quaternion.Euler(new Vector3(currAngle, 0, 0));
		rightBackWheel.localRotation = Quaternion.Euler(new Vector3(currAngle, 0, 0));
		leftFrontWheel.localRotation = Quaternion.Euler(new Vector3(currAngle, 0, 0));
		leftBackWheel.localRotation = Quaternion.Euler(new Vector3(currAngle, 0, 0));
		
    }
}
