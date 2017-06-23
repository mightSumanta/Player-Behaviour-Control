using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerControl
{
	public class XYMouseLookScript : MonoBehaviour 
	{
		public GameObject mouseLookManagerHolder;
		private MouseLookManager mouseLookManagerScript;

        void OnEnable()
        {
			mouseLookManagerScript = mouseLookManagerHolder.GetComponent<MouseLookManager>();
        }

        void LateUpdate () 
		{

            mouseLookManagerScript.xRotation = Mathf.Clamp(mouseLookManagerScript.xRotation,
                mouseLookManagerScript.minXRotation, mouseLookManagerScript.maxXRotation);

            mouseLookManagerScript.currentXRotation = Mathf.SmoothDamp(mouseLookManagerScript.currentXRotation,
                mouseLookManagerScript.xRotation, ref mouseLookManagerScript.xRotationVelocity, mouseLookManagerScript.lookSmoothDampTime);

            mouseLookManagerScript.currentYRotation = Mathf.SmoothDamp(mouseLookManagerScript.currentYRotation,
               mouseLookManagerScript.yRotation, ref mouseLookManagerScript.yRotationVelocity, mouseLookManagerScript.lookSmoothDampTime);

            transform.rotation = Quaternion.Euler(mouseLookManagerScript.currentXRotation, mouseLookManagerScript.currentYRotation - 180, 0);
		}

	}

}
