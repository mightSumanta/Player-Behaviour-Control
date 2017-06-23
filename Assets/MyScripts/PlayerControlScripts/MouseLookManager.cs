using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerControl
{
	public class MouseLookManager : MonoBehaviour 
	{

        [HideInInspector]
        public float xRotation;
        [HideInInspector]
        public float yRotation;
        public float lookSensitivity = 5;
        public float currentYRotation;
        public float minXRotation = -90;
        public float maxXRotation = +90;
        public float currentXRotation;
        public float yRotationVelocity;
        public float xRotationVelocity;
        public float lookSmoothDampTime = 0.1f;

        void Update () 
		{
            xRotation += Input.GetAxis("Mouse Y") * lookSensitivity;
            yRotation += Input.GetAxis("Mouse X") * lookSensitivity;
        }

	}

}
